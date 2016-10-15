using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.Filters;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Database.Model.HelperClasses;
using DSA.Database.Model.Repositories;
using log4net;

namespace DSA.Database.Model
{
    public class LocalCache
    {
        #region constructor

        public void Initialize()
        {
            InitRepositories();
            InitInterventionsTask = new Task(() =>
            {
                Thread.CurrentThread.Name = "InitInterventions";
                PopulateInterventionsDictionaryAsynch(CurrentUser.Id);
                //PopulateInterventiosDictionary();
                // AddYearToInterventionsDictionary();
            });
            InitSettingsTask = new Task(() =>
            {
                Thread.CurrentThread.Name = "InitSettings";
                InitSettings();
            });
            InitPatientsTask = new Task(() =>
            {
                Thread.CurrentThread.Name = "InitPatients";
                PopulatePatientsRepository();
            });
            InitStartTAsks();
        }

        #endregion costructor

        #region instance
        private static readonly LocalCache instance = new LocalCache();
        public static LocalCache Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion instance

        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalUser CurrentUser { get; set; }

        public LocalPatient SelectedPatient { get; set; }

        public PatientsRepository PatientsRepository { get; set; }

        public Dictionary<SettingsItemEnum, List<SettingsItem>> SettingsItems { get; set; }

        public Dictionary<int, Dictionary<int, List<LocalIntervention>>> InterventionsDictionary = new Dictionary<int, Dictionary<int, List<LocalIntervention>>>();

        public Dictionary<int, string> MonthsNames { get; set; }

        public List<LocalFilterGroup> FilterGroups { get; set; }

        public List<InterventionDetails> FilteredInterventionDetails = new List<InterventionDetails>();

        public List<LocalTechnician> Technicians { get; set; }

        public List<Task> StartDTSTasks;

        public PaymentInterval PaymentInterval;

        public List<LocalFilterGroup> LocalfilterGroups { get; set; }

        public List<string> months { get; set; }

        public IntervalIntervention IntervalIntervention = new IntervalIntervention();

        public Dictionary<int, List<InterventionDetails>> SelectedInterventions = new Dictionary<int, List<InterventionDetails>>();

        public List<LocalWork> Works { get; set; }

        public LocalProgramInfo ProgramInfo { get; set; }

        #endregion Fields

        #region Tasks

        private Task InitInterventionsTask;
        private Task InitSettingsTask;
        private Task InitPatientsTask;
        #endregion Tasks

        #region Methods

        public void InitStartTAsks()
        {
            StartDTSTasks = new List<Task>();
            StartDTSTasks.Add(InitSettingsTask);
            StartDTSTasks.Add(InitPatientsTask);
            StartDTSTasks.Add(InitInterventionsTask);
            foreach (var startDtsTask in StartDTSTasks)
            {
                startDtsTask.Start();
            }
            Log.Info("LOGIN-->WAIT FOR TASKS -->InitStartTAsks");
            Task.WaitAll(StartDTSTasks.ToArray());
            //          StartDTSTasks.Add(initre);

        }

        private void InitRepositories()
        {
            PatientsRepository = new PatientsRepository();
            months = new List<string>() { "Ianuarie", "Februarie", "Martie", "Aprilie", "Mai", "Iunie", "Iulie", "August", "Septembrie", "Octombrie", "Noiembrie", "Decembrie" };
            MonthsNames = new Dictionary<int, string>();
            for (int i = 0; i < 12; i++)
            {
                MonthsNames.Add(i, months[i]);
            }
            PaymentInterval = new PaymentInterval();
        }

