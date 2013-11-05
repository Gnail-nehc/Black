using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Entities;
using DAL.Repository;


namespace BAL.Repositories
{
    public class ControlRepository : RepositoryBase<Control>, IControlRepository
    {
        public ControlRepository(QADB_Entities dbContext)
            : base(dbContext)
        { 
        }

        public IEnumerable<Control> FindAll(int prjId, int pageIndex, int pageSize, out int recordCount)
        {
            IEnumerable<Control> controls = this.Get<int>(c => c.ProjectId == prjId, pageIndex, pageSize, c => c.Id, true);
			recordCount = this.GetCount(c => c.ProjectId == prjId);
            return controls;
        }

        public Control GetControlById(int id)
        {
            IEnumerable<Control> controls = this.Get(c => c.Id == id);
            if (controls.Count() == 1)
                return this.Get(c => c.Id == id).FirstOrDefault<Control>();
            else
                return null;
        }

        public IEnumerable<Control> QueryControls(int prjId, string controlType, string controlProperty, string propertyValue, string controlName, int pageIndex, int pageSize,out int recordCount)
        {
			propertyValue = propertyValue.Trim();
			controlName = controlName.Trim();
			Expression<Func<Control, bool>> expression = c =>
				controlType != "" ? c.ControlProperty.Type == controlType : true &&
				controlProperty != "" ? c.ControlProperty.Property == controlProperty : true &&
				propertyValue != "" ? c.PropertyValue.Contains(propertyValue) : true &&
				controlName != "" ? c.Name.Contains(controlName) : true &&
				c.ProjectId == prjId;
			IEnumerable<Control> controls = this.Get(expression, pageIndex, pageSize, c => c.Id, true);
			recordCount = this.Count(expression);
            return controls;
        }

        public bool AddControl(Control control,out string log)
        {
            try
            {
                this.Add(control);
                log = "done.";
                return true;
            }
            catch(Exception e)
            {
                log = "Add control failed." + e.Message;
                return false;
            }
        }

        public bool DeleteControl(Control control, out string log)
        {
            try
            {
                this.Delete(control);
                log = "done.";
                return true;
            }
            catch (Exception e)
            {
                log = "Delete control failed." + e.Message;
                return false;
            }
        }

        public bool UpdateControl(Control control, out string log)
        {
            try
            {
                this.Update(control);
                log = "done.";
                return true;
            }
            catch (Exception e)
            {
                log = "Update control failed." + e.Message;
                return false;
            }
        }

        public bool IsControlNameUniqueInSameTypeAndProperty(int prjId, string controlType, string controlProperty, string controlName)
        {
			int count = base.Count(c => c.ProjectId == prjId && c.ControlProperty.Type == controlType && c.ControlProperty.Property == controlProperty && c.Name == controlName);
            return count == 0 ? true : false;
        }

        public int GetCount(Expression<Func<Control, bool>> filter)
        {
            return base.Count(filter);
        }

    }
}
