﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.SettingsTileList;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.SettingsTile.EditSettingsTile
{
    public class EditSettingsTileViewModel:ViewModelBase
    {
        #region Constructor

        public EditSettingsTileViewModel(SettingsTileListViewModel parent,LocalCache.SettingsItemEnum settingsItemEnum)
        {
            Parent = parent;
            SaveBtn=new ActionButtonViewModel("Salvează  ",new DelegateCommand(OnSave),ImagePath.SaveIconPath);
            InitEditView(settingsItemEnum);
        }

        #endregion Constructor          

        #region Properties

        private Visibility typeVisibility=Visibility.Collapsed;

        public Visibility TypeVisibility
        {
            get { return typeVisibility; }
            set
            {
                if (value == typeVisibility)
                    return;
                typeVisibility = value;
                OnPropertyChanged();
            }
        }

        public SettingsTileListViewModel Parent { get; set; }

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

        private double cost;

        public double Cost
        {
            get { return cost; }
            set
            {
                if (value == cost)
                    return;
                cost = value;
                OnPropertyChanged();
            }
        }

        private double percent;

        public double Percent
        {
            get { return percent; }
            set
            {
                if (value == percent)
                    return;
                percent = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel saveBtn;
        public ActionButtonViewModel SaveBtn
        {
            get { return saveBtn; }
            set
            {
                if (value == saveBtn)
                    return;
                saveBtn = value;
                OnPropertyChanged();
            }
        }

        private string attributeRequired;
        public string AttributeRequired
        {
            get { return attributeRequired; }
            set
            {
                if (value == attributeRequired)
                    return;
                attributeRequired = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void SetData(string name, double cost,int id)
        {
            Id = id;
            Name = name;
            Cost = cost;      
            
        }

        private void OnSave()
        {
          //  Parent.OnSave(Id,Name,Cost,Percent,false);
        }

        private void InitEditView(LocalCache.SettingsItemEnum settingsItemEnum)
        {
            switch (settingsItemEnum)
            {
                    case LocalCache.SettingsItemEnum.Material:
                    AttributeRequired = "Cost:";
                    TypeVisibility=Visibility.Visible;
                    break;
                    case LocalCache.SettingsItemEnum.Manopera:
                    AttributeRequired = "Procent:";
                    TypeVisibility=Visibility.Visible;
                    break;
            }
        }

        #endregion Methods 

    }
}
