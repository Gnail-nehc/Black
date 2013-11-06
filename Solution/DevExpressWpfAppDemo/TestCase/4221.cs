using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIA.Framework.Core;
using UIA.Application.DevExpressWpfAppDemo.ReusableComponent;
using System.Windows.Automation;
using UIA.Framework.Core.CustomControl;

namespace UIA.Application.DevExpressWpfAppDemo.TestCase
{
	[TestClass]
	public class _1 : TestBase
	{
		public _1()	: base("1")
		{
		}

		[TestMethod]
		public void TC_1()
		{
			this.Logger.ScenarioStart(1, "Navigate to XXX");
			
			this.Logger.ScenarioEnd(1);

			this.Logger.ScenarioStart(2, "");
			
			this.Logger.ScenarioEnd(2);
		}
	}
}


