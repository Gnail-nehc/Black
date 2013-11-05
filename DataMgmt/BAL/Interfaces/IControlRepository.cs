using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IControlRepository
    {
        IEnumerable<Control> FindAll(int prjId, int pageIndex, int pageSize,out int recordCount);
        Control GetControlById(int id);
        IEnumerable<Control> QueryControls(int prjId, string controlType, string controlProperty, string propertyValue, string controlName, int pageIndex, int pageSize, out int recordCount);
        bool AddControl(Control control, out string log);
        bool DeleteControl(Control control, out string log);
        bool UpdateControl(Control control, out string log);
        bool IsControlNameUniqueInSameTypeAndProperty(int prjId, string controlType, string controlProperty, string controlName);
		int GetCount(Expression<Func<Control, bool>> filter);
    }
}
