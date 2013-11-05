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
    public class ControlController:ControllerBase
    {
        private readonly IControlService ControlService;

        public ControlController(IControlService controlService)
        {
            this.ControlService = controlService;
            this.AddDisposableObject(controlService);
        }

		[HttpGet]
        public ActionResult Index(int pageIndex = 1)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            this.ControlService.CurrentProjectId = (int)Session["PID"];
            int recordCount;
			if (Session["HasQueried_c"] == null)
			{
				IEnumerable<Control> controls = this.ControlService.FindAll(pageIndex, PagingInfo.PageSize, out recordCount);
				ViewBag.Title = "Control Info";
				return (Session["PropertyListWhenEdit"] == null) ? RenderIndexView(controls, null, recordCount, pageIndex) :
					RenderIndexView(controls, (IList<SelectListItem>)Session["PropertyListWhenEdit"], recordCount, pageIndex);
			}
			else
			{
				return Query(new ControlInfo { SearchedConditionEntity = (SearchedConditionEntity)Session["SearchedConditionEntity"] }, pageIndex);
			}
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ControlInfo info, int pageIndex = 1)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            this.ControlService.CurrentProjectId = (int)Session["PID"];
            string requestKey = Request.Form.Keys[Request.Form.Keys.Count - 1] as string;
            if (requestKey.Contains(".")) requestKey = requestKey.Substring(0, requestKey.IndexOf("."));
			object pageindex = Session["QueriedPageIndex_c"] ?? pageIndex;
            switch (requestKey)
            {
                case "query":
                    return Query(info,1);
                case "edit":
                    return Edit(info.SelectedControl.Id,info.SelectedControl.ControlProperty.Type, (int)pageindex);
                case "delete":
					return Delete(info.SelectedControl.Id, (int)pageindex);
                case "save":
					return Save(info.SelectedControl, (int)pageindex);
                default:
                    Session["UpdatedId_c"] = null;
                    Session["Log_c"] = null;
					return RedirectToAction("Index", new { pageIndex = (int)pageindex });
            }
        }

        private ActionResult Query(ControlInfo info, int pageIndex)
        {
            this.ControlService.CurrentProjectId = (int)Session["PID"];
            int recordCount;
            IEnumerable<Control> controls = this.ControlService.QueryControls(info.SearchedConditionEntity.ControlType,
                info.SearchedConditionEntity.ControlProperty,
                info.SearchedConditionEntity.PropertyValue,
				info.SearchedConditionEntity.ControlName, pageIndex, PagingInfo.PageSize, out recordCount);
            Session["HasQueried_c"] = true;
			Session["QueriedPageIndex_c"] = pageIndex;
            Session["SearchedConditionEntity"] = info.SearchedConditionEntity;
			return (Session["PropertyListWhenEdit"] == null) ? RenderIndexView(controls, null, recordCount, pageIndex) :
				RenderIndexView(controls, (IList<SelectListItem>)Session["PropertyListWhenEdit"], recordCount, pageIndex);
        }

        private ActionResult Edit(int cid, string selectedType, int pageIndex)
        {
            Session["UpdatedId_c"] = cid;
            string selectedProperty = this.ControlService.GetControlPropertyById(cid);
            IEnumerable<string> properties = this.ControlService.GetControlPropertiesAgainstControlType(selectedType);
            Session["PropertyListWhenEdit"] = BindDropdownlist(properties, selectedProperty, false);
            if (null != Session["HasQueried_c"])
            {
                SearchedConditionEntity sce = Session["SearchedConditionEntity"] as SearchedConditionEntity;
                ControlInfo ci = new ControlInfo
                {
                    SearchedConditionEntity = sce,
                    PropertyListWhenEdit = Session["PropertyListWhenEdit"] as IList<SelectListItem>
                };
                return Query(ci,pageIndex);
            }
			return RedirectToAction("Index", new {	pageIndex=pageIndex});

        }

		private ActionResult Delete(int cid, int pageIndex)
        {
            this.ControlService.CurrentProjectId = (int)Session["PID"];
            string log = "";
            bool isSuccess = this.ControlService.DeleteControl(cid, out log);
            Session["UpdatedId_c"] = null;
            Session["Log_c"] = isSuccess ? null : log;
            Session["PropertyListWhenEdit"] = null;
			return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        private ActionResult Save(Control updatedItem,int pageIndex)
        {
            int cid = updatedItem.Id;
            if (null != updatedItem.Name || null != updatedItem.ControlProperty.Property)
            {
                this.ControlService.CurrentProjectId = (int)Session["PID"];
                string log = "";
                bool isSuccess = this.ControlService.UpdateControl(cid,updatedItem.ControlProperty.Type,
                    updatedItem.ControlProperty.Property,updatedItem.PropertyValue,updatedItem.Name, out log);
                if (isSuccess)
                {
                    Session["Log_c"] = null;
                    Session["UpdatedId_c"] = null;
                    Session["PropertyListWhenEdit"] = null;
					return RedirectToAction("Index", new { pageIndex = pageIndex });
                }
                Session["Log_c"] = log;
                return Edit(cid, updatedItem.ControlProperty.Property, pageIndex);
            }
            Session["Log_c"] = "Control Type, Property, Name Required!";
            return Edit(cid, updatedItem.ControlProperty.Property, pageIndex);
        }

		[HttpGet]
        public ActionResult Create()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            ViewBag.Title = "Create Control";
            ClearSessions();
            return View("Create", new Control());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Control instance)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            ViewBag.Title = "Create Control";
            ClearSessions();
            string requestKey = Request.Form.Keys[Request.Form.Keys.Count - 1] as string;
            if (requestKey.Contains(".")) requestKey = requestKey.Substring(0, requestKey.IndexOf("."));
            if (requestKey == "save")
            {
                if (ModelState.IsValid)
                {
                    string log = "";
                    this.ControlService.CurrentProjectId = (int)Session["PID"];
                    bool isSuccess = this.ControlService.AddControl(instance.ControlProperty.Type,instance.ControlProperty.Property,instance.PropertyValue,instance.Name, out log);
                    if (isSuccess)
                    {
						Session["HasQueried_c"] = null;
                        return RedirectToAction("Index");
                    }
                    ViewBag.Log = log;
                }
                return View("Create", new Control());
            }
			Session["Log_c"] = null;
			Session["HasQueried_c"] = null;
            return RedirectToAction("Index");
        }

		[HttpGet]
        public ActionResult LoadTypes()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            this.ControlService.Platform = (string)Session["PLATFORM"];
            this.ControlService.CurrentProjectId = (int)Session["PID"];
            IEnumerable<string> types = this.ControlService.GetControlTypes();
            IList<SelectListItem> typeItems = BindDropdownlist(types, "", true);
            return Json(typeItems, JsonRequestBehavior.AllowGet);
        }

		[HttpGet]
        public ActionResult LoadProperties()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            string controlType = (null != Request.Params["controlType"]) ? Request.Params["controlType"] : "";
            this.ControlService.Platform = (string)Session["PLATFORM"];
            this.ControlService.CurrentProjectId = (int)Session["PID"];
            IEnumerable<string> properties = this.ControlService.GetControlPropertiesAgainstControlType(controlType);
            IList<SelectListItem> propertyItems = BindDropdownlist(properties, "", true);
            return Json(propertyItems, JsonRequestBehavior.AllowGet);
        }

        private ActionResult RenderIndexView(IEnumerable<Control> controls, IList<SelectListItem> propertyListWhenEdit, int recordCount, int pageIndex)
        {
            ControlInfo ci = new ControlInfo
            {
                Controls = controls,
                PropertyListWhenEdit = propertyListWhenEdit??null,
                SearchedConditionEntity = new SearchedConditionEntity
                {
                    ControlType = Nameof<Control>.Property(c => c.ControlProperty.Type),
                    ControlProperty = Nameof<Control>.Property(c => c.ControlProperty.Property),
                    PropertyValue = Nameof<Control>.Property(c => c.PropertyValue),
                    ControlName = Nameof<Control>.Property(c => c.Name)
                }
            };
            Func<int, UrlHelper, string> pageUrlAccessor = (currentPage, helper) => helper.RouteUrl("ControlPage", new { PageIndex = currentPage }).ToString();
            ViewResult result = View(ci);
            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;
            return result;
        }

        private IList<SelectListItem> BindDropdownlist(IEnumerable<string> data,string selectedText,bool includeBlank)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            if(includeBlank)
                items.Add(new SelectListItem { Text = "", Value = "" });
            Parallel.ForEach(data,item=>items.Add(new SelectListItem { Text = item, Value = item, Selected = (selectedText==item) }));
            return items;
        }

        private void ClearSessions()
        {
            Session["Log_c"] = null;
            Session["UpdatedId_c"] = null;
            Session["PropertyListWhenEdit"] = null;
        }
    }
}