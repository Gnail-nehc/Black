using UIA.Framework.Core.CustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core
{
    public class ControlProvider
    {
        public static T Transfer<T>(AutomationElement ae) where T : AEControlBase
        {
            return (T)Activator.CreateInstance(typeof(T), ae);
        }

		public static T Transfer<T>(AutomationElement topWindow,AutomationElement self) where T : AEControlBase
		{
			return (T)Activator.CreateInstance(typeof(T), topWindow,self);
		}
    }
}
