using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(CorsApi.UnityConfig), "Register")]

namespace CorsApi
{
    public class UnityConfig
    {
        public static void Register()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // container.RegisterType<IPricing, Pricing>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}