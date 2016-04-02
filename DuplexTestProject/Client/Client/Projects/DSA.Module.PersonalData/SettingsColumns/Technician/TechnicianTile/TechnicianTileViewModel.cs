using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTileList;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTypeTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTileList;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTile
{
    public class TechnicianTileViewModel:BindableObjectListBase<TechnicianTileViewModel>
    {
        #region Constructor

        public TechnicianTileViewModel(TechnicianTileListViewModel parent)
        {
            Parent = parent;
            DeleteBtn = new SymbolIconButtonViewModel(new DelegateCommand(OnDelete), ViewConstants.DeleteSymbol);
            EditButton = new SymbolIconButtonViewModel(new DelegateCommand(OnEdit), ViewConstants.EditSymbol);
        }

        #endregion Constructor

        #region Properties

        public LocalTechnician Technician;

        public TechnicianTileListViewModel Parent { get; set; }

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

        private SymbolIconButtonViewModel editButton;
        public SymbolIconButtonViewModel EditButton
        {
            get { return editButton; }
            set
            {
                if (value == editButton)
                    return;
                editButton = value;
                OnPropertyChanged();
            }
        }

        private Brush contentBackgrund=BackgroundColors.TechnicianTile;

        public Brush ContentBackgrund
        {
            get { return contentBackgrund; }
            set
            {
                if (value == contentBackgrund)
                    return;
                contentBackgrund = value;
                OnPropertyChanged();
            }
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value == id)
                    return;
                id = value;
                OnPropertyChanged();
            }
        }

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

        private bool isEdit;

        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                if (value == isEdit)
                    return;
                isEdit = value;
                if (isEdit)
                {
                    EditTehnicianVisibility=Visibility.Visible;
                    NormalTileVisibility=Visibility.Collapsed;
                }
                else
                {
                    EditTehnicianVisibility = Visibility.Collapsed;
                    NormalTileVisibility = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }

        private Visibility editTehnicianVisibility=Visibility.Collapsed;

        public Visibility EditTehnicianVisibility
        {
            get { return editTehnicianVisibility; }
            set
            {
                if (value == editTehnicianVisibility)
                    return;
                editTehnicianVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility normalTileVisibility;

        public Visibility NormalTileVisibility
        {
            get { return normalTileVisibility; }
            set
            {
                if (value == normalTileVisibility)
                    return;
                normalTileVisibility = value;
                OnPropertyChanged();
            }
        }

        private MaterialTileListViewModel materialTileListViewModel;

        public MaterialTileListViewModel MaterialTileListViewModel
        {
            get { return materialTileListViewModel; }
            set
            {
                if (value == materialTileListViewModel)
                    return;
                materialTileListViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override void OnClick(bool shouldSelect)
        {
            if (shouldSelect)
            {
                if(MaterialTileListViewModel==null)
                    MaterialTileListViewModel=new MaterialTileListViewModel(this);
                Parent.MaterialTileListViewModel = MaterialTileListViewModel;
                ContentBackgrund = BackgroundColors.TechnicianTileMouseOn;
            }
            else
            {
                ContentBackgrund = BackgroundColors.TechnicianTile;
            }
            IsSelected = shouldSelect;
        }

        public void MouseOn()
        {
            if (!IsSelected)
            {
                ContentBackgrund = BackgroundColors.TechnicianTileMouseOn;
            }
        }

        public void MouseOff()
        {
            if (!IsSelected)
            {
                ContentBackgrund = BackgroundColors.TechnicianTile;
            }
        }

        private void OnEdit()
        {
            IsEdit = true;
            Parent.SaveButtonVisibility=Visibility.Visible;
            Parent.AddButtonVisibility=Visibility.Collapsed;
            Parent.MaterialTileListViewModel.WasChanged = false;
        }

        private void OnDelete()
        {
            
        }

        public async void OnSave()
        {
            if (Id==0)
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    var addedTechnician = new LocalTechnician()
                    {
                        Id = 0,
                        Name = Name
                    };
                    addedTechnician.Id = await DatabaseHandler.Instance.AddTechnician(addedTechnician);
                    LocalCache.Instance.Technicians.Add(addedTechnician);
                    Id = addedTechnician.Id;
                }
                else
                {
                    MessageBox.Show("Adaugati Nume pentru a putea salva!");
                }
            }
            else
            {
                if (IsEdit)
                {
                    var technicianToEdit = LocalCache.Instance.Technicians.FirstOrDefault(item => item.Id == Id);
                    technicianToEdit.Name = Name;
                    DatabaseHandler.Instance.AddTechnician(technicianToEdit);
                }
            }
            IsEdit = false;
            Parent.MaterialTileListViewModel.WasChanged = false;
        }
        

        #endregion Methods
    }
}
