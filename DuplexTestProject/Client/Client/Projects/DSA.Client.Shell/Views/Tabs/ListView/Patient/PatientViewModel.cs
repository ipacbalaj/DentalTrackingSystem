using ClientShell.Helpers;
using DevExpress.Data.Utils.ServiceModel.Native;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure.ViewModel;
using Microsoft.Practices.Prism.Commands;

namespace ClientShell.Views.Tabs.ListView.Patient
{
    public class PatientViewModel:ViewModelBase
    {
        #region Constructor
        public PatientViewModel(LeftListViewModel parent)
        {
            Parent = parent;
            SavePatientButton=new ActionButtonViewModel("Save",new DelegateCommand(OnSave));
        }

        #endregion Constructor

        #region Properties

        public LeftListViewModel Parent { get; set; }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (surname != value)
                {
                    surname = value;
                    OnPropertyChanged();
                }
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged();
                }
            }
        }

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged();
                }
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }

        private ActionButtonViewModel savePatientButton;
        public ActionButtonViewModel SavePatientButton
        {
            get { return savePatientButton; }
            set
            {
                if (savePatientButton == value)
                    return;
                savePatientButton = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnSave()
        {
            Parent.SavePatient(this.ToLocalPatient());            
        }
        #endregion Methods

    }
}
