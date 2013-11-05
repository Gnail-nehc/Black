using UIA.Framework.Core;
using UIA.Framework.Core.CustomControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Application.DevExpressWpfAppDemo.ReusableComponent
{
    static class CommonAction
    {
        public static void ClickButton(this TestBase @this, Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null)
        {
            var target = @this.App.FindDescendant(searchCriteria, retrieveControl);
            ControlProvider.Transfer<AEButton>(target).Click();
        }

        public static void ClickButton(this TestBase @this, AutomationProperty searchBy, object value, AutomationElement retrieveControl = null)
        {
            var target = @this.App.FindDescendant(searchBy, value, retrieveControl);
            ControlProvider.Transfer<AEButton>(target).Click();
        }

		public static void ClickButton(this TestBase @this, string controlName, AutomationElement retrieveControl = null)
		{
			var target = @this.Gui(controlName, retrieveControl);
			ControlProvider.Transfer<AEButton>(target).Click();
		}

		public static void ClickButton(this TestBase @this, AutomationElement btn)
		{
			ControlProvider.Transfer<AEButton>(btn).Click();
		}

		public static void ClickToggleButton(this TestBase @this, string controlName, AutomationElement retrieveControl = null)
		{
			var target = @this.Gui(controlName, retrieveControl);
			ControlProvider.Transfer<AEButton2>(target).Click();
		}

		public static void ClickToggleButton(this TestBase @this, AutomationElement btn)
		{
			ControlProvider.Transfer<AEButton2>(btn).Click();
		}

        public static void ClickRibbonTab(this TestBase @this, AutomationProperty searchBy, object value, AutomationElement retrieveControl = null)
        {
            var target = @this.App.FindDescendant(searchBy, value, retrieveControl);
            ControlProvider.Transfer<AETabItem>(target).Select();
        }

		public static void ClickRibbonTab(this TestBase @this, string controlName, AutomationElement retrieveControl = null)
		{
			var target = @this.Gui(controlName, retrieveControl);
			ControlProvider.Transfer<AETabItem>(target).Select();
		}

        public static void EnterText(this TestBase @this, AutomationProperty searchBy, object value, string text, AutomationElement retrieveControl = null)
        {
            var target = @this.App.FindDescendant(searchBy, value, retrieveControl);
            ControlProvider.Transfer<AEEdit>(target).Set(text);
        }

		public static void EnterText(this TestBase @this, string controlName, string text, AutomationElement retrieveControl = null)
		{
			var target = @this.Gui(controlName, retrieveControl);
			ControlProvider.Transfer<AEEdit>(target).Set(text);
		}

		public static void SelectListItemByChildText(this TestBase @this, AutomationElement list, string itemText)
		{
			ControlProvider.Transfer<AEList>(list).SelectByChildText(itemText);
		}

		public static void SelectListItem(this TestBase @this, AutomationElement list, string itemText)
		{
			ControlProvider.Transfer<AEList>(list).Select(itemText);
		}

		public static void SelectListItem(this TestBase @this, AutomationElement list, int itemIndex)
		{
			ControlProvider.Transfer<AEList>(list).Select(itemIndex);
		}

		public static void SelectList(this TestBase @this, AutomationElement combo, string itemText)
		{
			ControlProvider.Transfer<AEComboBox>(combo).Select(itemText);
		}

        public static void SelectList(this TestBase @this, AutomationProperty searchBy, object value, string itemText, AutomationElement retrieveControl = null)
        {
            var target = @this.App.FindDescendant(searchBy, value, retrieveControl);
            ControlProvider.Transfer<AEComboBox>(target).Select(itemText);
        }

		public static void SelectList(this TestBase @this, string controlName, string itemText, AutomationElement retrieveControl = null)
		{
			var target = @this.Gui(controlName, retrieveControl);
			ControlProvider.Transfer<AEComboBox>(target).Select(itemText);
		}

        public static void SelectList(this TestBase @this, AutomationProperty searchBy, object value, int itemIndex, AutomationElement retrieveControl = null)
        {
            var target = @this.App.FindDescendant(searchBy, value, retrieveControl);
            ControlProvider.Transfer<AEComboBox>(target).Select(itemIndex);
        }

		public static void SelectList(this TestBase @this, string controlName, int itemIndex, AutomationElement retrieveControl = null)
		{
			var target = @this.Gui(controlName, retrieveControl);
			ControlProvider.Transfer<AEComboBox>(target).Select(itemIndex);
		}

        public static void SelectListByPreviousControl(this TestBase @this, AutomationProperty searchBy, object value, int itemIndex, AutomationElement labelRetrievedControl = null)
        {
            var label = @this.App.FindDescendant(searchBy, value, labelRetrievedControl);
            var comboBox = @this.App.FindNextSibling(label);
            ControlProvider.Transfer<AEComboBox>(comboBox).Select(itemIndex);
        }

		public static void SelectListByPreviousControl(this TestBase @this, string controlName, int itemIndex, AutomationElement labelRetrievedControl = null)
		{
			var label = @this.Gui(controlName, labelRetrievedControl);
			var comboBox = @this.App.FindNextSibling(label);
			ControlProvider.Transfer<AEComboBox>(comboBox).Select(itemIndex);
		}

        public static void SelectListByPreviousControl(this TestBase @this, AutomationProperty searchBy, object value, string itemText, AutomationElement labelRetrievedControl = null)
        {
            var label = @this.App.FindDescendant(searchBy, value, labelRetrievedControl);
            var comboBox = @this.App.FindNextSibling(label);
            ControlProvider.Transfer<AEComboBox>(comboBox).Select(itemText);
        }

		public static void SelectListByPreviousControl(this TestBase @this, string controlName, string itemText, AutomationElement labelRetrievedControl = null)
		{
			var label = @this.Gui(controlName, labelRetrievedControl);
			var comboBox = @this.App.FindNextSibling(label);
			ControlProvider.Transfer<AEComboBox>(comboBox).Select(itemText);
		}

        public static void EnterTextByPreviousControl(this TestBase @this, AutomationProperty searchBy, object value, string text, AutomationElement labelRetrievedControl = null)
        {
            var label = @this.App.FindDescendant(searchBy, value, labelRetrievedControl);
            var comboBox = @this.App.FindNextSibling(label);
            ControlProvider.Transfer<AEEdit>(comboBox).Set(text);
        }

		public static void EnterTextByPreviousControl(this TestBase @this, string controlName, string text, AutomationElement labelRetrievedControl = null)
		{
			var label = @this.Gui(controlName, labelRetrievedControl);
			var comboBox = @this.App.FindNextSibling(label);
			ControlProvider.Transfer<AEEdit>(comboBox).Set(text);
		}

        public static void SetCheckBoxByPreviousControl(this TestBase @this, AutomationProperty searchBy, object value, bool checkIn, AutomationElement labelRetrievedControl = null)
        {
            var label = @this.App.FindDescendant(searchBy, value, labelRetrievedControl);
            var checkBox = @this.App.FindNextSibling(label);
            ControlProvider.Transfer<AECheckBox>(checkBox).Check(checkIn);
        }

		public static void SetCheckBoxByPreviousControl(this TestBase @this, string controlName, bool checkIn, AutomationElement labelRetrievedControl = null)
		{
			var label = @this.Gui(controlName, labelRetrievedControl);
			var checkBox = @this.App.FindNextSibling(label);
			ControlProvider.Transfer<AECheckBox>(checkBox).Check(checkIn);
		}

        public static void EnterTextInListByPreviousControl(this TestBase @this, AutomationProperty searchBy, object value, string contentInList, AutomationElement labelRetrievedControl = null)
        {
            var label = @this.App.FindDescendant(searchBy, value, labelRetrievedControl);
            var comboBox = @this.App.FindNextSibling(label);
            var edit = @this.App.FindFirstChild(comboBox);
            ControlProvider.Transfer<AEEdit>(edit).Set(contentInList);
        }

		public static void EnterTextInListByPreviousControl(this TestBase @this, string controlName, string contentInList, AutomationElement labelRetrievedControl = null)
		{
			var label = @this.Gui(controlName, labelRetrievedControl);
			var comboBox = @this.App.FindNextSibling(label);
			var edit = @this.App.FindFirstChild(comboBox);
			ControlProvider.Transfer<AEEdit>(edit).Set(contentInList);
		}

        public static void ClickTreeItem(this TestBase @this, AutomationProperty searchBy, object value, AutomationElement retrievedControl = null)
        {
            var target = @this.App.FindDescendant(searchBy, value, retrievedControl);
            ControlProvider.Transfer<AETreeItem>(target).Select();
        }

		public static void ClickTreeItem(this TestBase @this, AutomationElement target)
		{
			ControlProvider.Transfer<AETreeItem>(target).Select();
		}

		public static void ClickTreeItem(this TestBase @this, string controlName, AutomationElement retrievedControl = null)
		{
			var target = @this.Gui(controlName, retrievedControl);
			ControlProvider.Transfer<AETreeItem>(target).Select();
		}

        public static void ClickTabItem(this TestBase @this, AutomationProperty searchBy, object value, AutomationElement retrievedControl = null)
        {
            Dictionary<AutomationProperty, object> searchCretia = new Dictionary<AutomationProperty, object>();
            searchCretia.Add(SearchBy.ByControlType, ControlType.TabItem);
            searchCretia.Add(searchBy, value);
            var target = @this.App.FindDescendant(searchCretia, retrievedControl);
            ControlProvider.Transfer<AETabItem>(target).Select();
        }

		public static void ClickTabItem(this TestBase @this, string controlName, AutomationElement retrievedControl = null)
		{
			var target = @this.Gui(controlName, retrievedControl);
			ControlProvider.Transfer<AETabItem>(target).Select();
		}

		public static void ExpandMenu(this TestBase @this, string controlName, AutomationElement retrievedControl = null)
		{
			var target = @this.Gui(controlName, retrievedControl);
			ControlProvider.Transfer<AEMenu>(target).Expand();
		}

        public static void ClickButtonByChildElement(this TestBase @this, AutomationProperty searchBy, object value, AutomationElement retrievedControl = null)
        {
            var child = @this.App.FindDescendant(searchBy, value, retrievedControl);
            var target = @this.App.FindParent(child);
            ControlProvider.Transfer<AEButton>(target).Click();
        }

		public static void ClickButtonByChildElement(this TestBase @this, string controlName, AutomationElement retrievedControl = null)
		{
			var child = @this.Gui(controlName, retrievedControl);
			var target = @this.App.FindParent(child);
			ControlProvider.Transfer<AEButton>(target).Click();
		}

        //public static void ClickIconInFrontOfTreeItem(this TestBase @this, AutomationProperty searchBy, object value, AutomationElement retrievedControl = null)
        //{
        //    Dictionary<AutomationProperty, object> searchCretia = new Dictionary<AutomationProperty, object>();
        //    searchCretia.Add(SearchBy.ByControlType, ControlType.Button);
        //    searchCretia.Add(searchBy, value);
        //    ControlProvider.Transfer<AEButton2>(@this.App.FindDescendant(searchCretia, retrievedControl)).Click();
        //}

		public static void VerificationForConditionTrue(this TestBase @this,bool condition, string errorMsg = "", string logInfo = "")
		{
			Assert.IsTrue(condition, errorMsg);
			@this.Logger.LogInfo("\r\n"+logInfo);
		}

    }
}
