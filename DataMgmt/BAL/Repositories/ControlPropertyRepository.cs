using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Entities;
using DAL.Repository;

namespace BAL.Repositories
{
    public class ControlPropertyRepository : RepositoryBase<ControlProperty>, IControlPropertyRepository
    {
        public ControlPropertyRepository(QADB_Entities dbContext)
            : base(dbContext)
        {
        }

        public int QueryPropertyId(string type, string property)
        {
            Guard.ArgumentNotNullOrEmpty(type, "controltype");
            Guard.ArgumentNotNullOrEmpty(property, "controlProperty");
            return this.Get(cp => cp.Type == type && cp.Property == property).FirstOrDefault<ControlProperty>().Id;
        }

        public string GetSpecificControlType(int propertyId)
        {
            Guard.ArgumentMustMoreThanZero(propertyId, "propertyId");
            return this.Get(cp => cp.Id == propertyId).FirstOrDefault<ControlProperty>().Type;
        }

        public string GetSpecificControlProperty(int propertyId)
        {
            Guard.ArgumentMustMoreThanZero(propertyId, "propertyId");
            return this.Get(cp => cp.Id == propertyId).FirstOrDefault<ControlProperty>().Property;
        }

        public IEnumerable<string> GetPropertiesByType(string type)
        {
            Guard.ArgumentNotNull(type, "controltype");
            IEnumerable<ControlProperty> cps=this.Get(cp => cp.Type == type);
            return cps.SelectMany<ControlProperty, string>(cp => new List<string> { cp.Property });
        }
    }   
}
