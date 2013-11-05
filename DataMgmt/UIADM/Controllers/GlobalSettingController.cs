using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Interfaces;
using DAL.Entities;
using UIADM.Extensions;
using UIADM.ViewModels;

namespace UIADM.Controllers
{
    public class GlobalSettingController : ControllerBase
    {
        private readonly IGlobalSettingService GlobalSettingService;

        public GlobalSettingController(IGlobalSettingService globalSettingService)
        {
            this.GlobalSettingService = globalSettingService;
            this.AddDisposableObject(globalSettingService);
        }

        
        public ActionResult Index(int pageIndex = 1)
        {
            ActionResult ar=VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            this.GlobalSettingService.CurrentProjectId = (int)Session["PID"];
            int recordCount;
			ViewBag.Title = "Global Setting Info";
			if (Session["HasQueried_g"] == null)
			{
				IEnumerable<GlobalSetting> globalSettings = this.GlobalSettingService.FindAll(pageIndex, PagingInfo.PageSize, out recordCount);
				return RenderIndexView(globalSettings, recordCount, pageIndex);
			}
			else
			{
				return Query(new GlobalSettingInfo { QueryConditionEntity = (QueryConditionEntity)Session["QueryConditionEntity"] }, pageIndex);
			}
        }

        [HttpPost]
        [ValidateInput(false)]
		public ActionResult Index(GlobalSettingInfo info, int pageIndex = 1)
        {
            ActionResult ar=VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            this.GlobalSettingService.CurrentProjectId = (int)Session["PID"];
            string requestKey=Request.Form.Keys[Request.Form.Keys.Count-1] as string;
            if(requestKey.Contains("."))  requestKey=requestKey.Substring(0,requestKey.IndexOf("."));
			object pageindex = Session["QueriedPageIndex_g"] ?? pageIndex;
            switch (requestKey)
            {
                case "query":
                    return Query(info,1);
                case "edit":
					return Edit(info.SelectedGlobalSetting.Id, (int)pageindex);
                case "delete":
					return Delete(info.SelectedGlobalSetting.Id, (int)pageindex);
                case "save":
					return Save(info.SelectedGlobalSetting, (int)pageindex);
                default:
                    Session["UpdatedId_g"] = null;
                    Session["Log_g"] = null;
					return RedirectToAction("Index", new { pageIndex = (int)pageindex });
            }
        }

        private ActionResult Query(GlobalSettingInfo info, int pageIndex)
        {
            this.GlobalSettingService.CurrentProjectId = (int)Session["PID"];
            int recordCount;
            IEnumerable<GlobalSetting> globalSettings = this.GlobalSettingService.QueryGlobalSettings(info.QueryConditionEntity.Name, info.QueryConditionEntity.Value, pageIndex, PagingInfo.PageSize, out recordCount);
			Session["HasQueried_g"] = true;
			Session["QueriedPageIndex_g"] = pageIndex;
            Session["QueryConditionEntity"] = info.QueryConditionEntity;
			return RenderIndexView(globalSettings, recordCount, pageIndex);
        }

		private ActionResult Edit(int gsid, int pageIndex)
        {
            Session["UpdatedId_g"] = gsid;
            if (null != Session["HasQueried_g"])
            {
                QueryConditionEntity qce = Session["QueryConditionEntity"] as QueryConditionEntity;
                GlobalSettingInfo info = new GlobalSettingInfo
                {   QueryConditionEntity =new QueryConditionEntity { Name = qce.Name, Value = qce.Value }};
                return Query(info, pageIndex);
            }
			return RedirectToAction("Index", new { pageIndex = pageIndex }); 
        }

		private ActionResult Delete(int gsid, int pageIndex)
        {
            this.GlobalSettingService.CurrentProjectId = (int)Session["PID"];
            string log="";
            bool isSuccess = this.GlobalSettingService.DeleteGlobalSetting(gsid, out log);
            Session["UpdatedId_g"] = null;
            Session["Log_g"] = isSuccess ? null : log;
			return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

		private ActionResult Save(GlobalSetting updatedItem, int pageIndex)
        {
            int gsid = updatedItem.Id;
            if (null != updatedItem.Name)
            {
                this.GlobalSettingService.CurrentProjectId = (int)Session["PID"];
                string log="";
                GlobalSetting instance = new GlobalSetting { Id = gsid, Name = updatedItem.Name, Value = updatedItem.Value };
                bool isSuccess = this.GlobalSettingService.UpdateGlobalSetting(instance, out log);
                if (isSuccess)
                {
                    Session["Log_g"] = null;
                    Session["UpdatedId_g"] = null;
					return RedirectToAction("Index", new { pageIndex = pageIndex });
                }
                Session["Log_g"] = log;
                return Edit(gsid, pageIndex);
            }
            Session["Log_g"] = "Name Required!";
			return Edit(gsid, pageIndex);
        }

        public ActionResult Create()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            ViewBag.Title = "Create Global Setting";
            return View("Create", new GlobalSetting());
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(GlobalSetting instance)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            ViewBag.Title = "Create Global Setting";
            string requestKey = Request.Form.Keys[Request.Form.Keys.Count - 1] as string;
            if (requestKey.Contains(".")) requestKey = requestKey.Substring(0, requestKey.IndexOf("."));
            if (requestKey == "save")
            {
                if (ModelState.IsValid)
                {
                    string log = "";
                    this.GlobalSettingService.CurrentProjectId = (int)Session["PID"];
                    bool isSuccess = this.GlobalSettingService.AddGlobalSetting(instance, out log);
                    if (isSuccess)
                    {
                        Session["Log_g"] = null;
						Session["HasQueried_g"] = null;
                        return RedirectToAction("Index");
                    }
                    ViewBag.Log = log;
                }
                return View("Create", new GlobalSetting());
            }
            Session["Log_g"] = null;
            return RedirectToAction("Index");
        }

        private ActionResult RenderIndexView(IEnumerable<GlobalSetting> globalSettings, int recordCount, int pageIndex)
        {

            GlobalSettingInfo gsi = new GlobalSettingInfo { GlobalSettings = globalSettings,    SelectedGlobalSetting = new GlobalSetting(),
                                                                 QueryConditionEntity = new QueryConditionEntity { Name = Nameof<GlobalSetting>.Property(gs => gs.Name), Value = Nameof<GlobalSetting>.Property(gs => gs.Value) }
                                                            };
            Func<int, UrlHelper, string> pageUrlAccessor = (currentPage, helper) => helper.RouteUrl("GlobalSettingPage", new { PageIndex = currentPage }).ToString();
            ViewResult result = View(gsi);
            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;
            return result;
        } 

    }   
}
