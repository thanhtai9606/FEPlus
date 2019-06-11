using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace FEPlus.EMCSApi.App_Start
{
    public class Startup
    {
        private static readonly IUnityContainer _container = UnityHelpers.GetConfiguredContainer();

        public static void StartServer()
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            var startup = _container.Resolve<Startup>();
            IDisposable webApplication = WebApp.Start(baseAddress, startup.Configuration);

            try
            {
                Console.WriteLine("Started on {0} is running...", baseAddress);

                Console.ReadKey();
            }
            finally
            {
                webApplication.Dispose();
            }


        }
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new UnityDependencyResolver(UnityHelpers.GetConfiguredContainer());
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            log4net.Config.XmlConfigurator.Configure();
            appBuilder.UseWebApi(config);
        }
    }
}
