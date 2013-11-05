using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UIADM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Project", action = "Index" });

            routes.MapRoute(
                name: "SelectProject",
                url: "Project",
                defaults: new { controller = "Project", action = "SelectProject" });

            routes.MapRoute(
                name: "Home",
                url: "Home",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "GlobalSetting",
                url: "GlobalSetting",
                defaults: new { controller = "GlobalSetting", action = "Index", pageIndex = 1 });

            routes.MapRoute(
                name: "TestData",
                url: "TestData",
                defaults: new { controller = "TestData", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Control",
                url: "Control",
                defaults: new { controller = "Control", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "ViewProjects",
                url: "ViewProjects",
                defaults: new { controller = "ViewProjects", action = "Index" });

            routes.MapRoute(
                name: "CreateGlobalSetting",
                url: "GlobalSetting/Create",
                defaults: new { controller = "GlobalSetting", action = "Create" });

            routes.MapRoute(
                name: "CreateControl",
                url: "Control/Create",
                defaults: new { controller = "Control", action = "Create" });

            routes.MapRoute(
                name: "CreateTestData",
                url: "TestData/Create",
                defaults: new { controller = "TestData", action = "Create" });

            routes.MapRoute(
                name: "BindTestCase",
                url: "TestData/Bind",
                defaults: new { controller = "TestData", action = "BindTestCase" });

            routes.MapRoute(
                name: "UnbindTestCase",
                url: "TestData/Unbind",
                defaults: new { controller = "TestData", action = "Unbind" });

            routes.MapRoute(
                name: "GlobalSettingPage",
                url: "GlobalSetting/Page{pageIndex}",
                defaults: new
                {
                    controller = "GlobalSetting",
                    action = "Index",
                    pageIndex = 1
                },
                constraints: new { pageIndex = @"\d+" });

            routes.MapRoute(
                name: "TestDataPage",
                url: "TestData/Page{pageIndex}",
                defaults: new
                {
                    controller = "TestData",
                    action = "Index",
                    pageIndex = 1
                },
                constraints: new { pageIndex = @"\d+" });

            routes.MapRoute(
                name: "ControlPage",
                url: "Control/Page{pageIndex}",
                defaults: new
                {
                    controller = "Control",
                    action = "Index",
                    pageIndex = 1
                },
                constraints: new { pageIndex = @"\d+" });

			routes.MapRoute(
				name: "LoadTypes",
				url: "Control/LoadTypes",
				defaults: new { controller = "Control", action = "LoadTypes" });

			routes.MapRoute(
			   name: "LoadProperties",
			   url: "Control/LoadProperties",
			   defaults: new { controller = "Control", action = "LoadProperties" });

			routes.MapRoute(
				name: "LoadTypesWhenCreate",
				url: "Control/Create/LoadTypes",
				defaults: new { controller = "Control", action = "LoadTypes" });
				
			routes.MapRoute(
				name: "LoadPropertiesWhenCreate",
				url: "Control/Create/LoadProperties",
				defaults: new { controller = "Control", action = "LoadProperties" });

        }
    }
}