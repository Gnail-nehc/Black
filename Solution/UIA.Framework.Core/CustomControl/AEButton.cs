using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AEButton : AEControlBase
    {

        public AEButton(AutomationElement ae)
            : base(ae)
        {

        }

        public void Click()
        {
			object pattern = TryGetPattern(InvokePattern.Pattern);
			if (null != pattern)
				((InvokePattern)pattern).Invoke();
			else
				throw new NotSupportedException();
        }
        
    }
}
