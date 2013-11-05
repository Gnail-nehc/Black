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
    public class GlobalSettingService : ServiceBase, IGlobalSettingService
    {
        public IGlobalSettingRepository GlobalSettingRepository { get; private set; }
        public GlobalSettingService(IGlobalSettingRepository globalSettingRepository)
        {
            this.GlobalSettingRepository = globalSettingRepository;
            this.AddDisposableObject(globalSettingRepository);
        }

        public IEnumerable<GlobalSetting> FindAll(int pageIndex, int pageSize, out int recordCount)
        {
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.GlobalSettingRepository.FindAll(pid, pageIndex, pageSize, out recordCount);
        }

        public IEnumerable<GlobalSetting> QueryGlobalSettings(string name, string value,int pageIndex,int pageSize, out int recordCount)
        {
            if (null == name) name = "";
            if (null == value) value = "";
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.GlobalSettingRepository.QueryGlobalSettings(pid, name, value, pageIndex, pageSize, out recordCount);
        }

        public bool AddGlobalSetting(GlobalSetting globalSetting, out string log)
        {
            Guard.ArgumentNotNull(globalSetting, "globalSettingObject");
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            globalSetting.ProjectId = pid;
            return this.GlobalSettingRepository.AddGlobalSetting(globalSetting, out log);
        }

        public bool DeleteGlobalSetting(int id,out string log)
        {
            return this.GlobalSettingRepository.DeleteGlobalSetting(id, out log);
        }

        public bool UpdateGlobalSetting(GlobalSetting globalSetting, out string log)
        {
            Guard.ArgumentNotNull(globalSetting, "globalSettingObject");
            globalSetting.ProjectId = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.GlobalSettingRepository.UpdateGlobalSetting(globalSetting, out log);
        }

        public int CurrentProjectId
        {
            get;
            set;
        }
    }
}
