using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEHeaderItem : AEControlBase
    {
		public AEHeaderItem(AutomationElement ae)
            : base(ae)
        {
        }

		public string Name
		{
			get
			{
				var text = this.Self.FindFirst(TreeScope.Children,new PropertyCondition(SearchBy.ByControlType,ControlType.Text));
				if (null != text)
					return text.Current.Name;
				return this.Self.Current.Name;
			}
		
		}


	}
}
