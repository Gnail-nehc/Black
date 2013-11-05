using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AEText : AEControlBase
    {
        public AEText(AutomationElement ae)
            : base(ae)
        {
        }

        public string Text
        {
            get
            {
				var pattern = TryGetPattern(ValuePattern.Pattern);
				if (null != pattern)
					return ((ValuePattern)pattern).Current.Value;
				else
					throw new NotSupportedException();
            }
        }

    }
}
