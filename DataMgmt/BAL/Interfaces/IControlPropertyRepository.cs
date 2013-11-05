using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IControlPropertyRepository
    {
        int QueryPropertyId(string type, string property);
        string GetSpecificControlType(int propertyId);
        string GetSpecificControlProperty(int propertyId);
        IEnumerable<string> GetPropertiesByType(string type);
    }
}
