using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIA.Framework.Core;
using UIA.Application.DevExpressWpfAppDemo.ReusableComponent;
using System.Windows.Automation;
using UIA.Framework.Core.CustomControl;

namespace UIA.Application.DevExpressWpfAppDemo.TestCase
{
	[TestClass]
	public class _4221 : TestBase
	{
		public _4221()	: base("4221")
		{
		}

		[TestMethod]
		public void TC_4221()
		{
			this.Logger.ScenarioStart(1, "Navigate to Cedent Data Management");
			this.NavigateToCedentDataManagement();
			this.Logger.ScenarioEnd(1);

			this.Logger.ScenarioStart(2, "");
			
			this.Logger.ScenarioEnd(2);
		}
	}
}


