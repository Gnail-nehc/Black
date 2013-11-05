using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using BAL.Interfaces;
using BAL.Repositories;
using BAL.Services;
using DAL.Entities;
using Microsoft.Practices.Unity;
using UnityHelper;

namespace UIADM
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            //IUnityContainer unityContainer = new UnityContainer();
            //unityContainer.RegisterType<IProjectRepository, ProjectRepository>();
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new UnityHttpControllerActivator(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            //container.RegisterType<DbContext, QADB_Entities>();

            container.RegisterType<IProjectRepository, ProjectRepository>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IGlobalSettingRepository, GlobalSettingRepository>();
            container.RegisterType<IGlobalSettingService, GlobalSettingService>();
            container.RegisterType<ITestDataRepository, TestDataRepository>();
            container.RegisterType<ITestDataService, TestDataService>();
            container.RegisterType<IControlRepository, ControlRepository>();
            container.RegisterType<IHomeService, HomeService>();
            container.RegisterType<IControlTypeRepository, ControlTypeRepository>();
            container.RegisterType<IControlPropertyRepository, ControlPropertyRepository>();
            container.RegisterType<IControlService, ControlService>();
            
            return container;
        }
    }
}