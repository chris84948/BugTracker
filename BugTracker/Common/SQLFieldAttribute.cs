using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Common
{
    class SQLFieldAttribute : Attribute
    {
        public readonly string SQLField;

        public SQLFieldAttribute(string sqlField)
        {
            this.SQLField = sqlField;
        }
    }
}
