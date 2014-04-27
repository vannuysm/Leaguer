using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Leaguerly.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) {
            config.DependencyResolver = DependencyResolver.GetResolver();

            // Enable OAuth and CORS support
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Route Config
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // JSON Config
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
