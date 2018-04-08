using MefBase;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ////仅在路由测试时使用，不用时注释掉
            //RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);

            //创建一个程序目录,用于从一个程序集获取获取所有的组件定义
            var catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);

            //创建一个组合容器
            var solver = new MefDependencySolver(catalog);
            //设置MEF依赖注入容器
            DependencyResolver.SetResolver(solver);
            GlobalConfiguration.Configuration.DependencyResolver = solver;
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }
    }
}