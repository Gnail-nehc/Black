using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AERadioButton : AEControlBase
    {
		public AERadioButton(AutomationElement ae)
            : base(ae)
        {
        }

		public void Select()
		{
			object pattern = TryGetPattern(SelectionItemPattern.Pattern);
			if (null != pattern)
				((SelectionItemPattern)pattern).Select();
			else
				throw new NotSupportedException();
		}

		public bool IsSelected
		{
			get
			{
				object pattern = TryGetPattern(SelectionItemPattern.Pattern);
				if (null != pattern)
					return ((SelectionItemPattern)pattern).Current.IsSelected;
				else
					throw new NotSupportedException();
			}
		}

	}
}
