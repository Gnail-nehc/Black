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
    public class TestDataController:ControllerBase
    {
        private readonly ITestDataService TestDataService;

        public TestDataController(ITestDataService testDataService)
        {
            this.TestDataService = testDataService;
            this.AddDisposableObject(testDataService);
        }

        public ActionResult Index(int pageIndex = 1)
        {
			ViewBag.Title = "Test Data Info";
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            Session["Log_bt"] = null;
            this.TestDataService.CurrentProjectId = (int)Session["PID"];
            int recordCount;
			if (Session["HasQueried_t"] == null)
			{
				IEnumerable<TestData> testDataMore;
				if (null == Session["TestCaseBound"])
					testDataMore = this.TestDataService.FindAll(pageIndex, PagingInfo.PageSize, out recordCount);
				else
					testDataMore = this.TestDataService.QueryTestDatas(Session["TestCaseBound"].ToString(), "", "", pageIndex, PagingInfo.PageSize, out recordCount);

				return RenderIndexView(testDataMore, recordCount, pageIndex);
			}
			else
			{
				return Query(new TestDataInfo { FindConditionEntity = (FindConditionEntity)Session["FindConditionEntity"] }, pageIndex);
			}
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(TestDataInfo info, int pageIndex = 1)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            this.TestDataService.CurrentProjectId = (int)Session["PID"];
            string requestKey = Request.Form.Keys[Request.Form.Keys.Count - 1] as string;
            if (requestKey.Contains(".")) requestKey = requestKey.Substring(0, requestKey.IndexOf("."));
			object pageindex = Session["QueriedPageIndex_t"] ?? pageIndex;
            switch (requestKey)
            {
                case "query":
                    return Query(info, 1);
                case "edit":
					return Edit(info.SelectedTestData.Id, (int)pageindex);
                case "delete":
					return Delete(info.SelectedTestData.Id, (int)pageindex);
                case "save":
					return Save(info.SelectedTestData, (int)pageindex);
                default:
                    Session["UpdatedId_t"] = null;
                    Session["Log_t"] = null;
					return RedirectToAction("Index", new { pageIndex = (int)pageindex });
            }
        }

        private ActionResult Query(TestDataInfo info, int pageIndex)
        {
            this.TestDataService.CurrentProjectId = (int)Session["PID"];
            int recordCount;
            string tcid;
            if (null == Session["TestCaseBound"])
                tcid = info.FindConditionEntity.TestCaseId;
            else
                tcid = Session["TestCaseBound"].ToString();
            IEnumerable<TestData> testDataMore = this.TestDataService.QueryTestDatas(tcid,
                info.FindConditionEntity.Name, info.FindConditionEntity.Value, pageIndex, PagingInfo.PageSize, out recordCount);
            Session["HasQueried_t"] = true;
			Session["QueriedPageIndex_t"] = pageIndex;
            Session["FindConditionEntity"] = info.FindConditionEntity;
			return RenderIndexView(testDataMore, recordCount, pageIndex);
        }

        private ActionResult Edit(int tdid,int pageIndex)
        {
            Session["UpdatedId_t"] = tdid;
            if (null != Session["HasQueried_t"])
            {
                FindConditionEntity fce = Session["FindConditionEntity"] as FindConditionEntity;
                string tcid;
                if (null == Session["TestCaseBound"])
                    tcid = fce.TestCaseId;
                else
                    tcid = Session["TestCaseBound"].ToString();
                TestDataInfo info = new TestDataInfo { FindConditionEntity = new FindConditionEntity { TestCaseId = tcid, Name = fce.Name, Value = fce.Value } };
                return Query(info, pageIndex);
            }
			return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        private ActionResult Delete(int tdid, int pageIndex)
        {
            this.TestDataService.CurrentProjectId = (int)Session["PID"];
            string log = "";
            bool isSuccess = this.TestDataService.DeleteTestData(tdid, out log);
            Session["UpdatedId_t"] = null;
            Session["Log_t"] = isSuccess ? null : log;
			return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        private ActionResult Save(TestData updatedItem, int pageIndex)
        {
            int tdid = updatedItem.Id;
            if (null != updatedItem.TestCaseId || null != updatedItem.Name)
            {
				
                this.TestDataService.CurrentProjectId = (int)Session["PID"];
                string log = "";
                string tcid;
                if (null == Session["TestCaseBound"])
                    tcid = updatedItem.TestCaseId;
                else
                    tcid = Session["TestCaseBound"].ToString();
                TestData instance = new TestData { Id = tdid, TestCaseId=tcid, Name = updatedItem.Name, Value = updatedItem.Value };
                bool isSuccess = this.TestDataService.UpdateTestData(instance, out log);
                if (isSuccess)
                {
                    Session["Log_t"] = null;
                    Session["UpdatedId_t"] = null;
					return RedirectToAction("Index", new { pageIndex = pageIndex });
                }
                Session["Log_t"] = log;
                return Edit(tdid, pageIndex);
            }
            Session["Log_t"] = "Test Case ID and Name Required!";
            return Edit(tdid, pageIndex);
        }

        public ActionResult Create()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            Session["Log_bt"] = null;
            ViewBag.Title = "Create Test Data";
            return View("Create", new TestData());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TestData instance)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            ViewBag.Title = "Create Test Data";
            string requestKey = Request.Form.Keys[Request.Form.Keys.Count - 1] as string;
            if (requestKey.Contains(".")) requestKey = requestKey.Substring(0, requestKey.IndexOf("."));
            if (requestKey == "save")
            {
                string log = "";
                this.TestDataService.CurrentProjectId = (int)Session["PID"];
                if (null == Session["TestCaseBound"])
                {
                    if (ModelState.IsValid)
                    {
                        bool isSuccess = this.TestDataService.AddTestData(instance, out log);
                        if (isSuccess)
                        {
                            Session["Log_t"] = null;
							Session["HasQueried_t"] = null;
                            return RedirectToAction("Index");
                        }
                        ViewBag.Log = log;
                    }
                }
                else
                {
                    instance.TestCaseId = Session["TestCaseBound"].ToString();
                    bool isSuccess = this.TestDataService.AddTestData(instance, out log);
                    if (isSuccess)
                    {
                        Session["Log_t"] = null;
                        return RedirectToAction("Index");
                    }
                    ViewBag.Log = log;
                }
                return View("Create", new TestData());
            }
            Session["Log_t"] = null;
			Session["HasQueried_t"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult BindTestCase()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            return View(new TestData());
        }

        [HttpPost]
        public ActionResult BindTestCase(TestData testdata)
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            if (null != testdata.TestCaseId)
            {
                Session["TestCaseBound"] = testdata.TestCaseId;
            }
            else
            {
                return View();
            }
            bool isExisting=this.TestDataService.ExistTestCase(testdata.TestCaseId);
            if (!isExisting)
            {
                Session["Log_bt"] = "The test case isn't existing.";
                return View();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Unbind()
        {
            ActionResult ar = VerifyIfProjectSelected("PRJNAME");
            if (null != ar) return ar;
            Session["TestCaseBound"] = null;
            return RedirectToAction("Index");
        }

        private ActionResult RenderIndexView(IEnumerable<TestData> testDataMore, int recordCount, int pageIndex)
        {
            TestDataInfo tdi = new TestDataInfo
            {
                TestDataMore = testDataMore,
                SelectedTestData = new TestData(),
                FindConditionEntity = new FindConditionEntity
                {
                    TestCaseId = Nameof<TestData>.Property(td => td.TestCaseId),
                    Name = Nameof<TestData>.Property(td => td.Name), 
                    Value = Nameof<GlobalSetting>.Property(gs => gs.Value) }
            };
            Func<int, UrlHelper, string> pageUrlAccessor = (currentPage, helper) => helper.RouteUrl("TestDataPage", new { PageIndex = currentPage }).ToString();
			ViewResult result = View(tdi);
            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;
            return result;
        } 
    }
}