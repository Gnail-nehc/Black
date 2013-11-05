using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IGlobalSettingService : IDefaultService
    {
        IEnumerable<GlobalSetting> FindAll(int pageIndex,int pageSize,out int recordCount);
		IEnumerable<GlobalSetting> QueryGlobalSettings(string name, string value, int pageIndex, int pageSize, out int recordCount);
        bool AddGlobalSetting(GlobalSetting globalSetting, out string log);
        bool DeleteGlobalSetting(int id, out string log);
        bool UpdateGlobalSetting(GlobalSetting globalSetting, out string log);
    }
}
