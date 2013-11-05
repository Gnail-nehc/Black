using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core
{
    public interface ISearchCriteria
    {
        AutomationElementCollection FindChildren(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null);

        AutomationElementCollection FindChildren(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null);

        AutomationElement FindChild(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null);

        AutomationElement FindChild(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null);
        
        AutomationElementCollection FindDescendants(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null);

        AutomationElementCollection FindDescendants(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null);

        AutomationElement FindDescendant(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null);

        AutomationElement FindDescendant(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null);

		AutomationElement FindParent(AutomationElement retrieveControl = null);

		AutomationElement FindNextSibling(AutomationElement retrieveControl = null);

		AutomationElement FindPreviousSibling(AutomationElement retrieveControl = null);

		AutomationElement FindLastChild(AutomationElement retrieveControl = null);

		AutomationElement FindFirstChild(AutomationElement retrieveControl = null);
    }
}
