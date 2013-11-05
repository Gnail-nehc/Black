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
    public class ControlTypeRepository : RepositoryBase<ControlType>, IControlTypeRepository
    {
        public ControlTypeRepository(QADB_Entities dbContext)
            : base(dbContext)
        { 
        }

        public IEnumerable<string> GetControlTypesByPlatform(string platform)
        {
            Guard.ArgumentNotNullOrEmpty(platform, "Tech Platform");
            IEnumerable<ControlType> cts = this.Get(ct => ct.Platform == platform);
            return cts.SelectMany<ControlType, string>(ct => new List<string> { ct.Type });
        }

        public IEnumerable<ControlType> GetControlTypes(string platform)
        {
            Guard.ArgumentNotNullOrEmpty(platform, "Tech Platform");
            return this.Get(ct => ct.Platform == platform);
        }
    }
}
