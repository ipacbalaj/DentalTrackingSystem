using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Financial.Entities;
using DSA.Financial.GenerateTablereqInfo;
using DSA.Financial.WordDoc;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DSA.Financial.FinancialScreen
{
    public class FinancialScreenViewModel:ViewModelBase
    {
        #region Constructor
        public FinancialScreenViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            eventAggregator.GetEvent<UpdateFinancialEvent>().Subscribe(UpdateFinancialScreen);
            GenerateButton = new ActionButtonViewModel("Generare Tabel", new DelegateCommand(OnGenerate), ImagePath.SaveIconPath);            
            GenerateWordDocButton = new ActionButtonViewModel("Genereaza Word", new DelegateCommand(OnGenerateWordDoc), ImagePath.GenerateWord);
        }

        #endregion Constructor          

        #region Properties

        private bool initialized;

        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer unityContainer;

        private ActionButtonViewModel generateButton;
        public ActionButtonViewModel GenerateButton
        {
            get { return generateButton; }
            set
            {
                if (value == generateButton)
                    return;
                generateButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel generateWordDocButton;

        public ActionButtonViewModel GenerateWordDocButton
        {
            get { return generateWordDocButton; }
            set
            {
                if (value == generateWordDocButton)
                    return;
                generateWordDocButton = value;
                OnPropertyChanged();
            }
        }


        private SumGenerator.OwnSumGenerator sumGenerator;

        public SumGenerator.OwnSumGenerator SumGenerator
        {
            get { return sumGenerator; }
            set
            {
                if (value == sumGenerator)
                    return;
                sumGenerator = value;
                OnPropertyChanged();
            }
        }

        private Brush contentBackground = BackgroundColors.FinancialColor;
        public Brush ContentBackground
        {
            get { return contentBackground; }
            set
            {
                if (value == contentBackground)
                    return;
                contentBackground = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FinancialInfo> financialEntities;

        public ObservableCollection<FinancialInfo> FinancialEntities
        {
            get { return financialEntities; }
            set
            {
                if (value == financialEntities)
                    return;
                financialEntities = value;
                OnPropertyChanged();
            }
        }

        private GenerateTableInfoViewModel generateTableInfoViewModel;

        public GenerateTableInfoViewModel GenerateTableInfoViewModel
        {
            get { return generateTableInfoViewModel; }
            set
            {
                if (value == generateTableInfoViewModel)
                    return;
                generateTableInfoViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void UpdateFinancialScreen(Object anObj)
        {
            SumGenerator = new SumGenerator.OwnSumGenerator(LocalCache.Instance.Works.Where(item => item.IncludedINFinancial).ToList());
            if (!initialized)
            {                
                GenerateTableInfoViewModel = new GenerateTableInfoViewModel();
                initialized = true;
            }
            
        }

        private void OnGenerate()
        {
            var finaincialInfo = SumGenerator.GenerateList(GenerateTableInfoViewModel.TotalSum, GenerateTableInfoViewModel.Percent);
            SetFinancialEntities(finaincialInfo);
        }

        private void SetFinancialEntities(List<FinancialInfo> financialEntities)
        {
            FinancialEntities = new ObservableCollection<FinancialInfo>(financialEntities.Where(item=>item.NumberOfProvServ>0));
        }

        private void SetWorks( )
        {
            
        }

        private void OnGenerateWordDoc()
        {
            try
            {
                WordDocGenerator wordDocGenerator = new WordDocGenerator();
                var finaincialList = FinancialEntities.ToList();
                var totalInfo = new FinancialInfoTotal();
                totalInfo.TotalText = "Total";
                foreach (var financialInfo in finaincialList)
                {
                    totalInfo.TotalNBOfProvServ += financialInfo.NumberOfProvServ;
                    totalInfo.DentalPrice += financialInfo.DentalPrice;
                    totalInfo.PriceDueToContract += financialInfo.PriceDueToContract;
                    totalInfo.TotalPriceProvServ += financialInfo.TotalPriceProvServ;
                    totalInfo.TotalDueToContract += financialInfo.TotalDueToContract;

                }
                wordDocGenerator.CreateTable(finaincialList, totalInfo, GenerateTableInfoViewModel.Year, GenerateTableInfoViewModel.SelectedMonth, GenerateTableInfoViewModel.Provider, GenerateTableInfoViewModel.Beneficiary);
            }
            catch (Exception)
            {
                MessageBox.Show("Pentru a genera fisierul word creati tabelul intai");
            }
          
        }


        #endregion Methods 

    }
}
