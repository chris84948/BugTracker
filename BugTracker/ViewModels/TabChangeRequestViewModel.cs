using System;
using System.DirectoryServices.AccountManagement;
using BugTracker.MVVM;
using BugTracker.Model;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;
using BugTracker.DataAccess;
using MahApps.Metro.Controls.Dialogs;
using BugTracker.Common;
using System.Threading.Tasks;

namespace BugTracker.ViewModels
{
    class TabChangeRequestViewModel : IssueViewModel, IDataErrorInfo
    {
        private IDataAccess dataAccess;
        private ChangeRequest ChangeRequest;

        public ICommand CloseTabCommand { get { return new RelayCommand(CloseTab, () => true); } }
        public ICommand SaveTabCommand { get { return new RelayCommand(SaveTab, () => true); } }

        public override int ID
        {
            get { return ChangeRequest.ID; }
            set
            {
                ChangeRequest.ID = value;
                IssueID = value;
                OnPropertyChanged(() => ID);
            }
        }

        public string VersionImplemented
        {
            get { return ChangeRequest.VersionImplemented; }
            set
            {
                ChangeRequest.VersionImplemented = value;
                OnPropertyChanged(() => VersionImplemented);
                IsDirty = true;
            }
        }

        public string DetailedDescription
        {
            get { return ChangeRequest.DetailedDescription; }
            set
            {
                ChangeRequest.DetailedDescription = value;
                OnPropertyChanged(() => DetailedDescription);
                IsDirty = true;
            }
        }

        public string Justification
        {
            get { return ChangeRequest.Justification; }
            set
            {
                ChangeRequest.Justification = value;
                OnPropertyChanged(() => Justification);
                IsDirty = true;
            }
        }

        public TabChangeRequestViewModel(Messenger messenger,
                                         DialogCoordinator dialogCoordinator,
                                         IDataAccess dataAccess)
            : this(messenger, dialogCoordinator, dataAccess,
            new ChangeRequest()
            {
                ID = 0,
                VersionImplemented = "",
                DetailedDescription = "",
                Justification = ""
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
            TabHeader = "New Change";
        }

        public TabChangeRequestViewModel(Messenger messenger,
                                         DialogCoordinator dialogCoordinator,
                                         IDataAccess dataAccess,
                                         ChangeRequest changeRequest,
                                         Issue issue)
            : base(issue)
        {
            this.messenger = messenger;
            this.dialogCoordinator = dialogCoordinator;
            this.dataAccess = dataAccess;
            this.ChangeRequest = changeRequest;
            this.ID = changeRequest.ID;

            ShowCloseButton = true;
            TabHeader = "Change #" + IssueID.ToString();
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
            this.ID = dataAccess.SaveChangeRequest(Issue, ChangeRequest);
            TabHeader = "Change #" + ID.ToString();
            base.Save();
        }
    }
}
