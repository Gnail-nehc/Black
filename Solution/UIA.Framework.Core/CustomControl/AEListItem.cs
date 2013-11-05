using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEListItem : AEControlBase
    {
		public AEListItem(AutomationElement ae)
            : base(ae)
        {
        }

		public void Click()
		{
			object pattern = TryGetPattern(SelectionItemPattern.Pattern);
			if (null != pattern)
			{
				((SelectionItemPattern)pattern).Select();
			}
			else
				throw new NotSupportedException();
		}


	}
}
