using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTileList;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTile.AddTechnicianTile
{
    public class AddTechnicianTileViewModel:ViewModelBase
    {
        #region Constructor

        public AddTechnicianTileViewModel(TechnicianTileListViewModel parent)
        {
            Parent = parent;
            SaveTechnicianButton=new ActionButtonViewModel("Salvează  ",new DelegateCommand(OnSave),ImagePath.SaveIconPath);
        }

        #endregion Constructor

        #region Properties

        public TechnicianTileListViewModel Parent { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;
                name = value;
                OnPropertyChanged();
            }

        }

        private ActionButtonViewModel saveTechnicianButton;

        public ActionButtonViewModel SaveTechnicianButton
        {
            get { return saveTechnicianButton; }
            set
            {
                if (value == saveTechnicianButton)
                    return;
                saveTechnicianButton = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties


        #region Methods

        private async void OnSave()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var addedTechnician = new LocalTechnician()
                {
                    Name = Name
                };
                addedTechnician.Id = await DatabaseHandler.Instance.AddTechnician(addedTechnician);
                LocalCache.Instance.Technicians.Add(addedTechnician);
                Parent.TechniciansObjectList.Add(new TechnicianTileViewModel(Parent)
                {
                    Id = addedTechnician.Id,
                    Name = Name
                });
                Parent.AddTileVisibility = Visibility.Collapsed;
                Parent.AddButtonVisibility = Visibility.Visible;   
            }
            else
            {
                MessageBox.Show("Adaugati Nume pentru a putea salva!");
            }
        }

        #endregion Methods
    }
}
