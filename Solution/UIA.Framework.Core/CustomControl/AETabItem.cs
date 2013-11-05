using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AETabItem : AEControlBase
    {
        public AETabItem(AutomationElement ae)
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
    }
}
