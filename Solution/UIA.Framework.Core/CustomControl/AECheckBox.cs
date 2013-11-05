using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AECheckBox : AEControlBase
    {
        public AECheckBox(AutomationElement ae)
            : base(ae)
        {

        }


        public void Check(bool checkedIn)
        {
            var pattern = this.Self.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
			if (!checkedIn)
            {
                while (this.State)
                    pattern.Toggle();
            }
            else
            {
                while (!this.State)
                    pattern.Toggle();
            }
        }

        public bool State
        {
            get
            {
				object pattern = TryGetPattern(TogglePattern.Pattern);
				if (null != pattern)
				{
					ToggleState status = ((TogglePattern)pattern).Current.ToggleState;
					return (ToggleState.Off == status) ? false : true;
				}
				else
					throw new NotSupportedException();
            }
        }
    }
}
