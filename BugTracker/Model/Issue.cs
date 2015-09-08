using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Common;

namespace BugTracker.Model
{
    public class Issue
    {
        [SearchAttribute(true)]
        [SQLFieldAttribute("ID")]
        public int IssueID;

        [SearchAttribute(true)]
        [SQLFieldAttribute("Description")]
        public string Description;

        [SearchAttribute(true)]
        [SQLFieldAttribute("UserCreated")]
        public string UserCreated;

        [SearchAttribute(true)]
        [SQLFieldAttribute("UserClosed")]
        public string UserClosed;

        [SearchAttribute(false)]
        [SQLFieldAttribute("DateCreated")]
        public DateTime? DateCreated;

        [SearchAttribute(false)]
        [SQLFieldAttribute("DateClosed")]
        public DateTime? DateClosed;

        [SearchAttribute(true)]
        [SQLFieldAttribute("PlantContact")]
        public string PlantContact;

        [SearchAttribute(true)]
        [SQLFieldAttribute("IssueType")]
        public int IssueType;

        [SearchAttribute(true)]
        [SQLFieldAttribute("IssueState")]
        public int IssueState;

        [SearchAttribute(true)]
        [SQLFieldAttribute("IssueResolution")]
        public int IssueResolution;

        [SearchAttribute(true)]
        [SQLFieldAttribute("Priority")]
        public int Priority;

        [SearchAttribute(true)]
        [SQLFieldAttribute("Severity")]
        public int Severity;
    }
}
