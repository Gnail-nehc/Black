using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BAL.Interfaces;
using DAL.Entities;
using UIADM.Extensions;
using UIADM.ViewModels;

namespace UIADM.Controllers
{
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService ProjectService;

        public ProjectController(IProjectService projectService)
        {
            this.ProjectService = projectService;
            this.AddDisposableObject(projectService);
        }

        //[OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult Index()
        {
            Session.RemoveAll();
            IEnumerable<Project> projects = this.ProjectService.FindAll();
            IList<SelectListItem> prjItems = new List<SelectListItem>();
            Parallel.ForEach(projects, p => prjItems.Add(new SelectListItem { Text = p.ProjectName, Value = p.Id.ToString() }));
            ViewBag.Title = "Select Project";
            return View(new ProjectInfo() { ProjectItems = prjItems, TProperty = Nameof<Project>.Property(p => p.ProjectName) });
        }

        [HttpPost]
        public ActionResult Index(ProjectInfo pinfo)
        {
            string prjName, platform;
            int pid = 0;
            if (int.TryParse(pinfo.TProperty, out pid))
            {
                bool isSuccess = this.ProjectService.SelectProject(pid, out prjName, out platform);
                if (isSuccess)
                {
                    Session["PID"] = pid;
                    Session["PRJNAME"] = prjName;
                    Session["PLATFORM"] = platform;
                    return RedirectToRoute("Home");
                }
            }
            return Redirect("/");
        }

        public ActionResult SelectProject()
        {
            Session.RemoveAll();
            IEnumerable<Project> projects = this.ProjectService.FindAll();
            IList<SelectListItem> prjItems = new List<SelectListItem>();
            Parallel.ForEach(projects, p => prjItems.Add(new SelectListItem { Text = p.ProjectName, Value = p.Id.ToString() }));
            ViewBag.Title = "Select Project";
            return View(new ProjectInfo() { ProjectItems = prjItems, TProperty = Nameof<Project>.Property(p => p.ProjectName) });
        }


    }
}
