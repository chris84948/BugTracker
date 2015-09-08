using System;
using System.Linq;
using System.Windows.Input;
using BugTracker.MVVM;
using BugTracker.Model;
using BugTracker.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace BugTracker.ViewModels
{
    public class IssueViewModel : ScreenBase, IDataErrorInfo
    {
        public Issue Issue;
        protected bool saving;
        protected UserDataValidator validator;

        public IEnumerable<EnumValueDescription> IssueStateList
        {
            get
            {
                return EnumHelper.GetAllValuesAndDescriptions<eIssueState>();
            }
        }

        public IEnumerable<EnumValueDescription> IssueResolutionList
        {
            get
            {
                return EnumHelper.GetAllValuesAndDescriptions<eIssueResolution>().Where((x) => x.Value != (int)eIssueResolution.NA);
            }
        }

        public IEnumerable<EnumValueDescription> PriorityList
        {
            get
            {
                return EnumHelper.GetAllValuesAndDescriptions<ePriority>();
            }
        }

        public IEnumerable<EnumValueDescription> SeverityList
        {
            get
            {
                return EnumHelper.GetAllValuesAndDescriptions<eSeverity>();
            }
        }

        public int IssueID
        {
            get { return Issue.IssueID; }
            set
            {
                Issue.IssueID = value;
                OnPropertyChanged(() => IssueID);
            }
        }

        public string Description
        {
            get { return Issue.Description; }
            set
            {
                Issue.Description = value;
                OnPropertyChanged(() => Description);
                IsDirty = true;
            }
        }

        public string UserCreated
        {
            get { return Issue.UserCreated; }
            set
            {
                Issue.UserCreated = value;
                OnPropertyChanged(() => UserCreated);
            }
        }

        public string UserClosed
        {
            get { return Issue.UserClosed; }
            set
            {
                Issue.UserClosed = value;
                OnPropertyChanged(() => UserClosed);
            }
        }

        public DateTime? DateCreated
        {
            get { return Issue.DateCreated; }
            set
            {
                Issue.DateCreated = value;
                OnPropertyChanged(() => DateCreated);
            }
        }

        public DateTime? DateClosed
        {
            get { return Issue.DateClosed; }
            set
            {
                Issue.DateClosed = value;
                OnPropertyChanged(() => DateClosed);
            }
        }

        public string PlantContact
        {
            get { return Issue.PlantContact; }
            set
            {
                Issue.PlantContact = value;
                OnPropertyChanged(() => PlantContact);
                IsDirty = true;
            }
        }

        public int IssueType
        {
            get { return Issue.IssueType; }
            set
            {
                Issue.IssueType = value;
                OnPropertyChanged(() => IssueType);
            }
        }

        public int IssueState
        {
            get { return Issue.IssueState; }
            set
            {
                Issue.IssueState = value;
                OnPropertyChanged(() => IssueState);
                IsDirty = true;

                if (value == (int)eIssueState.Closed)
                {
                    IssueResolution = (int)eIssueResolution.Completed;
                    DateClosed = DateTime.Now.Date;
                    UserClosed = Environment.UserName;
                }
            }
        }

        public int IssueResolution
        {
            get { return Issue.IssueResolution; }
            set
            {
                Issue.IssueResolution = value;
                OnPropertyChanged(() => IssueResolution);
                IsDirty = true;
            }
        }

        public int Severity
        {
            get { return Issue.Severity; }
            set
            {
                Issue.Severity = value;
                OnPropertyChanged(() => Severity);
                IsDirty = true;
            }
        }

        public int Priority
        {
            get { return Issue.Priority; }
            set
            {
                Issue.Priority = value;
                OnPropertyChanged(() => Priority);
                IsDirty = true;
            }
        }

        public IssueViewModel()
        {
            this.Issue = new Issue();
            validator = new UserDataValidator();
        }

        public IssueViewModel(Issue issue)
        {
            this.Issue = issue;
            validator = new UserDataValidator();
        }

        /// <summary>
        /// Used for data validation
        /// </summary>
        public string Error
        {
            get { return ""; }
        }

        /// <summary>
        /// Data validation method
        /// Only acts when attempting to save a tab
        /// Will show error if tab is empty but requires data
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                if (!saving && validator.IsUserDataValid())
                    return null;
                else
                    return validator.ValidateProperty(columnName,
                                                      GetType().GetProperty(columnName).GetValue(this).ToString(),
                                                      IssueState);
            }
        }

    }
}
