using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Model;

namespace BugTracker.Common
{
    public class UserDataValidator
    {
        private HashSet<string> validationList;

        private List<string> validateOnIssueOpenFields = new List<string>()
        {
            "Description",
            "PlantContact",
            "VersionFound",
            "DetailedDescription",
            "StepsToReproduce",
            "Workaround",
            "Justification"
        };

        private List<string> validateOnIssueClosedFields = new List<string>()
        {
            "VersionFixed",
            "Fix",
            "VersionImplemented"
        };

        public UserDataValidator()
        {
            validationList = new HashSet<string>();
        }

        public string ValidateProperty(string propertyName, string propertyValue, int issueState)
        {
            if (!validationList.Contains(propertyName)) validationList.Add(propertyName);

            if (validateOnIssueOpenFields.Contains(propertyName) ||
                (validateOnIssueClosedFields.Contains(propertyName) && issueState == (int)eIssueState.Closed))
            {
                if (String.IsNullOrEmpty(propertyValue))
                {
                    return "Field cannot be empty";
                }
                else
                {
                    validationList.Remove(propertyName);
                    return null;
                }
            }
            else if (propertyName == "IssueResolution" && issueState == (int)eIssueState.Closed)
            {
                if (propertyValue == "0")
                {
                    return "Issue resolution must be selected to close the bug";
                }
                else
                {
                    validationList.Remove(propertyName);
                    return null;
                }
            }

            // We're not checking validation on this property
            return null;
        }
        
        public bool IsUserDataValid()
        {
            return validationList.Count == 0;
        }
    }
}
