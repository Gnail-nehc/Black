using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Interfaces;
using DAL.Entities;

namespace UIADM.Controllers
{
    public class ManageProjectController : HttpControllerBase
    {
        private readonly IProjectService ProjectService;

        public ManageProjectController(IProjectService projectService)
        {
            this.ProjectService = projectService;
            this.AddDisposableObject(projectService);
        }

        // GET api/manageproject
        public IEnumerable<Project> Get()
        {
            return this.ProjectService.FindAll();
        }

        // POST api/manageproject
        public void Post(Project project)
        {
            string log = "";
            this.ProjectService.UpdateProject(project, out log);
        }

        // PUT api/manageproject
        public void Put(Project project)
        {
            string log = "";
            this.ProjectService.AddProject(project, out log);
        }

        // DELETE api/manageproject/1
        public void Delete(int id)
        {
            string log = "";
            this.ProjectService.DeleteProject(id, out log);
        }

    }
}
