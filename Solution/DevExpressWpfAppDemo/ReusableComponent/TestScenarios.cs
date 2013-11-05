using UIA.Framework.Core;
using UIA.Framework.Core.CustomControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace UIA.Application.DevExpressWpfAppDemo.ReusableComponent
{
    public static class TestScenarios
	{
		public static void NavigateToCedentDataManagement(this TestBase @this)
		{
			//@this.ClickTabItem("ViewTab");
			//@this.ExpandMenu("NavigationMenu");
			//@this.ClickButton("CedentDataMgmt");
			//SendKeys.SendWait("{ENTER}");

			//@this.App.Self.SetFocus();
			//SendKeys.SendWait("%");
			//SendKeys.SendWait("v");
			//SendKeys.SendWait("n");
			//SendKeys.SendWait("c");
			var dockLayoutManager = @this.Gui("GroupDockLayout");
			var leftLayoutGroup = @this.App.FindFirstChild(@this.App.FindFirstChild(dockLayoutManager));
			var rightLayoutGroup = @this.App.FindNextSibling(leftLayoutGroup);
			var target = @this.App.FindFirstChild(rightLayoutGroup);
			string actualRightPaneName = target.Current.Name;
			@this.VerificationForConditionTrue(actualRightPaneName.Contains(@this.GlobalData["RightPanelExpectedCaption"]), "The right Sub should be " + @this.GlobalData["RightPanelExpectedCaption"], "Verify the right pane: " + actualRightPaneName + " pass.");
			target = @this.App.FindFirstChild(@this.App.FindFirstChild(leftLayoutGroup));
			@this.VerificationForConditionTrue(target.Current.Name.Contains(@this.GlobalData["LeftPanelExpectedCaption"]), "The left Sub should be " + @this.GlobalData["LeftPanelExpectedCaption"], "Verify the right pane: " + actualRightPaneName + " pass.");
		}

		public static void VerifyCedentSearchConditions(this TestBase @this)
		{
			var searchBtn = @this.Gui("btnSearch");
			var nextBtn = @this.App.FindNextSibling(searchBtn);
			Assert.IsTrue(nextBtn.Current.Name.Contains("country"), "The button next to search button is not Country button.");
			nextBtn = @this.App.FindNextSibling(nextBtn);
			int currentMonth = DateTime.Now.Month;
			if (currentMonth < 9)
			{
				Assert.IsTrue(nextBtn.Current.Name.Equals(Convert.ToString(DateTime.Now.Year)), "the underwriting year should be set by default to the current year if the current month is less than September.");
			}
			else
			{
				Assert.IsTrue(nextBtn.Current.Name.Equals(Convert.ToString(DateTime.Now.Year + 1)), "the underwriting year should be set by default to the current year if the current month is less than September.");
			}
			@this.ClickToggleButton(@this.App.FindNextSibling(nextBtn));
			SendKeys.SendWait("{ENTER}");
			var window = @this.App.FindFirstChild();
			var radioBtn1 = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindFirstChild(window)));
			var radioBtn2 = @this.App.FindNextSibling(radioBtn1);
			var radioBtn3 = @this.App.FindNextSibling(radioBtn2);
			@this.AssertionForCedentConditionWithRadioButton(radioBtn1);
			@this.AssertionForCedentConditionWithRadioButton(radioBtn2);
			@this.AssertionForCedentConditionWithRadioButton(radioBtn3);
		}

		private static void AssertionForCedentConditionWithRadioButton(this TestBase @this, AutomationElement radioBtn)
		{
			@this.VerificationForConditionTrue(radioBtn.Current.ControlType == ControlType.RadioButton, "It should be radio button.");
			@this.VerificationForConditionTrue(@this.GlobalData["VerifyRadioBtnConditions"].Contains(radioBtn.Current.Name), "It should be one of the " + @this.GlobalData["VerifyRadioBtnConditions"], "Verify radio button " + radioBtn.Current + " pass.");
		}

		public static void SearchCedentByConditions(this TestBase @this, string cedentName = "", string country = "", string year = "", string rwsId = "", string sicsId = "", string rppId = "", string selectedCedent = "")
		{
			var dxTab = @this.Gui("DXTabControl");
			var tabItem = @this.App.FindFirstChild(dxTab);
			var searchBtn = @this.App.FindNextSibling(@this.Gui("editBox"));
			VerifyTabs(@this);

			if ("" != cedentName)
			{
				@this.EnterText("editBox", cedentName);
			}
			if ("" != country)
			{
				var addCountryBtn = @this.App.FindNextSibling(searchBtn);
				@this.ClickToggleButton(addCountryBtn);
				@this.ClickToggleButton(addCountryBtn);
				SendKeys.SendWait("{ENTER}");
				var list = @this.App.FindFirstChild(@this.App.FindFirstChild(@this.App.FindFirstChild()));
				@this.SelectListItemByChildText(list, country);
			}
			if ("" != year)
			{
				var yearBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn));
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				var listview = @this.App.FindFirstChild(@this.App.FindFirstChild());
				@this.SelectListItem(listview, year);
				SendKeys.SendWait("{ENTER}");
			}
			var idBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn)));
			if (rwsId != "" || sicsId != "" || rppId != "")
			{
				@this.ClickToggleButton(idBtn);
				SendKeys.SendWait("{ENTER}");
				var window = @this.App.FindFirstChild();
				var edit = @this.App.FindFirstChild(window);
				ControlProvider.Transfer<AEEdit>(edit).Set(rwsId);
				AutomationElement rb;
				if (rwsId != "")
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(edit));
				else if(sicsId != "")
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(edit)));
				else
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(edit))));
				bool isSelected = ControlProvider.Transfer<AERadioButton>(rb).IsSelected;
				if (!isSelected)
					ControlProvider.Transfer<AERadioButton>(rb).Select();
				SendKeys.SendWait("{ENTER}");
			}

			searchBtn.SetFocus();
			@this.ClickButton(searchBtn);
			Thread.Sleep(7*1000);
			var resultList = @this.App.FindLastChild(@this.App.FindParent(searchBtn));
			var firstItem = @this.App.FindFirstChild(resultList);
			if (firstItem != null)
			{
				resultList.SetFocus();
				if (selectedCedent == "")
					@this.SelectListItem(resultList, 0);
				else
					@this.SelectListItem(resultList, selectedCedent);
				Thread.Sleep(8000);
				var datagrid = @this.App.FindDescendant(SearchBy.ByControlType, ControlType.DataGrid);
				var datarows = ControlProvider.Transfer<AEDataGrid>(datagrid).DataRows;
				if(datarows == null)
				{
					@this.Logger.LogInfo("DB Manager displays a blank table when no databases are registered for the selected Cedent");
					Assert.IsTrue(true);
				}
				VerifyCedentInfoTable(@this);
				VerifyButtonsInDBManager(@this);
			}
			else
			{
				@this.Logger.LogError("Not found Cedent under current condition");
				Assert.Fail();
			}
			//Clean up
			if ("" != cedentName)
			{
				@this.EnterText("editBox", "");
			}
			if ("" != year)
			{
				var yearBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn));
				yearBtn.SetFocus();
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				Thread.Sleep(500);
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				var listview = @this.App.FindFirstChild(@this.App.FindFirstChild());
				int defaultYear = DateTime.Now.Month < 9 ? DateTime.Now.Year : DateTime.Now.Year + 1;
				@this.SelectListItem(listview, Convert.ToString(defaultYear));
				SendKeys.SendWait("{ENTER}");
			}
			if (rwsId != "" || sicsId != "" || rppId != "")
			{
				@this.ClickToggleButton(idBtn);
				SendKeys.SendWait("{ENTER}");
				var window = @this.App.FindFirstChild();
				var edit = @this.App.FindFirstChild(window);
				ControlProvider.Transfer<AEEdit>(edit).Set("");
			}
		}

		private static void VerifyTabs(this TestBase @this)
		{
			var dxTab = @this.Gui("DXTabControl");
			var tabItem = @this.App.FindFirstChild(dxTab);
			for (int i = 6; i > 0; i--)
			{
				@this.VerificationForConditionTrue(@this.GlobalData["VerifyTabs"].Contains(tabItem.Current.Name.Replace("DXTabItem", "")), "It should be one of the " + @this.GlobalData["VerifyTabs"]);
				tabItem = @this.App.FindNextSibling(tabItem);
			}
		}

		private static void VerifyCedentInfoTable(this TestBase @this)
		{
			var dxTab = @this.Gui("DXTabControl");
			var cedentInfoText = @this.App.FindFirstChild(@this.App.FindFirstChild(@this.App.FindFirstChild(dxTab)));
			var cedent = @this.App.FindNextSibling(cedentInfoText);
			var uy = @this.App.FindNextSibling(@this.App.FindNextSibling(cedent));
			var rwsid = @this.App.FindNextSibling(@this.App.FindNextSibling(uy));
			var rppid = @this.App.FindNextSibling(@this.App.FindNextSibling(rwsid));

			@this.VerificationForConditionTrue(cedent.Current.Name.Contains("Cedent"), "It should be Cedent not " + cedent.Current.Name, "Verify Cedent column in CedentInfo table Pass!");
			@this.VerificationForConditionTrue(uy.Current.Name.Contains("UY"), "It should be UY not "+ uy.Current.Name, "Verify UY column in CedentInfo table Pass!");
			@this.VerificationForConditionTrue(rwsid.Current.Name.Contains("RWS ID"), "It should be RWS ID not " + rwsid.Current.Name, "Verify RWS ID column in CedentInfo table Pass!");
			@this.VerificationForConditionTrue(rppid.Current.Name.Contains("RPP ID"), "It should be RPP ID not " + rppid, "Verify RPP ID column in CedentInfo table Pass!");
		}

		private static void VerifyButtonsInDBManager(this TestBase @this)
		{
			var dxTab = @this.Gui("DXTabControl");
			var retrieve = @this.App.FindFirstChild(@this.App.FindFirstChild(dxTab));
			var buttons = @this.App.FindChildren(SearchBy.ByControlType, ControlType.Button, retrieve);
			string[] buttonText = @this.GlobalData["ButtonsInDBManager"].Split(',');
			int i = 0;
			foreach (AutomationElement button in buttons)
			{
				Assert.IsNotNull(button);
				var text = @this.App.FindFirstChild(button);
				string expected = buttonText[i++];
				if (text != null)
				{
					@this.VerificationForConditionTrue(text.Current.Name.Contains(expected), "The button should call " + expected + " not " + text.Current.Name, "Verify button " + expected + " Pass!");
				}
				else
				{
					expected = expected.Replace("-", "");
					@this.VerificationForConditionTrue(button.Current.AutomationId.Contains(expected), "The button should call " + expected + " not " + button.Current.AutomationId, "Verify button " + expected + " Pass!");
				}
			}
		}

		public static void SearchAllCedentsAndVerifyUI(this TestBase @this, string selectedCedent = "", bool ifSelectCedent = true)
		{
			Thread.Sleep(5000);
			//click Search button
			SendKeys.SendWait("{TAB 7}");
			SendKeys.SendWait("{ENTER}");
			var searchBtn = @this.App.FindNextSibling(@this.Gui("editBox"));
			var resultList = @this.App.FindLastChild(@this.App.FindParent(searchBtn));
			var firstItem = @this.App.FindFirstChild(resultList);
			if (firstItem != null)
			{
				var dxTab = @this.Gui("DXTabControl");
				
				if (ifSelectCedent)
				{
					resultList.SetFocus();
					if (selectedCedent == "")
						@this.SelectListItem(resultList, 0);
					else
						@this.SelectListItemByChildText(resultList, selectedCedent);
					Thread.Sleep(8000);
					var datagrid = @this.App.FindDescendant(SearchBy.ByControlType, ControlType.DataGrid);
					var datarows = ControlProvider.Transfer<AEDataGrid>(datagrid).DataRows;
					if (datarows == null)
					{
						@this.Logger.LogInfo("DB Manager displays a blank table when no databases are registered for the selected Cedent");
						Assert.IsTrue(true);
					}
					VerifyButtonsInDBManager(@this);
				}
				else
				{
					VerifyTabs(@this);
				}
				VerifyCedentInfoTable(@this);
			}
			else
			{
				@this.Logger.LogError("Not found Cedent");
				Assert.Fail();
			}
		}

		public static void SearchCedentByInvalidConditions(this TestBase @this, string cedentName = "", string country = "", string year = "", string rwsId = "", string sicsId = "", string rppId = "")
		{
			var dxTab = @this.Gui("DXTabControl");
			var tabItem = @this.App.FindFirstChild(dxTab);
			var searchBtn = @this.App.FindNextSibling(@this.Gui("editBox"));
			VerifyTabs(@this);
			if ("" != cedentName)
			{
				@this.EnterText("editBox", cedentName);
			}
			if ("" != country)
			{
				var addCountryBtn = @this.App.FindNextSibling(searchBtn);
				@this.ClickToggleButton(addCountryBtn);
				@this.ClickToggleButton(addCountryBtn);
				SendKeys.SendWait("{ENTER}");
				var list = @this.App.FindFirstChild(@this.App.FindFirstChild(@this.App.FindFirstChild()));
				@this.SelectListItemByChildText(list, country);
			}
			if ("" != year)
			{
				var yearBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn));
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				var listview = @this.App.FindFirstChild(@this.App.FindFirstChild());
				@this.SelectListItem(listview, year);
				SendKeys.SendWait("{ENTER}");
			}
			var idBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn)));
			if (rwsId != "" || sicsId != "" || rppId != "")
			{
				@this.ClickToggleButton(idBtn);
				SendKeys.SendWait("{ENTER}");
				var window = @this.App.FindFirstChild();
				var edit = @this.App.FindFirstChild(window);
				ControlProvider.Transfer<AEEdit>(edit).Set(rwsId);
				AutomationElement rb;
				if (rwsId != "")
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(edit));
				else if (sicsId != "")
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(edit)));
				else
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(edit))));
				bool isSelected = ControlProvider.Transfer<AERadioButton>(rb).IsSelected;
				if (!isSelected)
					ControlProvider.Transfer<AERadioButton>(rb).Select();
				SendKeys.SendWait("{ENTER}");
			}
			searchBtn.SetFocus();
			@this.ClickButton(searchBtn);
			Thread.Sleep(7000);
			var resultList = @this.App.FindLastChild(@this.App.FindParent(searchBtn));
			var firstItem = @this.App.FindFirstChild(resultList);
			Assert.IsNull(firstItem, "Cedent should not be found under invalid conditions");
			//Clean up
			if ("" != cedentName)
			{
				@this.EnterText("editBox", "");
			}
			if ("" != year)
			{
				var yearBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn));
				yearBtn.SetFocus();
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				Thread.Sleep(500);
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				var listview = @this.App.FindFirstChild(@this.App.FindFirstChild());
				int defaultYear = DateTime.Now.Month < 9 ? DateTime.Now.Year : DateTime.Now.Year + 1;
				@this.SelectListItem(listview, Convert.ToString(defaultYear));
				SendKeys.SendWait("{ENTER}");
			}
			if (rwsId != "" || sicsId != "" || rppId != "")
			{
				@this.ClickToggleButton(idBtn);
				SendKeys.SendWait("{ENTER}");
				var window = @this.App.FindFirstChild();
				var edit = @this.App.FindFirstChild(window);
				ControlProvider.Transfer<AEEdit>(edit).Set("");
			}

		}

		public static void AddDatabaseAndVerifyUI(this TestBase @this)
		{
			@this.ClickButton("AddBtn");
			var catAutomation = @this.App.GetTopWindowContainsName("CAT Automation");
			//@this.ClickToggleButton("AddFilesBtn", catAutomation);
			SendKeys.SendWait("{TAB 4}");
			SendKeys.SendWait("{ENTER}");
			Thread.Sleep(500);
			var exposure = @this.Gui("ExposureGroup", catAutomation);
			var rb1 = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindFirstChild(exposure)));
			var rb2 = @this.App.FindNextSibling(rb1);
			var rb3 = @this.App.FindNextSibling(rb2);
			var rb4= @this.App.FindNextSibling(rb3);
			@this.VerificationForConditionTrue(rb1.Current.Name.Contains("Submission"), "The radio button should be Submission", "Verify radio button: Submission PASS!");
			@this.VerificationForConditionTrue(rb2.Current.Name.Contains("Staging area"), "The radio button should be Staging area", "Verify radio button: Staging area PASS!");
			@this.VerificationForConditionTrue(rb3.Current.Name.Contains("Other path"), "The radio button should be Other path", "Verify radio button: Other path PASS!");
			@this.VerificationForConditionTrue(rb4.Current.Name.Contains("Blank"), "The radio button should be Blank", "Verify radio button: Blank PASS!");
			var checkbox = @this.App.FindLastChild(exposure);
			@this.VerificationForConditionTrue(checkbox.Current.Name.Contains("Upgrade automatically"), "The checkbox should be Upgrade automatically", "Verify checkbox: Upgrade automatically PASS!");
		}

		public static void DatabaseUpgradeAndVerifyUI(this TestBase @this)
		{
			@this.ClickButton("AddBtn");
			var catAutomation = @this.App.GetTopWindowContainsName("CAT Automation");
			//@this.ClickToggleButton("AddFilesBtn", catAutomation);
			SendKeys.SendWait("{TAB 4}");
			SendKeys.SendWait("{ENTER}");
			Thread.Sleep(500);
			var exposure = @this.Gui("ExposureGroup", catAutomation);
			var checkbox = @this.App.FindLastChild(exposure);
			Assert.IsTrue(checkbox.Current.Name.Contains("Upgrade automatically"), "The radio button should be Upgrade automatically");
			bool isChecked = ControlProvider.Transfer<AECheckBox>(checkbox).State;
			Assert.IsTrue(isChecked, "The checkbox should be uncheck by default");
		}

		public static void VerifyColumnsUnderDBMangerTab(this TestBase @this)
		{
			var datagrid = @this.App.FindDescendant(SearchBy.ByControlType, ControlType.DataGrid);
			string verifiedColumns = @this.GlobalData["VerifyColumnsUnderDBManager"];
			string[] arrColumn = verifiedColumns.Split(',');
			int i = 0;
			foreach (var header in ControlProvider.Transfer<AEDataGrid>(datagrid).Columns)
			{
				string verifiedText = arrColumn[i++];
				Assert.AreEqual(verifiedText, header.Name, false, System.Globalization.CultureInfo.CurrentUICulture, "The column should be " + verifiedText);
				@this.Logger.LogInfo("Verify column: " + verifiedText + " pass. Actual name in script: " + header.Name);

			}
		}

		public static void GotoAnalysisTabAfterCedentSelected(this TestBase @this)
		{
			Thread.Sleep(7000);
			@this.ClickButton("AddBtn");
			Thread.Sleep(2000);
			SendKeys.SendWait("{DOWN}");
			SendKeys.SendWait("{TAB}");
			SendKeys.SendWait("{DOWN}");
			Thread.Sleep(4000);
		}

		public static void GotoRMSAnalysisTab(this TestBase @this, string cedentName = "", string country = "", string year = "", string rwsId = "", string sicsId = "", string rppId = "", string selectedCedent = "")
		{
			var dxTab = @this.Gui("DXTabControl");
			var tabItem = @this.App.FindFirstChild(dxTab);
			var searchBtn = @this.App.FindNextSibling(@this.Gui("editBox"));
			if ("" != cedentName)
			{
				@this.EnterText("editBox", cedentName);
			}
			if ("" != country)
			{
				var addCountryBtn = @this.App.FindNextSibling(searchBtn);
				@this.ClickToggleButton(addCountryBtn);
				@this.ClickToggleButton(addCountryBtn);
				SendKeys.SendWait("{ENTER}");
				var list = @this.App.FindFirstChild(@this.App.FindFirstChild(@this.App.FindFirstChild()));
				@this.SelectListItemByChildText(list, country);
			}
			if ("" != year)
			{
				var yearBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn));
				@this.ClickToggleButton(yearBtn);
				SendKeys.SendWait("{ENTER}");
				var listview = @this.App.FindFirstChild(@this.App.FindFirstChild());
				@this.SelectListItem(listview, year);
				SendKeys.SendWait("{ENTER}");
			}
			var idBtn = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(searchBtn)));
			if (rwsId != "" || sicsId != "" || rppId != "")
			{
				@this.ClickToggleButton(idBtn);
				SendKeys.SendWait("{ENTER}");
				var window = @this.App.FindFirstChild();
				var edit = @this.App.FindFirstChild(window);
				ControlProvider.Transfer<AEEdit>(edit).Set(rwsId);
				AutomationElement rb;
				if (rwsId != "")
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(edit));
				else if (sicsId != "")
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(edit)));
				else
					rb = @this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(@this.App.FindNextSibling(edit))));
				bool isSelected = ControlProvider.Transfer<AERadioButton>(rb).IsSelected;
				if (!isSelected)
					ControlProvider.Transfer<AERadioButton>(rb).Select();
				SendKeys.SendWait("{ENTER}");
			}

			searchBtn.SetFocus();
			@this.ClickButton(searchBtn);
			Thread.Sleep(7 * 1000);
			var resultList = @this.App.FindLastChild(@this.App.FindParent(searchBtn));
			var firstItem = @this.App.FindFirstChild(resultList);
			if (firstItem != null)
			{
				resultList.SetFocus();
				if (selectedCedent == "")
					@this.SelectListItem(resultList, 0);
				else
					@this.SelectListItem(resultList, selectedCedent);
				Thread.Sleep(7000);
				//VerifyCedentInfoTable(@this);
				//VerifyButtonsInDBManager(@this);
				@this.ClickButton("AddBtn");
				Thread.Sleep(2000);
				SendKeys.SendWait("{DOWN}");
				SendKeys.SendWait("{TAB}");
				SendKeys.SendWait("{DOWN}");
				Thread.Sleep(4000);
			}
			else
			{
				@this.Logger.LogError("Not found Cedent under current condition");
				Assert.Fail();
			}
		}


		public static void VerifyCollapsibleAreasUnderRMSAnalysis(this TestBase @this)
		{
			var catAutomation = @this.App.GetTopWindowContainsName("CAT Automation");
			var barManager = @this.Gui("BarManager", catAutomation);
			barManager = @this.Gui("BarManager", barManager);
			var groups = @this.App.FindChildren(SearchBy.ByControlType, ControlType.Group, barManager);
			@this.VerificationForConditionTrue(groups[5].Current.Name.Contains(@this.TestData["collapsibleArea1"]), "The control should be" + @this.TestData["collapsibleArea1"], "Verify control " + @this.TestData["collapsibleArea1"] + " PASS!");
			@this.VerificationForConditionTrue(groups[6].Current.Name.Contains(@this.TestData["collapsibleArea2"]), "The control should be" + @this.TestData["collapsibleArea2"], "Verify control " + @this.TestData["collapsibleArea2"] + " PASS!");
			@this.VerificationForConditionTrue(groups[7].Current.Name.Contains(@this.TestData["collapsibleArea3"]), "The control should be" + @this.TestData["collapsibleArea3"], "Verify control " + @this.TestData["collapsibleArea3"] + " PASS!");
			@this.VerificationForConditionTrue(groups[8].Current.Name.Contains(@this.TestData["collapsibleArea4"]), "The control should be" + @this.TestData["collapsibleArea4"], "Verify control " + @this.TestData["collapsibleArea4"] + " PASS!");
			@this.VerificationForConditionTrue(groups[9].Current.Name.Contains(@this.TestData["collapsibleArea5"]), "The control should be" + @this.TestData["collapsibleArea5"], "Verify control " + @this.TestData["collapsibleArea5"] + " PASS!");
		}

		public static void VerifyRDMsHierarchyWithSpecificCedent(this TestBase @this, string cedentName = "")
		{
			Thread.Sleep(7000);
			//Open the first analysis result
			SendKeys.SendWait("{TAB 6}");
			SendKeys.SendWait("{+}");
			var catAutomation = @this.App.GetTopWindowContainsName("CAT Automation");
			var dataPanel = @this.Gui("DataPanel", catAutomation);
			var items = @this.App.FindChildren(SearchBy.ByControlType, ControlType.TreeItem, dataPanel);
			if (items != null & items.Count > 0)
			{
				@this.VerificationForConditionTrue(items[0].Current.Name.Contains("AnalysisDatabase"), "Verify analysis result under RDMs fail", "PASS: analysis result exist under RDMs");
				@this.VerificationForConditionTrue(@this.App.FindFirstChild(items[0]).Current.Name.Contains("RDM"), "The prefix of the analysis result node should be RDM.", "PASS - Verify analysis result node is RDM as prefix");
				@this.VerificationForConditionTrue(items[1].Current.Name.Contains("AnalysisResult"), "Verify treaties under analysis result fail", "PASS:  treaties exist under analysis result");
				var headerPanel = @this.App.FindPreviousSibling(dataPanel);
				int headerCounter=1;
				foreach(AutomationElement header in @this.App.FindChildren(SearchBy.ByControlType,ControlType.HeaderItem))
				{
					string actualHeader = header.Current.Name;
					string expectedHeader = @this.GlobalData["AnalysisResultGridHeader"+headerCounter];
					@this.VerificationForConditionTrue(actualHeader.Equals(expectedHeader,StringComparison.CurrentCultureIgnoreCase),"Expected header: "+expectedHeader+" ; Actual Header: "+actualHeader,"Verify header "+actualHeader+" PASS!");
					++headerCounter;
				}
			}
			else
			{
				if(cedentName!="")
					@this.Logger.LogWarning("Not found analysis in the cedent " + cedentName);
				Assert.Fail();
			}
		}

	}
}
