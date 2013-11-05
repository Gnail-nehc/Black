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
    public class ProjectRepository : RepositoryBase<Project>,IProjectRepository
    {
        public ProjectRepository(QADB_Entities dbContext):base(dbContext)
        { 
        }

        public IEnumerable<Project> FindAll()
        {
            return this.Get();
        }

        public bool SelectProject(int prjId, out string prjName, out string platform)
        {
            Guard.ArgumentMustMoreThanZero(prjId, "projectId");
            prjName = string.Empty;
            platform = string.Empty;
            int recordCount = this.Count(prj => prj.Id == prjId);
            if (recordCount > 0)
            {
                var prjInfo = this.Get(prj => prj.Id == prjId).FirstOrDefault();
                prjName = prjInfo.ProjectName;
                platform = prjInfo.Platform;
                return true;
            }
            return false;
        }

        public bool AddProject(Project project, out string log)
        {
            if (project.ProjectName == "")
            {
                log = "project name shouldn't be empty.";
                return false;
            }
            bool isUnique = IsProjectNameUnique(project.ProjectName);
            if (isUnique)
            {
                this.Add(project);
                log = "done.";
            }
            else
            {
                log = "Fail to Add.The name should be unique.";
            }
            return isUnique;
        }

        public bool DeleteProject(int id, out string log)
        {
            try
            {
                Project project = this.Get(p => p.Id == id).FirstOrDefault<Project>();
                this.Delete(project);
                log = "done.";
                return true;
            }
            catch (Exception e)
            {
                log = "Fail to delete." + e.Message;
                return false;
            }
        }

        public bool UpdateProject(Project project, out string log)
        {
            if (project.ProjectName == "")
            {
                log = "project name shouldn't be empty.";
                return false;
            }
            bool isUnique = IsProjectNameUnique(project.ProjectName);
            if (isUnique)
            {
                this.Update(project);
                log = "done.";
            }
            else
            {
                log = "Fail to update.The name should be unique.";
            }
            return isUnique;
        }

        public int GetCount(System.Linq.Expressions.Expression<Func<Project, bool>> filter)
        {
            return base.Count(filter);
        }

        private bool IsProjectNameUnique(string prjName)
        {
            int count = base.Count(p => p.ProjectName == prjName);
            return count < 1 ? true : false;
        }
    }
}
