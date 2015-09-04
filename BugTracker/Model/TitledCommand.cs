using System;
using System.Windows.Input;

namespace BugTracker.Model
{
    public class TitledCommand
    {
        public String Title { get; set; }
        public ICommand Command { get; set; }
    }
}
