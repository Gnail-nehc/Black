using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIA.Framework.Core
{
    public class ControlNotFoundException : Exception
    {
        public ControlNotFoundException(string searchCondtions)
            : base(string.Format("Fail to find control! search condtions: {0}.", searchCondtions))
        {

        }
    }
}
