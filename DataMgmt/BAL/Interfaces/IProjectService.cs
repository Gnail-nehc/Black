using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<Project> FindAll();
        bool SelectProject(int prjId, out string prjName, out string platform);
        bool AddProject(Project project, out string log);
        bool DeleteProject(int id, out string log);
        bool UpdateProject(Project project, out string log);
    }
}
