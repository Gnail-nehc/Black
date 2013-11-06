using UIA.Framework.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace UIA.Application.DevExpressWpfAppDemo
{
    public abstract class TestBase : TestModel
    {
	private static string projectId = ConfigurationManager.AppSettings["DemoProjectID"];
        private DataTable dtControlInfo;

        protected TestBase(string testCaseId)
            : base(projectId, testCaseId)
        {
            dtControlInfo = base.LoadControlsInfo(projectId);
        }

        public AppUnderTest App
        {
            get
            {
                return AppUnderTest.CreateInstance();
            }
        }

        [TestInitialize]
        public override void Init()
        {
		this.App.TimeoutSecond = 35;
		this.App.Close(GlobalData["ProcessName"]);
		this.App.Launch(GlobalData["AppPath"]);
		//this.App.Launch(GlobalData["rppLocal"]);
        }

        [TestCleanup]
        public override void End()
        {
		//MessageBox.Show("done.");
            	//this.App.Close();
        }

        /// <summary>
        /// /Get descendant node by control name record in DB
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="controlRetrieve"></param>
        /// <returns></returns>
	public AutomationElement Gui(string controlName, AutomationElement controlRetrieve = null, bool searchDescendant = true)
        {
            var searchingControl = controlRetrieve ?? this.App.Self;
            try
            {
                IEnumerable<DataRow> query = from row in dtControlInfo.AsEnumerable()
                                             where row.Field<string>(ReturnedControlFuncStruct.UIName) == controlName
                                             select row;
                if (query.Count() != 0)
                {
                    DataTable dt = query.CopyToDataTable<DataRow>();
                    var searchCondition=new Dictionary<AutomationProperty,object>();
					var firstRow = dt.Rows[0];
					string type = firstRow.Field<string>(ReturnedControlFuncStruct.UIType);
					searchCondition.Add(SearchBy.ByControlType, this.GetControlType(type));
                    foreach (DataRow row in dt.Rows)
                    {
                        string property = row.Field<string>(ReturnedControlFuncStruct.UIProperty);
                        string propValue = row.Field<string>(ReturnedControlFuncStruct.PropValue);
                        searchCondition.Add(this.GetAPByStr(property),propValue);
                    }
					var target = searchDescendant ? this.App.FindDescendant(searchCondition, searchingControl) :
						this.App.FindChild(searchCondition, searchingControl);
					if (target != null)
						return target;
					else
					{
						string error = "";
						foreach (KeyValuePair<AutomationProperty, object> item in searchCondition)
						{
							error += "Property: " + item.Key + " Value: " + item.Value + "\r\n";
						}
						throw new ControlNotFoundException(error);
					}
                }
                else
                {
                    throw new LoadControlFailException(controlName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        private AutomationProperty GetAPByStr(string property)
        {
            AutomationProperty ap;
            switch (property)
            {
            	case "Name":
			ap = AutomationElement.NameProperty; break;
		case "AutomationId":
			ap = AutomationElement.AutomationIdProperty; break;
		case "ControlType":
			ap = AutomationElement.ControlTypeProperty; break;
		case "ClassName":
			ap = AutomationElement.ClassNameProperty; break;
		case "HelpText":
			ap = AutomationElement.HelpTextProperty; break;
		case "AcceleratorKey":
			ap = AutomationElement.AcceleratorKeyProperty; break;
		case "AccessKey":
			ap = AutomationElement.AccessKeyProperty; break;
		case "BoundingRectangle":
			ap = AutomationElement.BoundingRectangleProperty; break;
		case "ClickablePoint":
			ap = AutomationElement.ClickablePointProperty; break;
		case "Culture":
			ap = AutomationElement.CultureProperty; break;
		case "FrameworkId":
			ap = AutomationElement.FrameworkIdProperty; break;
		case "HasKeyboardFocus":
			ap = AutomationElement.HasKeyboardFocusProperty; break;
		case "IsContentElement":
			ap = AutomationElement.IsContentElementProperty; break;
		case "IsControlElement":
			ap = AutomationElement.IsControlElementProperty; break;
		case "IsDockPatternAvailable":
			ap = AutomationElement.IsDockPatternAvailableProperty; break;
		case "IsEnabled":
			ap = AutomationElement.IsEnabledProperty; break;
		case "IsExpandCollapsePatternAvailable":
			ap = AutomationElement.IsExpandCollapsePatternAvailableProperty; break;
		case "IsGridItemPatternAvailable":
			ap = AutomationElement.IsGridItemPatternAvailableProperty; break;
		case "IsGridPatternAvailable":
			ap = AutomationElement.IsGridPatternAvailableProperty; break;
		case "IsInvokePatternAvailable":
			ap = AutomationElement.IsInvokePatternAvailableProperty; break;
		case "IsItemContainerPatternAvailable":
			ap = AutomationElement.IsItemContainerPatternAvailableProperty; break;
		case "IsKeyboardFocusable":
			ap = AutomationElement.IsKeyboardFocusableProperty; break;
		case "IsMultipleViewPatternAvailable":
			ap = AutomationElement.IsMultipleViewPatternAvailableProperty; break;
		case "IsOffscreen":
			ap = AutomationElement.IsOffscreenProperty; break;
		case "IsPassword":
			ap = AutomationElement.IsPasswordProperty; break;
		case "IsRangeValuePatternAvailable":
			ap = AutomationElement.IsRangeValuePatternAvailableProperty; break;
		case "IsRequiredForForm":
			ap = AutomationElement.IsRequiredForFormProperty; break;
		case "IsScrollItemPatternAvailable":
			ap = AutomationElement.IsScrollItemPatternAvailableProperty; break;
		case "IsScrollPatternAvailable":
			ap = AutomationElement.IsScrollPatternAvailableProperty; break;
		case "IsSelectionItemPatternAvailable":
			ap = AutomationElement.IsSelectionItemPatternAvailableProperty; break;
		case "IsSelectionPatternAvailable":
			ap = AutomationElement.IsSelectionPatternAvailableProperty; break;
		case "IsSynchronizedInputPatternAvailable":
			ap = AutomationElement.IsSynchronizedInputPatternAvailableProperty; break;
		case "IsTableItemPatternAvailable":
			ap = AutomationElement.IsTableItemPatternAvailableProperty; break;
		case "IsTablePatternAvailable":
			ap = AutomationElement.IsTablePatternAvailableProperty; break;
		case "IsTextPatternAvailable":
			ap = AutomationElement.IsTextPatternAvailableProperty; break;
		case "IsTogglePatternAvailable":
			ap = AutomationElement.IsTogglePatternAvailableProperty; break;
		case "IsTransformPatternAvailable":
			ap = AutomationElement.IsTransformPatternAvailableProperty; break;
		case "IsValuePatternAvailable":
			ap = AutomationElement.IsValuePatternAvailableProperty; break;
		case "IsVirtualizedItemPatternAvailable":
			ap = AutomationElement.IsVirtualizedItemPatternAvailableProperty; break;
		case "IsWindowPatternAvailable":
			ap = AutomationElement.IsWindowPatternAvailableProperty; break;
		case "ItemStatus":
			ap = AutomationElement.ItemStatusProperty; break;
		case "ItemType":
			ap = AutomationElement.ItemTypeProperty; break;
		case "LabeledBy":
			ap = AutomationElement.LabeledByProperty; break;
		case "LocalizedControlType":
			ap = AutomationElement.LocalizedControlTypeProperty; break;
		case "NativeWindowHandle":
			ap = AutomationElement.NativeWindowHandleProperty; break;
		case "Orientation":
			ap = AutomationElement.OrientationProperty; break;
		case "ProcessId":
			ap = AutomationElement.ProcessIdProperty; break;
                default:
                    ap = AutomationElement.RuntimeIdProperty; break;
            }
            return ap;
        }

	private ControlType GetControlType(string strType)
        {
		ControlType type;
		switch (strType)
            	{
			case "Text":
				type = ControlType.Text; break;
                	case "Button":
                		type = ControlType.Button;break;
			case "ComboBox":
				type = ControlType.ComboBox; break;
			case "Edit":
				type = ControlType.Edit; break;
                	case "CheckBox":
                    		type = ControlType.CheckBox; break;
			case "TabItem":
				type = ControlType.TabItem; break;
			case "Tab":
				type = ControlType.Tab; break;
			case "TreeItem":
				type = ControlType.TreeItem; break;
			case "Tree":
				type = ControlType.Tree; break;
			case "Window":
				type = ControlType.Window; break;
			case "ListItem":
				type = ControlType.ListItem; break;
			case "List":
				type = ControlType.List; break;
			case "Calendar":
				type = ControlType.Calendar; break;
	                case "Custom":
	                    type = ControlType.Custom; break;
	                case "DataGrid":
	                    type = ControlType.DataGrid; break;
	                case "DataItem":
	                    type = ControlType.DataItem; break;
	                case "Document":
	                    type = ControlType.Document; break;
	                case "Group":
	                    type = ControlType.Group; break;
	                case "Header":
	                    type = ControlType.Header; break;
	                case "HeaderItem":
	                    type = ControlType.HeaderItem; break;
	                case "Hyperlink":
	                    type = ControlType.Hyperlink; break;
	                case "Image":
	                    type = ControlType.Image; break;
	                case "Menu":
	                    type = ControlType.Menu; break;
	                case "MenuBar":
	                    type = ControlType.MenuBar; break;
	                case "MenuItem":
	                    type = ControlType.MenuItem; break;
	                case "Pane":
	                    type = ControlType.Pane; break;
	                case "ProgressBar":
	                    type = ControlType.ProgressBar; break;
	                case "RadioButton":
	                    type = ControlType.RadioButton; break;
	                case "ScrollBar":
	                    type = ControlType.ScrollBar; break;
	                case "Separator":
	                    type = ControlType.Separator; break;
	                case "Slider":
	                    type = ControlType.Slider; break;
	                case "Spinner":
	                    type = ControlType.Spinner; break;
	                case "SplitButton":
	                    type = ControlType.SplitButton; break;
	                case "StatusBar":
	                    type = ControlType.StatusBar; break;
	                case "Table":
	                    type = ControlType.Table; break;
	                case "Thumb":
	                    type = ControlType.Thumb; break;
	                case "TitleBar":
	                    type = ControlType.TitleBar; break;
	                case "ToolTip":
	                    type = ControlType.ToolTip; break;
	                default:
	                    type = ControlType.ToolBar;break;
            	}
			return type;
        }
    }

    public class LoadControlFailException : Exception
    {
        public LoadControlFailException(string controlName)
            : base(string.Format("Fail to load control! Name: [{0}].Please check DB first.", controlName))
        {

        }
    }

    
}
