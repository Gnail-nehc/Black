using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Entities;

namespace BAL.Services
{
    public class ProjectService:ServiceBase,IProjectService
    {
        public IProjectRepository ProjectRepository { get; private set; }
        public ProjectService(IProjectRepository projectRepository)
        {
            this.ProjectRepository = projectRepository;
            this.AddDisposableObject(projectRepository);
        }

        public IEnumerable<Project> FindAll()
        {
            return this.ProjectRepository.FindAll();
        }

        public bool SelectProject(int prjId, out string prjName, out string platform)
        {
            string projectname, platformName;
            bool flag = this.ProjectRepository.SelectProject(prjId, out projectname, out platformName);
            prjName = projectname;
            platform = platformName;
            return flag;
        }

        public bool AddProject(Project project, out string log)
        {
            return this.ProjectRepository.AddProject(project, out log);
        }

        public bool DeleteProject(int id, out string log)
        {
            return this.ProjectRepository.DeleteProject(id, out log);
        }

        public bool UpdateProject(Project project, out string log)
        {
            return this.ProjectRepository.UpdateProject(project, out log);
        }
    }
}
