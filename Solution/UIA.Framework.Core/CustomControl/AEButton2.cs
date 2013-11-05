using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AEButton2 : AEControlBase
    {

        public AEButton2(AutomationElement ae)
            : base(ae)
        {

        }

        public void Click()
        {
			object pattern = TryGetPattern(TogglePattern.Pattern);
			if (null != pattern)
				((TogglePattern)pattern).Toggle();
			else
				throw new NotSupportedException();
        }

    }
}
