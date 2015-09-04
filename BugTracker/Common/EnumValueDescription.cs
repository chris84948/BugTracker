using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Common
{
    public class EnumValueDescription
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public EnumValueDescription(int value, string desc)
        {
            this.Value = value;
            this.Description = desc;
        }
    }
}
