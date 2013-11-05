using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
    public class AEList : AEControlBase
    {
        public AEList(AutomationElement ae)
            : base(ae)
        {
        }

        /// <param name="index">the first index of items is 0</param>
        public void Select(int index)
        {
            var listview = this.Self;
            var selectItems = listview.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
			var selectAction = TryGetPattern(SelectionItemPattern.Pattern, selectItems[index]);
			if (null != selectAction)
			{
				((SelectionItemPattern)selectAction).Select();
			}
			else
				throw new NotSupportedException();
        }

        public void Select(string text)
        {
            var listview = this.Self;
            var selectItems = listview.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            foreach (AutomationElement item in selectItems)
            {
                string itemText = item.Current.Name;
				if (text.Equals(itemText, StringComparison.CurrentCultureIgnoreCase))
				{
					var itemPattern = TryGetPattern(SelectionItemPattern.Pattern, item);
					if (null != itemPattern)
					{
						((SelectionItemPattern)itemPattern).Select();
						break;
					}
					else
						throw new NotSupportedException();
				}
            }
        }

		public void SelectByChildText(string text)
		{
			var listview = this.Self;
			var selectItems = listview.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
			foreach (AutomationElement item in selectItems)
			{
				var childText = TreeWalker.RawViewWalker.GetFirstChild(item);
				string itemText = childText.Current.Name;
				if (text.Equals(itemText, StringComparison.CurrentCultureIgnoreCase))
				{
					var itemPattern = TryGetPattern(SelectionItemPattern.Pattern, item);
					if (null != itemPattern)
					{
						((SelectionItemPattern)itemPattern).Select();
						break;
					}
					else
						throw new NotSupportedException();
				}
			}
		}

        public string SelectedText
        {
            get
            {
				var selectionAction = TryGetPattern(SelectionPattern.Pattern);
				if (null != selectionAction)
				{
					var selection = ((SelectionPattern)selectionAction).Current.GetSelection();
					return (selection.Count() > 0) ? selection.FirstOrDefault().Current.Name : "";
				}
				else
					throw new NotSupportedException();
            }
        }
    }
}
