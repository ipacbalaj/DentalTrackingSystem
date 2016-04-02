using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DSA.Common.Controls.Buttons.ListItemButton;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.ViewModel;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Common.Controls.Pagination
{
    public class PaginationViewModel:ViewModelBase
    {
        #region Constructor
        public PaginationViewModel(int nbOfQuestions)
        {
            NumberOfQuestions = nbOfQuestions;
            NextCommand=new DelegateCommand(OnNext);   
            PrevCommand=new DelegateCommand(OnPrev); 
            PopulatePageItemList();            
        }

        #endregion Constructor

        #region Properties

        public OnChangePageDelegate OnChangePageDel { get; set; }

        private ObjectList<ListItemButtonViewModel> pageItemsList = new ObjectList<ListItemButtonViewModel>(false);
        public ObjectList<ListItemButtonViewModel> PageItemsList
        {
            get { return pageItemsList; }
            set
            {
                if (pageItemsList == value)
                    return;
                pageItemsList = value;
                OnPropertyChanged();
            }
        }

        public int questionsPerPage = 1;
        public int QuestionsPerPage
        {
            get { return questionsPerPage; }
            set
            {
                if (questionsPerPage != value)
                {
                    questionsPerPage = value;
                    PopulatePageItemList();
                    DecideQPerPage();                    
                    OnPropertyChanged();
                }
            }
        }

        public int numberOfPages;
        public int NumberOfPages
        {
            get { return numberOfPages; }
            set
            {
                if (numberOfPages != value)
                {
                    if(numberOfPages<value) OnChangeNumberPages(true);
                    else if(numberOfPages>value) OnChangeNumberPages(false);
                    numberOfPages = value;
                    OnPropertyChanged();
                }
            }
        }

        public int questionIndex = 0;
        public int QuestionIndex
        {
            get { return questionIndex; }
            set
            {
                if (questionIndex != value)
                {
                    questionIndex = value;
                    if (questionIndex <= 0)
                    {
                        QuestionIndex = 0;
                        PrevEnabled = false;
                        NextEnabled = true;
                    }
                    else
                    {
                        PrevEnabled = true;
                        NextEnabled = questionIndex < NumberOfQuestions-QuestionsPerPage;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public int numberOfQuestions;
        public int NumberOfQuestions
        {
            get { return numberOfQuestions; }
            set
            {
                if (numberOfQuestions != value)
                {
                    numberOfQuestions = value;
                    NumberOfPages = (int)Math.Ceiling(((double)NumberOfQuestions) / QuestionsPerPage);
                   // OnSpecificPageCommand(NumberOfPages.ToString());
                    NextEnabled = NumberOfQuestions - QuestionIndex - QuestionsPerPage > 0;
                    OnPropertyChanged();
                }
            }
        }

        public bool nextEnabled;
        public bool NextEnabled
        {
            get { return nextEnabled; }
            set
            {
                if (nextEnabled != value)
                {
                    nextEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool prevEnabled;
        public bool PrevEnabled
        {
            get { return prevEnabled; }
            set
            {
                if (prevEnabled != value)
                {
                    prevEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> qPerPageList = new ObservableCollection<string>() { "1","5", "10", "15", "20", "25", "50", "100"};
        public ObservableCollection<string> QPerPageList
        {
            get { return qPerPageList; }
            set
            {
                if (qPerPageList != value)
                {
                    qPerPageList = value;
                    OnPropertyChanged();
                }
            }
        }
        public delegate void OnChangePageDelegate(int questionIndex,int qPerPage);

        public ICommand NextCommand { get; set; }
        public ICommand PrevCommand { get; set; }

        #endregion Properties

        #region Methods

        private void OnNext()
        {
            QuestionIndex += QuestionsPerPage;
            int pageIndex = (QuestionIndex / QuestionsPerPage)+1;
            var pageToSelect = PageItemsList.List.FirstOrDefault(item => item.Id == pageIndex);
            pageToSelect.OnSelected();
        }        

        private void OnPrev()
        {
            QuestionIndex -= QuestionsPerPage;
            int pageIndex = (QuestionIndex / QuestionsPerPage)+1;
            var pageToSelect = PageItemsList.List.FirstOrDefault(item => item.Id == pageIndex);
            pageToSelect.OnSelected();
        }

        private void PopulatePageItemList()
        {
            PageItemsList=new ObjectList<ListItemButtonViewModel>(false);
            int nbOfPages= (int)Math.Ceiling(((double)NumberOfQuestions) / QuestionsPerPage);
            for (int i = 1; i <= nbOfPages; i++)
            {
                PageItemsList.Add(new ListItemButtonViewModel(i,""+i,new DelegateCommand<string>(OnSpecificPageCommand)));
            }
            var firstElement = PageItemsList.List.FirstOrDefault();
            if (firstElement != null)
                firstElement.OnSelected(firstElement);
        }

        public void OnSpecificPageCommand(string page="1")
        {
            QuestionIndex = (Convert.ToInt32(page) - 1)*QuestionsPerPage;
            OnChangePageDel(QuestionIndex, QuestionsPerPage);
        }

        private void OnChangeNumberPages(bool numberIsGreater)
        {
            var lastItem = PageItemsList.List.LastOrDefault();
            if(lastItem!=null)
            if (numberIsGreater)
            {                
                    PageItemsList.Add(new ListItemButtonViewModel(lastItem.Id + 1, "" + (lastItem.Id + 1), new DelegateCommand<string>(OnSpecificPageCommand)));
                    PageItemsList.List.LastOrDefault().OnSelected();
                    NextEnabled = false;
            }
            else
            {
                PageItemsList.List.Remove(lastItem);
                //OnSpecificPageCommand(NumberOfPages.ToString());
                var newLast = PageItemsList.List.LastOrDefault();
                if (newLast != null) newLast.OnSelected();

            }
        }

        private void DecideQPerPage()
        {
            QuestionIndex = 0;
            if (QuestionsPerPage >= NumberOfQuestions)
            {
                NextEnabled = false;
                PrevEnabled = false;
                OnSpecificPageCommand();
            }
            else
            {
                OnSpecificPageCommand();
                NextEnabled = true;
            }
        }

        public void OnNumberOfItemsChanged(int value)
        {
            NumberOfQuestions += value;
            NumberOfPages = (int)Math.Ceiling(((double)NumberOfQuestions) / QuestionsPerPage);
            OnChangePageDel(QuestionIndex,QuestionsPerPage);
        }

        #endregion Methods
    }
}
