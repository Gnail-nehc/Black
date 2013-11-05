using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEWindow : AEControlBase
    {
		public AEWindow(AutomationElement ae)
            : base(ae)
        {
        }

		public void Close()
		{
			object pattern = TryGetPattern(WindowPattern.Pattern);
			if (null != pattern)
			{
				((WindowPattern)pattern).Close();
			}
			else
				throw new NotSupportedException();
		}

		public void Maximize()
		{
			object pattern = TryGetPattern(WindowPattern.Pattern);
			if (null != pattern)
			{
				((WindowPattern)pattern).SetWindowVisualState(WindowVisualState.Maximized);
			}
			else
				throw new NotSupportedException();
		}

		public void Minimize()
		{
			object pattern = TryGetPattern(WindowPattern.Pattern);
			if (null != pattern)
			{
				((WindowPattern)pattern).SetWindowVisualState(WindowVisualState.Minimized);
			}
			else
				throw new NotSupportedException();
		}

		public void WaitForInputIdle(int timeoutMillisec)
		{
			object pattern = TryGetPattern(WindowPattern.Pattern);
			if (null != pattern)
			{
				((WindowPattern)pattern).WaitForInputIdle(timeoutMillisec);
			}
			else
				throw new NotSupportedException();
		}
	}
}
