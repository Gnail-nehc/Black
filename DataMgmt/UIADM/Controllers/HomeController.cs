using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Interfaces;
using UIADM.ViewModels;

namespace UIADM.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IHomeService HomeService;

        public HomeController(IHomeService homeService)
        {
            this.HomeService = homeService;
            this.AddDisposableObject(homeService);
        }

        public ActionResult Index()
        {
            if (null != Session["PRJNAME"])
            {
                this.HomeService.CurrentProjectId = (int)Session["PID"];
                ViewBag.Title = "Home Page";
                return View(new SummaryInfo { TestCaseNo=HomeService.ReturnTestCaseNumber(),
                                              GlobalSettingNo = HomeService.ReturnGlobalSettingNumber(),
                                              TestDataNo = HomeService.ReturnTestDataNumber(),
                                              ControlNo = HomeService.ReturnControlNumber()
                });
            }
            return Redirect("/");
        }

        

    }
}
