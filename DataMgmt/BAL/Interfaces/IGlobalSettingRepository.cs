using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IGlobalSettingRepository
    {
        IEnumerable<GlobalSetting> FindAll(int prjId,int pageIndex,int pageSize,out int recordCount);
		IEnumerable<GlobalSetting> QueryGlobalSettings(int prjId, string name, string value, int pageIndex, int pageSize, out int recordCount);
        bool AddGlobalSetting(GlobalSetting globalSetting, out string log);
        bool DeleteGlobalSetting(int id, out string log);
        bool UpdateGlobalSetting(GlobalSetting globalSetting, out string log);
        int GetCount(Expression<Func<GlobalSetting, bool>> filter);
    }
}
