using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public abstract class AEControlBase
    {
        private AutomationElement self;

        public AutomationElement Self
        {
            get { return self; }
            set { self = value; }
        }

        public bool IsEnable
        {
            get
            {
                return (null != this.Self) ? this.Self.Current.IsEnabled : false;
            }
        }

        public bool IsContent
        {
            get
            {
                return (null != this.Self) ? this.Self.Current.IsContentElement : false;
            }
        }

        public bool IsControl
        {
            get
            {
                return (null != this.Self) ? this.Self.Current.IsControlElement : false;
            }
        }

        protected AEControlBase(AutomationElement ae)
        {
            this.self = ae;
        }

		protected AEControlBase(AutomationElement topWindow, AutomationElement ae)
		{
			this.self = ae;
		}

		protected object TryGetPattern(AutomationPattern pattern,AutomationElement elementNeedToGet = null)
		{
			elementNeedToGet = elementNeedToGet ?? this.self;
			object returnPattern;
			elementNeedToGet.TryGetCurrentPattern(pattern, out returnPattern);
			return returnPattern ?? null;
		}
                
    }
}
