using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IProjectRepository
    {
        IEnumerable<Project> FindAll();
        bool SelectProject(int prjId, out string prjName, out string platform);
        bool AddProject(Project project, out string log);
        bool DeleteProject(int id, out string log);
        bool UpdateProject(Project project, out string log);
        int GetCount(Expression<Func<Project, bool>> filter);
    }
}
