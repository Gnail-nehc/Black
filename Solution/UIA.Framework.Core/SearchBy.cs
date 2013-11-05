using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core
{
    public struct SearchBy
    {
        public static readonly AutomationProperty ByAutomationId = AutomationElement.AutomationIdProperty;
        public static readonly AutomationProperty ByName = AutomationElement.NameProperty;
        public static readonly AutomationProperty ByControlType = AutomationElement.ControlTypeProperty;
        public static readonly AutomationProperty ByClassName = AutomationElement.ClassNameProperty;
        public static readonly AutomationProperty ByProcessId = AutomationElement.ProcessIdProperty;
        public static readonly AutomationProperty ByItemType = AutomationElement.ItemTypeProperty;
        public static readonly AutomationProperty ByItemStatus = AutomationElement.ItemStatusProperty;
        public static readonly AutomationProperty ByIsPassword = AutomationElement.IsPasswordProperty;
        public static readonly AutomationProperty ByIsEnabled = AutomationElement.IsEnabledProperty;
        public static readonly AutomationProperty ByRuntimeId = AutomationElement.RuntimeIdProperty;
    }
}
