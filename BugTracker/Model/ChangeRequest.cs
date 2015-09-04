using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Common;

namespace BugTracker.Model
{
    class ChangeRequest
    {
        [SQLFieldAttribute("ID")]
        public int ID;

        [SQLFieldAttribute("VersionImplemented")]
        public string VersionImplemented;

        [SQLFieldAttribute("DetailedDescription")]
        public string DetailedDescription;

        [SQLFieldAttribute("Justification")]
        public string Justification;
    }
}
