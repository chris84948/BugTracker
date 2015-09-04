using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Common;

namespace BugTracker.Model
{
    class Bug
    {
        [SQLFieldAttribute("ID")]
        public int ID;

        [SQLFieldAttribute("VersionFound")]
        public string VersionFound;

        [SQLFieldAttribute("VersionFixed")]
        public string VersionFixed;

        [SQLFieldAttribute("DetailedDescription")]
        public string DetailedDescription;

        [SQLFieldAttribute("StepsToReproduce")]
        public string StepsToReproduce;

        [SQLFieldAttribute("Workaround")]
        public string Workaround;

        [SQLFieldAttribute("Fix")]
        public string Fix;
    }
}
