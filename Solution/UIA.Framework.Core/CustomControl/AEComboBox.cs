using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace UIA.Framework.Core.CustomControl
{
    public class AEComboBox : AEControlBase
    {
        public AEComboBox(AutomationElement ae)
            : base(ae)
        {
        }

        /// <param name="index">the first index of items is 0</param>
        public void Select(int index)
        {
			ExpandComboBox();
            var combo = this.Self;
            var selectItems = combo.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
			var selectAction = TryGetPattern(SelectionItemPattern.Pattern, selectItems[index]);
			if (null != selectAction)
			{
				((SelectionItemPattern)selectAction).Select();
				object pattern;
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
			var listItems = this.Self.FindAll(TreeScope.Children, new PropertyCondition(SearchBy.ByControlType, ControlType.ListItem));
			int count = 0;
			foreach (AutomationElement listItem in listItems)
			{
				var textObj = TreeWalker.RawViewWalker.GetFirstChild(listItem) ?? listItem;
				if (text.Equals(textObj.Current.Name, StringComparison.CurrentCultureIgnoreCase))
				{
					var itemPattern = TryGetPattern(SelectionItemPattern.Pattern, listItem);
					if (null != itemPattern)
					{
						((SelectionItemPattern)itemPattern).Select();
						object pattern=null;
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
						KeyboardToSelectListItem(listItems,count);
					}
				}
				count++;
			}
        }

		private void KeyboardToSelectListItem(AutomationElementCollection listItems, int targetItemIndex)
		{
			string defaultValue = this.DisplayText;
			if (defaultValue == "")
			{
				SendKeys.SendWait("{DOWN " + targetItemIndex+1 + "}");
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

		private string[] GetListItemValue(AutomationElementCollection listItems)
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
			Thread.Sleep(500);
		}

		public string DisplayText
		{
			get
			{
				var listItems = this.Self.FindAll(TreeScope.Children,new PropertyCondition(SearchBy.ByControlType,ControlType.ListItem));
				foreach(AutomationElement listItem in listItems)
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
    }
}
