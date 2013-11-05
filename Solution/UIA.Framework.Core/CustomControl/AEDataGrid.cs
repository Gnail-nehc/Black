using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIA.Framework.Core.CustomControl
{
	public class AEDataGrid : AEControlBase
    {

		public AEDataGrid(AutomationElement ae)
            : base(ae)
        {
        }

		public IList<AEHeaderItem> Columns
		{
			get
			{ 
				var collection = this.Self.FindAll(TreeScope.Descendants,new PropertyCondition(SearchBy.ByControlType,ControlType.HeaderItem));
				if (null == collection)
					return null;
				IList<AEHeaderItem> returnList = new List<AEHeaderItem>();
				foreach(AutomationElement elem in collection)
				{
					var item = ControlProvider.Transfer<AEHeaderItem>(elem);
					returnList.Add(item);
				}
				return returnList;
			}
		}

		public IList<AEDataItem> DataRows
		{
			get
			{
				var collection = this.Self.FindAll(TreeScope.Descendants, new PropertyCondition(SearchBy.ByControlType, ControlType.DataItem));
				if (null == collection)
					return null;
				IList<AEDataItem> returnList = new List<AEDataItem>();
				foreach (AutomationElement elem in collection)
				{
					var item = ControlProvider.Transfer<AEDataItem>(elem);
					returnList.Add(item);
				}
				return returnList;
			}
		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">start from 0</param>
		public void ClickDataItemByIndex(int index)
		{
			DataRows.ElementAt(index).Click();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="rowIndex">start from 0</param>
		/// <returns></returns>
		public string GetContentByColumnNameAndRowIndex(string columnName, int rowIndex)
		{
			if(rowIndex>=DataRows.Count)
			{
				Debug.WriteLine("The rowIndex is out of scope!");
			}
			for (int i = 0; i < Columns.Count; i++)
			{
				string name = Columns.ElementAt(i).Name;
				if (name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
				{
					return DataRows.ElementAt(rowIndex).Cells.ElementAt(i).Text;
				}
			}
			return "";
		}

		public AEDataItem GetDataItemByCellValue(string columnName, string cellValue)
		{
			int columnIndex = -1;
			foreach (var column in Columns)
			{
				columnIndex++;
				if (columnName.Equals(column.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
					break;
			}
			if (columnIndex != -1)
			{
				foreach (var item in DataRows)
				{
					if (cellValue.Equals(item.Cells[columnIndex].Text.Trim(), StringComparison.CurrentCultureIgnoreCase))
						return item;
				}
			}
			return null;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="rowIndex">start from 0</param>
		/// <returns></returns>
		public void SetValueInCellByColumnNameAndRowIndex(string columnName, int rowIndex,string newContent)
		{
			if (rowIndex >= DataRows.Count)
			{
				Debug.WriteLine("The rowIndex is out of scope!");
			}
			for (int i = 0; i < Columns.Count; i++)
			{
				string name = Columns.ElementAt(i).Name;
				if (name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
				{
					var target = DataRows.ElementAt(rowIndex).Cells.ElementAt(i);
					if (target.CanSetValue)
					{
						target.Set(newContent);
					}
				}
			}
		}
	}
}
