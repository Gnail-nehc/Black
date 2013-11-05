using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEMenu : AEControlBase
    {

		public AEMenu(AutomationElement ae)
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

		public void Expand()
		{
			object pattern = TryGetPattern(ExpandCollapsePattern.Pattern);
			if (null != pattern)
				((ExpandCollapsePattern)pattern).Expand();
			else
				throw new NotSupportedException();
		}
	}
}
