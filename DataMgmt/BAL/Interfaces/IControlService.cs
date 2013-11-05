using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IControlService : IDefaultService
    {
        string Platform { get; set; }
        IEnumerable<Control> FindAll(int pageIndex, int pageSize, out int recordCount);
        IEnumerable<Control> QueryControls(string controlType, string controlProperty, string propertyValue, string controlName, int pageIndex, int pageSize, out int recordCount);
        IEnumerable<string> GetControlTypes();
        IEnumerable<ControlType> GetControlTypeEntities();
        IEnumerable<string> GetControlPropertiesAgainstControlType(string type);
        bool AddControl(string controlType, string controlProperty, string propertyValue, string name, out string log);
        bool DeleteControl(int id, out string log);
        bool UpdateControl(int id, string controlType, string controlProperty, string newPropertyValue, string newName, out string log);
        bool GetControlTypeAndPropertyById(int cid, out string controlType, out string controlProperty);
        string GetControlPropertyById(int cid);
    }
}
