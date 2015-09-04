using BugTracker.Common;
using BugTracker.Model;
using BugTracker.MVVM;
using System;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Windows.Input;
using BugTracker.DataAccess;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows;
using System.Threading.Tasks;

namespace BugTracker.ViewModels
{
    class TabBugViewModel : IssueViewModel, IDataErrorInfo
    {
        private IDataAccess dataAccess;
        private Bug Bug;

        public ICommand CloseTabCommand { get { return new RelayCommand(CloseTab, () => true); } }
        public ICommand SaveTabCommand { get { return new RelayCommand(SaveTab, () => true); } }

        public override int ID
        {
            get { return Bug.ID; }
            set
            {
                Bug.ID = value;
                IssueID = value;
                OnPropertyChanged(() => ID);
            }
        }

        public string VersionFound
        {
            get { return Bug.VersionFound; }
            set
            {
                Bug.VersionFound = value;
                OnPropertyChanged(() => VersionFound);
                IsDirty = true;
            }
        }

        public string VersionFixed
        {
            get { return Bug.VersionFixed; }
            set
            {
                Bug.VersionFixed = value;
                OnPropertyChanged(() => VersionFixed);
                IsDirty = true;
            }
        }

        public string DetailedDescription
        {
            get { return Bug.DetailedDescription; }
            set
            {
                Bug.DetailedDescription = value;
                OnPropertyChanged(() => DetailedDescription);
                IsDirty = true;
            }
        }

        public string StepsToReproduce
        {
            get { return Bug.StepsToReproduce; }
            set
            {
                Bug.StepsToReproduce = value;
                OnPropertyChanged(() => StepsToReproduce);
                IsDirty = true;
            }
        }

        public string Workaround
        {
            get { return Bug.Workaround; }
            set
            {
                Bug.Workaround = value;
                OnPropertyChanged(() => Workaround);
                IsDirty = true;
            }
        }

        public string Fix
        {
            get { return Bug.Fix; }
            set
            {
                Bug.Fix = value;
                OnPropertyChanged(() => Fix);
                IsDirty = true;
            }
        }

        public TabBugViewModel(Messenger messenger,
                               DialogCoordinator dialogCoordinator,
                               IDataAccess dataAccess)
            : this(messenger, dialogCoordinator, dataAccess,
            new Bug()
            {
                ID = 0,
                VersionFound = "",
                VersionFixed = "",
                DetailedDescription = "",
                StepsToReproduce = "",
                Workaround = "",
                Fix = ""
            }, new Issue()
            {
                IssueID = 0,
                Description = "",
                UserCreated = Environment.UserName,
                UserClosed = "",
                DateCreated = DateTime.Now,
                DateClosed = DateTime.Now,
                PlantContact = "",
                IssueType = (int)eIssueType.Bug,
                IssueState = (int)eIssueState.Open,
                IssueResolution = (int)eIssueResolution.NA,
                Priority = (int)ePriority.Low,
                Severity = (int)eSeverity.Low
            })
        {
            IsDirty = true;
            TabHeader = "New Bug";
        }

        public TabBugViewModel(Messenger messenger,
                               DialogCoordinator dialogCoordinator,
                               IDataAccess dataAccess,
                               Bug bug,
                               Issue issue)
            : base(issue)
        {
            this.messenger = messenger;
            this.dialogCoordinator = dialogCoordinator;
            this.dataAccess = dataAccess;
            this.Bug = bug;
            this.ID = bug.ID;

            ShowCloseButton = true;
            TabHeader = "Bug #" + IssueID.ToString();
        }

        private void RefreshAllFieldsToForceValidation()
        {
            foreach (PropertyInfo propInfo in typeof(TabBugViewModel).GetProperties())
            {
                if (propInfo.PropertyType == typeof(string) && propInfo.Name.ToUpper() != "ITEM")
                    OnPropertyChanged(propInfo.Name);
            }
        }

        public override async void CloseTab()
        {
            if (IsDirty)
                await CheckForSavingTab();
            else
                messenger.closeTab(this);
        }

        private async Task CheckForSavingTab()
        {
            var result = await Dialogs.GetUserConfirmation(dialogCoordinator, this);
            
            if (result == MessageDialogResult.Affirmative)
            {
                SaveTab();
                messenger.closeTab(this);
            }
            else if (result == MessageDialogResult.Negative)
            {
                messenger.closeTab(this);
            }
            else
            {
                // do nothing - just close the dialog
            }
        }

        private void SaveTab()
        {
            saving = true;

            RefreshAllFieldsToForceValidation();
            
            if (validator.IsUserDataValid())
                Save();

            saving = false;
        }

        public override void Save()
        {
            this.ID = dataAccess.SaveBug(Issue, Bug);
            TabHeader = "Bug #" + ID.ToString();
            base.Save();
        }
    }
}