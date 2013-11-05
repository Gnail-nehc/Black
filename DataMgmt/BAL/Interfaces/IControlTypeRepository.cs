using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IControlTypeRepository
    {
        IEnumerable<string> GetControlTypesByPlatform(string platform);
        IEnumerable<ControlType> GetControlTypes(string platform);
    }
}
