using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEDataItem : AEControlBase
    {

		public AEDataItem(AutomationElement ae)
            : base(ae)
        {
        }

		public IList<AEDataGridCell> Cells
		{
			get
			{
				var cells = this.Self.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.IsEnabledProperty,true));
				if (null == cells)
					return null;
				IList<AEDataGridCell> returnList = new List<AEDataGridCell>();
				foreach (AutomationElement elem in cells)
				{
					var cell = ControlProvider.Transfer<AEDataGridCell>(elem);
					returnList.Add(cell);
				}
				return returnList;
			}
		}


		public void Click()
		{
			object pattern;
			if (this.Self.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
			{
				((InvokePattern)pattern).Invoke();
			}
			else if (this.Self.TryGetCurrentPattern(SelectionItemPattern.Pattern, out pattern))
			{
				((SelectionItemPattern)pattern).Select();
			}
			else if (this.Self.TryGetCurrentPattern(ScrollItemPattern.Pattern, out pattern))
			{
				((ScrollItemPattern)pattern).ScrollIntoView();
			}
		}

	}
}
