﻿using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.Helpers;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using Microsoft.Practices.Prism.Commands;
using MessageBox = System.Windows.MessageBox;

namespace DSA.Common.Controls.LoginControls.ChangePassword
{
    public class ChangePasswordViewModel : ViewModelBase
    {        
        #region Constructor

        public ChangePasswordViewModel()
        {
            ChangePassCommand = new DelegateCommand(OnChangeCommand);
            ConfirmCommand = new DelegateCommand(OnConfirmCommand);
            CancelCommand=new DelegateCommand(OnCloseCommand);
            ChoseFileCommand = new DelegateCommand(OnSetPath);
            ChangePassButton = new SymbolIconButtonViewModel(ChangePassCommand, "Schimbă parolă");
            ConfirmButton = new SymbolIconButtonViewModel(ConfirmCommand,"Salvează  ");
            SelectPathButton = new SymbolIconButtonViewModel(ChoseFileCommand, "Selectează Locație");
            //CancelButton = new SymbolIconButtonViewModel(CancelCommand,"Anuleaza");
            Inititialize();
        }

        #endregion Constructor

        #region Properties

        public Action CloseAction { get; set; }
        public ICommand ChoseFileCommand { get; set; }
//        public AccountListViewModel Paremt { get; set; }

        private SymbolIconButtonViewModel changePassButton;
        public SymbolIconButtonViewModel ChangePassButton
        {
            get { return changePassButton; }
            set
            {
                if (changePassButton != value)
                {
                    changePassButton = value;
                    OnPropertyChanged();
                }
            }
        }

        private SymbolIconButtonViewModel selectPathButton;

        public SymbolIconButtonViewModel SelectPathButton
        {
            get { return selectPathButton; }
            set
            {
                if (value == selectPathButton)
                    return;
                selectPathButton = value;
                OnPropertyChanged();
            }
        }

        private int borderLineVisibility = 0;
        public int BorderLineVisibility
        {
            get { return borderLineVisibility; }
            set
            {
                if (borderLineVisibility != value)
                {
                    borderLineVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private string uploadFilePath="";

        public string UploadFilePath
        {
            get { return uploadFilePath; }
            set
            {
                if (value == uploadFilePath)
                    return;
                uploadFilePath = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel confirmButton;
        public SymbolIconButtonViewModel ConfirmButton
        {
            get { return confirmButton; }
            set
            {
                if (confirmButton != value)
                {
                    confirmButton = value;
                    OnPropertyChanged();
                }
            }
        }

        private SymbolIconButtonViewModel cancelButton;
        public SymbolIconButtonViewModel CancelButton
        {
            get { return cancelButton; }
            set
            {
                if (cancelButton != value)
                {
                    cancelButton = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility cancelButtonVisibility = Visibility.Collapsed;
        public Visibility CancelButtonVisibility
        {
            get { return cancelButtonVisibility; }
            set
            {
                if (cancelButtonVisibility != value)
                {
                    cancelButtonVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility changePassButtonVisibility = Visibility.Visible;
        public Visibility ChangePassButtonVisibility
        {
            get { return changePassButtonVisibility; }
            set
            {
                if (changePassButtonVisibility != value)
                {
                    changePassButtonVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility confirmButtonVisibility = Visibility.Collapsed;
        public Visibility ConfirmButtonVisibility
        {
            get { return confirmButtonVisibility; }
            set
            {
                if (confirmButtonVisibility != value)
                {
                    confirmButtonVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility passwordFieldsVisibility = Visibility.Collapsed;
        public Visibility PasswordFieldsVisibility
        {
            get { return passwordFieldsVisibility; }
            set
            {
                if (passwordFieldsVisibility != value)
                {
                    passwordFieldsVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

         private Visibility changePassVisibility;
        public Visibility ChangePassVisibility
        {
            get { return changePassVisibility; }
            set
            {
                if (changePassVisibility != value)
                {
                    changePassVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private string retypedPassword;
        public string RetypedPassword
        {
            get { return retypedPassword; }
            set
            {
                if (retypedPassword != value)
                {
                    retypedPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
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

        private string server;
        public string Server
        {
            get { return server; }
            set
            {
                if (server == value)
                    return;
                server = value;
                OnPropertyChanged();
            }
        }

        private int port;
        public int Port
        {
            get { return port; }
            set
            {
                if (port == value)
                    return;
                port = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        public ICommand ChangePassCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        #region Methods

        private void Inititialize()
        {
//            Server = Parent.MailSender.MailData.Server;
//            Email = Parent.MailSender.MailData.SenderEmail.EmailAddress;
//            Port = Parent.MailSender.MailData.Port;
//            Password = Parent.MailSender.MailData.SenderEmail.Password;
//            RetypedPassword = Parent.MailSender.MailData.SenderEmail.Password;
        }


        /// <summary>
        /// Called when [change command].
        /// Show the fields for typing new password and confirm button
        /// </summary>
        private void OnChangeCommand()
        {            
            ConfirmButtonVisibility = Visibility.Visible;
            CancelButtonVisibility = Visibility.Visible;
            BorderLineVisibility = 1;
        }

        /// <summary>
        /// Called when [confirm command].
        /// verify if password and retype password are equal 
        /// if true save the new password
        /// otherwise warning message
        /// </summary>
        private void OnConfirmCommand()
        {
            if (ConfirmNewPass(Password, RetypedPassword))
            {
                ConfirmButtonVisibility = Visibility.Collapsed;
                PasswordFieldsVisibility = Visibility.Collapsed;
                CancelButtonVisibility=Visibility.Collapsed;
                BorderLineVisibility = 0;

                LocalCache.Instance.CurrentUser.Username = Email;
                XmlSerializerHelper.SaveToXml(ViewConstants.appDataPath, LocalCache.Instance.CurrentUser);
                LocalCache.Instance.CurrentUser.Password = Password;
                DatabaseHandler.Instance.EditUser(LocalCache.Instance.CurrentUser);
                {
                    DatabaseHandler.Instance.AddEditProgramInfo(new LocalProgramInfo()
                    {
                        ExportFilePath = UploadFilePath,
                        LastImportDateFile = new DateTime(2000, 1, 1)
                    });
                }
                
            }
            else
            {
                MessageBox.Show("Rescrierea parolei este gresita!");
                Password = "";
                RetypedPassword = "";
            }

        }

        /// <summary>
        /// When selecting another user collapse the password and confirm button
        /// </summary>
        public void Reset()
        {
            ChangePassButtonVisibility = Visibility.Visible;
            ConfirmButtonVisibility = Visibility.Collapsed;
            PasswordFieldsVisibility = Visibility.Collapsed;
            ChangePassButtonVisibility = Visibility.Visible;
            CancelButtonVisibility=Visibility.Collapsed;
            BorderLineVisibility = 0;
            Password = "";
            RetypedPassword = "";
        }

        /// <summary>
        /// return true if params are equal 
        /// otherwise false
        /// </summary>
        /// <param name="pass">The password.</param>
        /// <param name="retypedPass">The retyped password.</param>
        /// <returns></returns>
        private bool ConfirmNewPass(string pass, string retypedPass)
        {
            return pass == retypedPass;
        }

        private void OnCloseCommand()
        {
            BorderLineVisibility = 0;
        }

        private void OnSetPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                UploadFilePath = fbd.SelectedPath;
            }
        }

        #endregion Methods
    }
}

