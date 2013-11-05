using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AETreeItem : AEControlBase
    {

        public AETreeItem(AutomationElement ae)
            : base(ae)
        {

        }

        public void Select()
        {
			object pattern;
			if (this.Self.TryGetCurrentPattern(SelectionItemPattern.Pattern, out pattern))
			{
				((SelectionItemPattern)pattern).Select();
			}
			else if (this.Self.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
			{
				((InvokePattern)pattern).Invoke();
			}
			else
				throw new NotSupportedException();
        }

		public void Expand()
		{
			object pattern = TryGetPattern(ExpandCollapsePattern.Pattern);
			if (null != pattern)
			{
				var ptn = pattern as ExpandCollapsePattern;
				ptn.Expand();
			}
			else
				throw new NotSupportedException();
		}

		public bool IsSelected
		{
			get
			{
				object pattern = TryGetPattern(SelectionItemPattern.Pattern);
				if (null != pattern)
				{
					var ptn = pattern as SelectionItemPattern;
					return ptn.Current.IsSelected;
				}
				else
					throw new NotSupportedException();
			}
		}

    }
}