        public void PopulateInterventionsDictionaryAsynch(int userId)
        {
            InterventionsDictionary = new Dictionary<int, Dictionary<int, List<LocalIntervention>>>();
            var currentYears = DatabaseHandler.Instance.GetLocalYears(userId);
            Object thisLock = new Object();
            List<Task> getYearTaskList = new List<Task>();
            foreach (var currentYear in currentYears)
            {
                getYearTaskList.Add(new Task(delegate
                {

                    if (!InterventionsDictionary.ContainsKey(currentYear.YearNb))
                    {
                        Dictionary<int, List<LocalIntervention>> monthsIntervensions = new Dictionary<int, List<LocalIntervention>>();
                        foreach (var localIntervention in currentYear.Interventions)
                        {
                            int monthNumber = localIntervention.DateHourDetail.Date.Month;
                            if (monthsIntervensions.ContainsKey(monthNumber))
                            {
                                monthsIntervensions[monthNumber].Add(localIntervention);
                            }
                            else
                            {
                                monthsIntervensions.Add(localIntervention.DateHourDetail.Date.Month,
                                    new List<LocalIntervention>() { localIntervention });
                            }
                        }
                        lock (thisLock)
                        {
                            InterventionsDictionary.Add(currentYear.YearNb, monthsIntervensions);
                        }
                    }
                }));
            }
            foreach (var task in getYearTaskList)
            {
                task.Start();
            }
            Task.WaitAll(getYearTaskList.ToArray());
        }

        public void AddIntervention(LocalIntervention localIntervention)
        {
            int year = localIntervention.DateHourDetail.Date.Year;
            int month = localIntervention.DateHourDetail.Date.Month;
            if (InterventionsDictionary.ContainsKey(year))
            {
                if (InterventionsDictionary[year].ContainsKey(month))
                {
                    InterventionsDictionary[year][month].Add(localIntervention);
                }
                else
                {
                    InterventionsDictionary[year].Add(month, new List<LocalIntervention>() { localIntervention });
                }
            }
            else
            {
                InterventionsDictionary.Add(year, new Dictionary<int, List<LocalIntervention>>());
                InterventionsDictionary[year].Add(month, new List<LocalIntervention>() { localIntervention });
            }
        }

        private async void InitSettings()
        {
            SettingsItems = new Dictionary<LocalCache.SettingsItemEnum, List<SettingsItem>>();
            List<SettingsItem> areas = new List<SettingsItem>(await DatabaseHandler.Instance.GetAreas());
            SettingsItems.Add(SettingsItemEnum.Zona, areas);
            List<SettingsItem> works = new List<SettingsItem>(await DatabaseHandler.Instance.GetWorks());
            SettingsItems.Add(SettingsItemEnum.Manopera, works);
            Works = await DatabaseHandler.Instance.GetWorksWithTypes();
            List<SettingsItem> locations = new List<SettingsItem>(await DatabaseHandler.Instance.GetlLocations());
            SettingsItems.Add(SettingsItemEnum.Locatie, locations);
            List<SettingsItem> materials = new List<SettingsItem>(await DatabaseHandler.Instance.GetMaterials());
            SettingsItems.Add(SettingsItemEnum.Material, materials);
            List<SettingsItem> technicians = new List<SettingsItem>(await DatabaseHandler.Instance.GetTechnicians());
            SettingsItems.Add(SettingsItemEnum.Tehnician, technicians);
            Technicians = await DatabaseHandler.Instance.GetTechnicians();
        }

        private async void PopulatePatientsRepository()
        {
            var patients = await DatabaseHandler.Instance.GetPatients(CurrentUser.Id);
            PatientsRepository.SetPatient(patients);
        }

        #region SelectedInterventions

        public void AddSelectedInterventions(int monthYearId, List<InterventionDetails> interventions)
        {
            if (SelectedInterventions.ContainsKey(monthYearId))
            {
                SelectedInterventions[monthYearId] = interventions;
            }
            else
            {
                SelectedInterventions.Add(monthYearId, interventions);
            }
        }




        #endregion SelectedInterventions

        #endregion Methods

        #region delegates

        public Delegate PaymentIntervalSelecteDelegate { get; set; }


        #endregion delegates

        #region BackUp

        public void SaveDatabase()
        {
            string fileName = DateTime.Now.ToShortDateString() + DateTime.Now.Ticks + ".sdf";
            string sourcePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DTS\";
            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DTS\BackUp";

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, "dsa.sdf");
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);
        }

        #endregion BackUp

        public enum SettingsItemEnum { Locatie, Material, Manopera, Tehnician, Zona, Luna, An };
    }

}
