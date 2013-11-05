using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Entities;
using DAL.Repository;

namespace BAL.Services
{
    public class ControlService : ServiceBase,IControlService
    {
        public IControlRepository ControlRepository { get; private set; }
        public IControlTypeRepository ControlTypeRepository { get; private set; }
        public IControlPropertyRepository ControlPropertyRepository { get; private set; }
        public ControlService(IControlRepository controlRepository, 
                              IControlTypeRepository controlTypeRepository,
                              IControlPropertyRepository controlPropertyRepository)
        {
            this.ControlRepository = controlRepository;
            this.ControlTypeRepository = controlTypeRepository;
            this.ControlPropertyRepository = controlPropertyRepository;
            this.AddDisposableObject(controlRepository);
            this.AddDisposableObject(ControlTypeRepository);
            this.AddDisposableObject(ControlPropertyRepository);
        }

        public string Platform { get; set; }

        public IEnumerable<Control> FindAll(int pageIndex, int pageSize, out int recordCount)
        {
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.ControlRepository.FindAll(pid, pageIndex, pageSize, out recordCount);
        }

        public IEnumerable<Control> QueryControls(string controlType, string controlProperty, string propertyValue, string controlName, int pageIndex, int pageSize, out int recordCount)
        {
            if (null == controlType) controlType = "";
            if (null == controlProperty) controlProperty = "";
            if (null == propertyValue) propertyValue = "";
            if (null == controlName) controlName = "";
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.ControlRepository.QueryControls(pid, controlType, controlProperty, propertyValue, controlName,pageIndex,pageSize,out recordCount);
        }

        public IEnumerable<string> GetControlTypes()
        {
            string platform = (null != Platform && Platform != "") ? Platform : "Web";
            return this.ControlTypeRepository.GetControlTypesByPlatform(platform);
        }

        public IEnumerable<ControlType> GetControlTypeEntities()
        {
            string platform = (null != Platform && Platform != "") ? Platform : "Web";
            return this.ControlTypeRepository.GetControlTypes(platform);
        }

        public IEnumerable<string> GetControlPropertiesAgainstControlType(string type)
        {
            return this.ControlPropertyRepository.GetPropertiesByType(type);
        }

        public bool AddControl(string controlType, string controlProperty, string propertyValue, string controlName, out string log)
        {
            Guard.ArgumentNotNullOrEmpty(controlType, "controlType");
            Guard.ArgumentNotNullOrEmpty(controlProperty, "controlProperty");
            if (null == propertyValue) propertyValue = "";
            Guard.ArgumentNotNullOrEmpty(controlName, "controlName");
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            bool isUnique = this.ControlRepository.IsControlNameUniqueInSameTypeAndProperty(pid, controlType, controlProperty, controlName);
            if (isUnique)
            {
                log = "done.";
                int propId = this.ControlPropertyRepository.QueryPropertyId(controlType,controlProperty);
                Control instance = new Control();
                instance.ProjectId = pid;
                instance.PropertyId = propId;
                instance.PropertyValue = propertyValue;
                instance.Name = controlName;
                return this.ControlRepository.AddControl(instance,out log);
            }
            else
            {
                log = "Fail to Add.The control name should be unique for " + controlType + " in Project " + pid.ToString();
            }
            return isUnique ;
        }

        public bool DeleteControl(int id, out string log)
        {
            Control instance = this.ControlRepository.GetControlById(id);
            if (null != instance)
            {
                return this.ControlRepository.DeleteControl(instance, out log);
            }
            else
            {
                log = "Not found control.";
                return false;
            }
        }

        public bool UpdateControl(int id, string controlType, string controlProperty, string newPropertyValue, string newName, out string log)
        {
            Guard.ArgumentNotNullOrEmpty(controlType, "controlType");
            Guard.ArgumentNotNullOrEmpty(controlProperty, "controlProperty");
            if (null == newPropertyValue) newPropertyValue = "";
            Guard.ArgumentNotNullOrEmpty(newName, "controlName");
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
			string property = this.GetControlPropertyById(id);
			string controlName = this.ControlRepository.GetControlById(id).Name;
			bool isUnique = true;
			bool ifChanged = (property.Equals(controlProperty) && controlName.Equals(newName)) ? false : true;
			if (ifChanged)
			{ 
				isUnique = this.ControlRepository.IsControlNameUniqueInSameTypeAndProperty(pid, controlType, controlProperty, newName);
			}
			if (isUnique)
            {
                int propId = this.ControlPropertyRepository.QueryPropertyId(controlType, controlProperty);
                Control instance = new Control() { Id = id, ProjectId = pid, PropertyId = propId, PropertyValue = newPropertyValue, Name = newName }; 
                this.ControlRepository.UpdateControl(instance,out log);
            }
            else
            {
                log = "Fail to Update.The control name should be unique for " + controlType + " in Project " + pid.ToString();
            }
			return isUnique;

        }

        public string GetControlPropertyById(int cid)
        {
            return this.ControlRepository.GetControlById(cid).ControlProperty.Property;
        }

        public bool GetControlTypeAndPropertyById(int cid, out string controlType, out string controlProperty)
        {
            Control instance = this.ControlRepository.GetControlById(cid);
            if (null != instance)
            {
                controlType = instance.ControlProperty.Type;
                controlProperty = instance.ControlProperty.Property;
                return true;
            }
            else
            {
                controlType = "";
                controlProperty = "";
                return false;
            }
        }

        public int CurrentProjectId { get; set; }
        
    }
}
