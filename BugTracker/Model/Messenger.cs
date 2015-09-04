using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugTracker.ViewModels;
using System.Threading.Tasks;

namespace BugTracker.Model
{
    public class Messenger
    {
        public Action<int> openIssue;
        public Action<ScreenBase> closeTab;
        public Action tabSaveStatusChanged;
    }
}
