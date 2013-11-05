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
    public class GlobalSettingRepository : RepositoryBase<GlobalSetting>,IGlobalSettingRepository
    {
        public GlobalSettingRepository(QADB_Entities dbContext)
            : base(dbContext)
        { 
        }

        public IEnumerable<GlobalSetting> FindAll(int prjId, int pageIndex, int pageSize, out int recordCount)
        {
            IEnumerable < GlobalSetting > globalSettings= this.Get<int>(gs => gs.ProjectId == prjId, pageIndex, pageSize, gs => gs.Id, true);
			recordCount = this.GetCount(c => c.ProjectId == prjId);
            return globalSettings;
        }

		public IEnumerable<GlobalSetting> QueryGlobalSettings(int prjId, string name, string value, int pageIndex, int pageSize, out int recordCount)
        {
			name = name.Trim();
			value = value.Trim();
			Expression<Func<GlobalSetting, bool>> expression;
			if ("" == name && "" == value)
				expression = gs => gs.ProjectId == prjId;
			else if("" != name && "" == value)
				expression = gs => gs.ProjectId == prjId && gs.Name.Contains(name);
			else if("" == name && "" != value)
				expression = gs => gs.ProjectId == prjId && gs.Value.Contains(value);
			else
				expression = gs => gs.ProjectId == prjId && gs.Name.Contains(name) && gs.Value.Contains(value);
			IEnumerable<GlobalSetting> globalSettings = this.Get(expression, pageIndex, pageSize, gs => gs.Id, true);
			recordCount = this.Count(expression);
            return globalSettings;
        }

        public bool AddGlobalSetting(GlobalSetting globalSetting, out string log)
        {
            if (globalSetting.Name == "")
            {
                log = "Global Setting name shouldn't be empty.";
                return false;
            }
            bool isUnique = IsNameUniqueInProject(globalSetting.ProjectId, globalSetting.Name);
            if (isUnique)
            {
                this.Add(globalSetting);
                log = "done.";
            }
            else
            {
                log = "Fail to Add.The name should be unique in Project " + globalSetting.ProjectId;
            }
            return isUnique;
        }

        public bool DeleteGlobalSetting(int id, out string log)
        {
            try
            {
                GlobalSetting globalSetting = this.Get(gs => gs.Id == id).FirstOrDefault<GlobalSetting>();
                this.Delete(globalSetting);
                log = "done.";
                return true;
            }
            catch(Exception e)
            {
                log = "Fail to delete."+e.Message;
                return false;
            }
        }

		private GlobalSetting FindById(int id)
		{
			var target = this.Get(gs => gs.Id == id).FirstOrDefault<GlobalSetting>();
			return target;
		}

        public bool UpdateGlobalSetting(GlobalSetting globalSetting, out string log)
        {
            if (globalSetting.Name == "")
            {
                log = "Global Setting name shouldn't be empty.";
                return false;
            }
			GlobalSetting gs = this.FindById(globalSetting.Id);
			bool ifNameChanged = (!gs.Name.Equals(globalSetting.Name)) ? true : false;
			bool isUnique = true;
			if (ifNameChanged)
			{
				isUnique = IsNameUniqueInProject(globalSetting.ProjectId, globalSetting.Name);
			}
			if (isUnique)
            {
                this.Update(globalSetting);
                log = "done.";
            }
            else
            {
                log = "Fail to Update.The name should be unique in Project " + globalSetting.ProjectId;
            }
			return isUnique;
        }

        public int GetCount(Expression<Func<GlobalSetting, bool>> filter)
        {
            return base.Count(filter);
        }

        private bool IsNameUniqueInProject(int prjId,string name)
        {
            int count = base.Count(gs => gs.ProjectId == prjId && gs.Name == name);
            return count == 0 ? true : false;
        }

    }
}
