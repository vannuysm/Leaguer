using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Leaguerly.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) {
            config.DependencyResolver = DependencyResolver.GetResolver();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
