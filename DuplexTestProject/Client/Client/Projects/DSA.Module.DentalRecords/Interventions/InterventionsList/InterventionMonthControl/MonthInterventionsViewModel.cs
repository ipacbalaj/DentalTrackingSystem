using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.Entities;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.Filters;
using DSA.Database.Model.Helpers;
using DSA.Module.DentalRecords.Helpers;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionMonthControl
{
    public class MonthInterventionsViewModel : BindableObjectListBase<MonthInterventionsViewModel>
    {
        #region Constructor

        public MonthInterventionsViewModel(YearInterventionsViewModel parent, List<LocalIntervention> localInterventions, int monthNumber)
        {
            Parent = parent;
            SelectedRows = new ObservableCollection<InterventionDetails>();
            PopulateInterventions(localInterventions);
            MonthName = LocalCache.Instance.MonthsNames[monthNumber - 1];
            MonthId = monthNumber;
            DeleteRowCommand = new DelegateCommand<InterventionDetails>(OnDelete);
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        void SelectedTasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RaisePropertyChanged("SelectedInterv");
                //SelectedInterv = new ObservableCollection<InterventionDetails>(SelectedInterv);
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Constructor

        #region Properties


        public ICommand DeleteRowCommand { get; private set; }

        private readonly IEventAggregator eventAggregator;

        private InterventionDetails selectedIntervention;
        public InterventionDetails SelectedIntervention
        {
            get { return selectedIntervention; }
            set
            {
                if (value == selectedIntervention)
                    return;
                selectedIntervention = value;
                //                Parent.Parent.Parent.AddInterventionTileViewModel.SelectedInterventionDetails = selectedIntervention;
                //                if (LocalCache.Instance.IntervalIntervention.SetIntervention(selectedIntervention))
                //                {
                //
                //                }
                OnPropertyChanged();
            }
        }

        public YearInterventionsViewModel Parent { get; set; }

        private SymbolIconButtonViewModel deleteBtn;
        public SymbolIconButtonViewModel DeleteBtn
        {
            get { return deleteBtn; }
            set
            {
                if (deleteBtn == value)
                    return;
                deleteBtn = value;
                OnPropertyChanged();
            }
        }

        private Visibility contentVisibility = Visibility.Collapsed;
        public Visibility ContentVisibility
        {
            get { return contentVisibility; }
            set
            {
                if (contentVisibility != value)
                {
                    contentVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<InterventionDetails> interventions;
        public ObservableCollection<InterventionDetails> Interventions
        {
            get { return interventions; }
            set
            {
                if (interventions != value)
                {
                    interventions = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<InterventionDetails> seenInterventions;
        public ObservableCollection<InterventionDetails> SeenInterventions
        {
            get { return seenInterventions; }
            set
            {
                if (seenInterventions != value)
                {
                    seenInterventions = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<InterventionDetails> selectedInterv;
        public List<InterventionDetails> SelectedInterv
        {
            get { return selectedInterv; }
            set
            {
                if (selectedInterv != value)
                {
                    selectedInterv = value;
                    //                    foreach (var interventionDetailse in selectedInterv)
                    //                    {
                    //                        var selectedIntervention = SeenInterventions.FirstOrDefault(item => item.Id == interventionDetailse.Id);
                    //                        selectedIntervention.Brush = BackgroundColors.BackgroundFilled;
                    //                    }
                    //                    var firstIntervention = selectedInterv.FirstOrDefault();
                    //                    if(firstIntervention!=null)
                    LocalCache.Instance.AddSelectedInterventions(Parent.Year * 100 + MonthId, selectedInterv);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<InterventionDetails> SelectedRows { get; private set; }

        private string monthName;
        public string MonthName
        {
            get { return monthName; }
            set
            {
                if (monthName != value)
                {
                    monthName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int monthId;
        public int MonthId
        {
            get { return monthId; }
            set
            {
                if (monthId != value)
                {
                    monthId = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground = BackgroundColors.MonthTileColor;
        public Brush ContentBackGround
        {
            get { return contentBackground; }
            set
            {
                if (contentBackground != value)
                {
                    contentBackground = value;
                    OnPropertyChanged();
                }
            }
        }

        private string expanderRotation = "270";
        public string ExpanderRotation
        {
            get { return expanderRotation; }
            set
            {
                if (expanderRotation != value)
                {
                    expanderRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods
        public override void OnClick(bool shouldSelect)
        {
            IsSelected = shouldSelect;
            if (shouldSelect)
            {
                ContentVisibility = Visibility.Visible;
                ContentBackGround = BackgroundColors.MonthTileColorMouseOver;
                ExpanderRotation = "90";
                SeenInterventions = Interventions;
            }
            else
            {
                ContentVisibility = Visibility.Collapsed;
                ContentBackGround = BackgroundColors.MonthTileColor;
                ExpanderRotation = "270";
                SeenInterventions = new ObservableCollection<InterventionDetails>();
            }
        }

        public void MouseOn()
        {
            if (!IsSelected)
            {
                ContentBackGround = BackgroundColors.MonthTileColorMouseOver;
            }
        }

        private int numarInterventii;
        public int NumarInterventii
        {
            get { return numarInterventii; }
            set
            {
                if (numarInterventii != value)
                {
                    numarInterventii = value;
                    OnPropertyChanged();
                }
            }
        }

        private double totalIncasari;
        public double TotalIncasari
        {
            get { return totalIncasari; }
            set
            {
                if (totalIncasari != value)
                {
                    totalIncasari = value;
                    OnPropertyChanged();
                }
            }
        }

        private double totalProcent;

        public double TotalProcent
        {
            get { return totalProcent; }
            set
            {
                if (value == totalProcent)
                    return;
                totalProcent = value;
                OnPropertyChanged();
            }
        }

        public void MouseOff()
        {
            if (!IsSelected)
            {
                ContentBackGround = BackgroundColors.MonthTileColor;
            }
        }

        public int PopulateInterventions(List<LocalIntervention> localInterventions)
        {
            Interventions = new ObservableCollection<InterventionDetails>();
            double totalIncasari = 0;
            double totalProcent = 0;
            foreach (var localIntervention in localInterventions)
            {
                Interventions.Add(localIntervention.ToInterventionDetails());
                totalIncasari += localIntervention.Revenue;
                totalProcent += localIntervention.Percent;
                //                TotalIncasari += localIntervention.Revenue;
                //                TotalProcent += localIntervention.Percent;
            }
            NumarInterventii = Interventions.Count;
            TotalProcent = totalProcent;
            TotalIncasari = totalIncasari;
            //SeenInterventions = Interventions;
            return NumarInterventii;
        }

        public void AddIntervention(InterventionDetails newlyAddedIntervention)
        {
            //SeenInterventions.Add(newlyAddedIntervention);
            SelectedIntervention = newlyAddedIntervention;
            Interventions.Add(newlyAddedIntervention);
            NumarInterventii += 1;
            TotalIncasari += newlyAddedIntervention.Revenue;
            TotalProcent += newlyAddedIntervention.Percent;
            if (newlyAddedIntervention.NewlyAdded && !IsSelected)
            {
                this.OnSelected(this);
            }
            // SelectedIntervention = newlyAddedIntervention;
        }

        public void RemoveIntervention(InterventionDetails removedIntervention)
        {
            NumarInterventii -= 1;
            TotalIncasari -= removedIntervention.Revenue;
            TotalProcent -= removedIntervention.Percent;
            Parent.TotalIncasari -= removedIntervention.LocalIntervention.Revenue;
            Parent.TotalProfit -= removedIntervention.LocalIntervention.Percent;
            Parent.NumarInterventii -= 1;
            Interventions.Remove(removedIntervention);
        }

        public void FilterInterventions()
        {
            List<InterventionDetails> tempInterventions = new List<InterventionDetails>();
            foreach (var interventionDetailse in Interventions)
            {
                tempInterventions.Add(interventionDetailse);
            }
            if (LocalCache.Instance.FilterGroups != null)
            {
                var activeGroups = LocalCache.Instance.FilterGroups.Where(item => item.IsActive);
                foreach (var localFilterGroup in activeGroups)
                {
                    var activeFIlters = localFilterGroup.Filters.Where(item => item.IsActive);
                    foreach (var localFilter in activeFIlters)
                    {
                        var selectedItems = GetSelectedItemsNames(localFilter.FilterItems);
                        if (selectedItems.Count > 0)
                            switch (localFilter.Type)
                            {
                                case "Zona:":
                                    tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Area)).ToList();
                                    break;
                                case "Manopera:":
                                    tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Work)).ToList();
                                    break;
                                case "Material:":
                                    tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Material)).ToList();
                                    break;
                                case "Locatie:":
                                    tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Location)).ToList();
                                    break;
                                case "Luna:":
                                    tempInterventions = tempInterventions.Where(item => selectedItems.Contains(LocalCache.Instance.MonthsNames[item.Date.Month - 1])).ToList();
                                    break;
                                case "An:":
                                    tempInterventions =
                                        tempInterventions.Where(item => selectedItems.Contains(item.Date.Year.ToString()))
                                            .ToList();
                                    break;
                            }
                    }
                    if (localFilterGroup.IsIntervalActive)
                    {
                        tempInterventions =
                            tempInterventions.Where(
                                item => item.Date.Date <= localFilterGroup.EndDate.Date && item.Date.Date >= localFilterGroup.StartDate.Date)
                                .ToList();
                    }
                }
            }
            // SeenInterventions = new ObservableCollection<InterventionDetails>(tempInterventions);
            LocalCache.Instance.FilteredInterventionDetails.AddRange(tempInterventions);
        }

        private List<string> GetSelectedItemsNames(List<LocalFilterItem> filterItems)
        {
            return filterItems.Select(item => item.Name).ToList();
        }

        public void OnPaymentCellValueChanged(bool payedByDental, int interventionId)
        {
            DatabaseHandler.Instance.SetInterventionDentalPaymentStatus(interventionId, payedByDental);
            //            Parent.Parent.Parent.SetPaymentInterval(payedByDental);
            //            LocalCache.Instance.PaymentInterval.InitData();
            // Parent.Parent.ChangePaymentStatus(payedByDental);
        }

        public void ChangePaymentStatus(bool payed)
        {
            var interventionIds = new List<int>();
            foreach (var interventionDetailse in SelectedInterv)
            {
                var intervention = SeenInterventions.FirstOrDefault(item => item.Id == interventionDetailse.Id);
                intervention.WasPayedByDental = payed;
                interventionIds.Add(intervention.Id);
            }
            DatabaseHandler.Instance.SetInterventionsAsPayed(interventionIds, payed);
        }

        public void EditIntervention()
        {
            Parent.Parent.Parent.AddInterventionTileViewModel.SetInterventionToEdit();
        }

        private void OnDelete(InterventionDetails intervention)
        {
            DialogResult dialogResult = MessageBox.Show("Doriți sî ștergeți manopera?", "Atentie", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    NumarInterventii--;
                    TotalIncasari -= intervention.LocalIntervention.Revenue;
                    Parent.TotalIncasari -= intervention.LocalIntervention.Revenue;
                    Parent.NumarInterventii -= 1;
                    Parent.TotalProfit -= intervention.LocalIntervention.Percent;
                    TotalProcent -= intervention.LocalIntervention.Percent;
                    DatabaseHandler.Instance.RemoveIntervention(intervention.LocalIntervention.Id);
                    eventAggregator.GetEvent<TotalsModifiedEvent>().Publish(new TotalsIfo()
                    {
                        TotalHours = -intervention.LocalIntervention.DateHourDetail.Duration,
                        TotalInverventions = -1,
                        TotalRevenue = -intervention.LocalIntervention.Revenue,
                        TotalProfit = -intervention.LocalIntervention.Percent
                    });
                    SeenInterventions.Remove(intervention);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A intervenit o eroare la ștergere.");

                }
            }
        }

        #endregion Methods
    }
}
