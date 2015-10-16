using System;
using BugTracker.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BugTracker.Converters;
using System.Text;
using System.Reflection;
using BugTracker.MVVM;
using System.Windows.Input;
using BugTracker.DataAccess;
using MahApps.Metro.Controls.Dialogs;

namespace BugTracker.ViewModels
{
    class TabAllIssuesViewModel : ScreenBase
    {
        private IDataAccess dataAccess;
        private System.Timers.Timer filterTimer;

        private List<IssueViewModel> _allIssues;
        public List<IssueViewModel> AllIssues
        {
            get { return _allIssues; }
            set
            {
                _allIssues = value;
                OnPropertyChanged(() => AllIssues);
            }
        }

        private List<TitledCommand> _dropdownCommands;
        public List<TitledCommand> DropdownCommands
        {
            get { return _dropdownCommands; }
            set
            {
                _dropdownCommands = value;
                OnPropertyChanged(() => DropdownCommands);
            }
        }


        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged(() => Filter);
                ResetTimer();

                ShowNoItemsIndicator = string.IsNullOrEmpty(_filter) & AllIssues.Count == 0;
                Console.WriteLine("show" + ShowNoItemsIndicator.ToString());

            }
        }

        private bool _ShowNoItemsIndicator;
        public bool ShowNoItemsIndicator
        {
            get { return _ShowNoItemsIndicator; }
            set
            {
                _ShowNoItemsIndicator = value;
                OnPropertyChanged(() => ShowNoItemsIndicator);
            }
        }

        public TabAllIssuesViewModel(Messenger messenger, DialogCoordinator dialogCoordinator, IDataAccess dataAccess)
        {
            this.messenger = messenger;
            this.dialogCoordinator = dialogCoordinator;
            this.dataAccess = dataAccess;

            ShowCloseButton = false;
            TabHeader = "All Issues";
            AllIssues = new List<IssueViewModel>();
            DropdownCommands = CreateListOfDropdownCommands();

            filterTimer = new System.Timers.Timer(1000);
            filterTimer.AutoReset = false;
            filterTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => RefreshTab();

            AllIssues = dataAccess.GetAllIssues(String.Empty);
        }

        private List<TitledCommand> CreateListOfDropdownCommands()
        {
            DropdownCommands = new List<TitledCommand>();

            DropdownCommands.Add(new TitledCommand() { Title = "ID Number", Command = new RelayCommand(AddIDFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Description", Command = new RelayCommand(AddDescriptionFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Created By", Command = new RelayCommand(AddUserCreatedFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Closed By", Command = new RelayCommand(AddUserClosedFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Contact", Command = new RelayCommand(AddContactFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Issue Type", Command = new RelayCommand(AddTypeFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Issue State", Command = new RelayCommand(AddStateFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Priority", Command = new RelayCommand(AddPriorityFilter, () => true) });
            DropdownCommands.Add(new TitledCommand() { Title = "Severity", Command = new RelayCommand(AddSeverityFilter, () => true) });

            return DropdownCommands;
        }

        private void AddIDFilter() { AddFilter("ID"); }
        private void AddDescriptionFilter() { AddFilter("Description"); }
        private void AddUserCreatedFilter() { AddFilter("CreatedBy"); }
        private void AddUserClosedFilter() { AddFilter("ClosedBy"); }
        private void AddContactFilter() { AddFilter("Contact"); }
        private void AddTypeFilter() { AddFilter("Type"); }
        private void AddStateFilter() { AddFilter("State"); }
        private void AddPriorityFilter() { AddFilter("Priority"); }
        private void AddSeverityFilter() { AddFilter("Severity"); }

        public void OpenIssueInNewTab(int issueID)
        {
            messenger.openIssue(issueID);
        }

        private void AddFilter(string newFilter)
        {
            if (String.IsNullOrWhiteSpace(Filter))
                Filter = newFilter + ":";
            else
                Filter += " " + newFilter + ":";

            // Stop timer now until user types again
            filterTimer.Stop();
        }

        private void ResetTimer()
        {
            filterTimer.Stop();
            filterTimer.Start();
        }

        public void RefreshTab()
        {
            AllIssues = dataAccess.GetAllIssues(Filter);
        }
    }
}
