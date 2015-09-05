using BugTracker.Common;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using BugTracker.MVVM;
using System.Windows.Input;
using System.Windows;
using BugTracker.DataAccess;
using BugTracker.Model;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro;
using BugTracker.Properties;
using System.Diagnostics;

namespace BugTracker.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private Messenger messenger;
        private IDataAccess dataAccess;
        private DialogCoordinator dialogCoordinator;

        public ICommand SaveAllTabsCommand { get { return new RelayCommand(SaveAllTabs, () => true); } }
        public ICommand AddBugCommand { get { return new RelayCommand(AddBug, () => true); } }
        public ICommand AddChangeRequestCommand { get { return new RelayCommand(AddChangeRequest, () => true); } }

        private ObservableCollection<ScreenBase> _tabs;
        public ObservableCollection<ScreenBase> Tabs
        {
            get { return _tabs; }
            set
            {
                _tabs = value;
                OnPropertyChanged(() => Tabs);
            }
        }

        private ScreenBase _selectedTab;
        public ScreenBase SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged(() => SelectedTab);

                if (value != null)
                {
                    if (value.GetType() == typeof(TabAllIssuesViewModel))
                        ((TabAllIssuesViewModel)value).RefreshTab();
                }
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(() => SelectedIndex);

                AnyTabsDirty = DoAnyTabsHaveUnsavedChanges();
            }
        }

        private bool _anyTabsDirty;
        public bool AnyTabsDirty
        {
            get { return _anyTabsDirty; }
            set
            {
                _anyTabsDirty = value;
                OnPropertyChanged(() => AnyTabsDirty);
            }
        }

        private bool _flyoutOpen;
        public bool FlyoutOpen
        {
            get { return _flyoutOpen; }
            set
            {
                _flyoutOpen = value;
                OnPropertyChanged(() => FlyoutOpen);
            }
        }

        public ShellViewModel()
        {
            messenger = new Messenger();
            messenger.closeTab = CloseTab;
            messenger.openIssue = OpenOrLocateIssueInTabs;
            messenger.tabSaveStatusChanged = CurrentTabIsDirty;

            dialogCoordinator = DialogCoordinator.Instance;

            dataAccess = new SQLiteController();

            Tabs = new ObservableCollection<ScreenBase>();
            Tabs.Add(new TabAllIssuesViewModel(messenger, dialogCoordinator, dataAccess));
                        
            SelectedTab = Tabs[0];
        }

        private void OpenOrLocateIssueInTabs(int issueID)
        {
            if (Tabs.Select((x) => x.ID).Contains(issueID))
                SwitchToExistingTab(issueID);

            else
                OpenNewTabWithIssue(issueID);    
        }

        private void SwitchToExistingTab(int issueID)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                if (Tabs[i].ID == issueID)
                {
                    SelectedIndex = i;
                    return;
                }
            }
        }

        private void OpenNewTabWithIssue(int issueID)
        {
            var issue = dataAccess.GetIssue(issueID);

            if (issue.IssueType == (int)eIssueType.Bug)
            {
                var bug = dataAccess.GetBug(issue.IssueID);
                Tabs.Add(new TabBugViewModel(messenger, dialogCoordinator, dataAccess, bug, issue.Issue));
            }
            else
            {
                var changeRequest = dataAccess.GetChangeRequest(issue.IssueID);
                Tabs.Add(new TabChangeRequestViewModel(messenger, dialogCoordinator, dataAccess, changeRequest, issue.Issue));
            }

            SelectedIndex = Tabs.Count - 1;
        }

        public void CloseTab(ScreenBase tabToClose)
        {
            Tabs.Remove(tabToClose);
        }

        private void CurrentTabIsDirty()
        {
            AnyTabsDirty = DoAnyTabsHaveUnsavedChanges();
        }
        
        private void SaveAllTabs()
        {
            foreach (var tab in Tabs)
                tab.Save();
        }

        private void AddBug()
        {
            Tabs.Add(new TabBugViewModel(messenger, dialogCoordinator, dataAccess));
            SelectedIndex = Tabs.Count - 1;
        }

        private void AddChangeRequest()
        {
            Tabs.Add(new TabChangeRequestViewModel(messenger, dialogCoordinator, dataAccess));
            SelectedIndex = Tabs.Count - 1;
        }

        private bool DoAnyTabsHaveUnsavedChanges()
        {
            foreach (var tab in Tabs)
            {
                if (tab.IsDirty)
                    return true;
            }

            return false;
        }
    }
}
