using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace UIA.Framework.Core.CustomControl
{
	public class AEComboBox2 : AEControlBase
    {
		private AutomationElement topWindow;
		public AEComboBox2(AutomationElement _topWindow, AutomationElement self)
			: base(_topWindow, self)
		{
			topWindow = _topWindow;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">start from 0</param>
		public void Select(int index)
		{
			ExpandComboBox();
			var selectItems = GetItemsUnderListView(topWindow);
			var selectAction = TryGetPattern(SelectionItemPattern.Pattern, selectItems[index]);
			if (null != selectAction)
			{
				object pattern;
				((SelectionItemPattern)selectAction).Select();
				if (this.Self.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern))
				{
					((ExpandCollapsePattern)pattern).Collapse();
				}
				else if (this.Self.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
				{
					((InvokePattern)pattern).Invoke();
				}
			}
			else
				KeyboardToSelectListItem(selectItems, index);

		}

		public void Select(string text)
		{
			ExpandComboBox();
			var listItems = GetItemsUnderListView(topWindow);
			int count = 0;
			foreach (AutomationElement listItem in listItems)
			{
				var textObj = TreeWalker.RawViewWalker.GetFirstChild(listItem) ?? listItem;
				if (text.Equals(textObj.Current.Name, StringComparison.CurrentCultureIgnoreCase))
				{
					var itemPattern = TryGetPattern(SelectionItemPattern.Pattern, listItem);
					if (null != itemPattern)
					{
						object pattern;
						((SelectionItemPattern)itemPattern).Select();
						if (this.Self.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern))
						{
							((ExpandCollapsePattern)pattern).Collapse();
						}
						else if (this.Self.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
						{
							((InvokePattern)pattern).Invoke();
						}
						break;
					}
					else
					{
						KeyboardToSelectListItem(listItems, count);
					}
				}
				count++;
			}
		}

		private void KeyboardToSelectListItem(IList<AutomationElement> listItems, int targetItemIndex)
		{
			string defaultValue = this.DisplayText;
			if (defaultValue == "")
			{
				SendKeys.SendWait("{DOWN " + targetItemIndex + 1 + "}");
			}
			else
			{
				string[] arrListValue = GetListItemValue(listItems);
				int i = 0;
				for (; i < arrListValue.Count(); i++)
				{
					if (arrListValue[i] == defaultValue)
					{
						break;
					}
				}
				if (i - targetItemIndex < 0)
					SendKeys.SendWait("{DOWN " + (targetItemIndex - i) + "}");
				else
					SendKeys.SendWait("{UP " + (i - targetItemIndex) + "}");
			}
			Thread.Sleep(500);
			SendKeys.SendWait("{ENTER}");
		}

		private string[] GetListItemValue(IList<AutomationElement> listItems)
		{
			string strItems = "";
			foreach (AutomationElement item in listItems)
			{
				var textObj = TreeWalker.RawViewWalker.GetFirstChild(item) ?? item;
				string value = textObj.Current.Name;
				strItems += value + "|";
			}
			return strItems.Substring(0, strItems.Length - 1).Split('|');
		}

		private IList<AutomationElement> GetItemsUnderListView(AutomationElement topWind)
		{
			IList<AutomationElement> children = new List<AutomationElement>();
			var window = TreeWalker.RawViewWalker.GetFirstChild(topWind);
			var paneOrListView = TreeWalker.RawViewWalker.GetFirstChild(window);
			var firstChild = TreeWalker.RawViewWalker.GetFirstChild(paneOrListView);
			children.Add(firstChild);
			var next = TreeWalker.RawViewWalker.GetNextSibling(firstChild);
			while (null != next)
			{
				children.Add(next);
				next = TreeWalker.RawViewWalker.GetNextSibling(next);
			}
			return children;
		}

		public string DisplayText
		{
			get
			{
				ExpandComboBox();
				var listItems = GetItemsUnderListView(topWindow);
				foreach (AutomationElement listItem in listItems)
				{
					var itemPattern = TryGetPattern(SelectionItemPattern.Pattern, listItem);
					if (null != itemPattern)
					{
						if (((SelectionItemPattern)itemPattern).Current.IsSelected)
						{
							var textObj = TreeWalker.RawViewWalker.GetFirstChild(listItem) ?? listItem;
							return textObj.Current.Name;
						}
					}
				}
				return "";
			}
		}

		private void ExpandComboBox()
		{
			object pattern;
			if (this.Self.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern))
			{
				((ExpandCollapsePattern)pattern).Expand();
			}
			else if (this.Self.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
			{
				((InvokePattern)pattern).Invoke();
			}
			else
				throw new NotSupportedException();
			Thread.Sleep(600);
		}


	}
}
