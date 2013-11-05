using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEDataGridCell : AEControlBase
    {
		public AEDataGridCell(AutomationElement ae)
            : base(ae)
        {
        }

		

		public bool CanSetValue
		{
			get
			{
				var ivpa = this.Self.GetCurrentPropertyValue(AutomationElement.IsValuePatternAvailableProperty);
				return (bool)ivpa;
			}
		}

		public string Text
		{
			get
			{
				var pattern = TryGetPattern(ValuePattern.Pattern);
				if (null != pattern)
					return ((ValuePattern)pattern).Current.Value;
				else
					throw new NotSupportedException();
			}
		}

		public void Set(string text)
		{
			if (CanSetValue)
			{
				var pattern = TryGetPattern(ValuePattern.Pattern);
				if (null != pattern)
					((ValuePattern)pattern).SetValue(text);
				else
					throw new NotSupportedException();
			}
		}


	}
}
