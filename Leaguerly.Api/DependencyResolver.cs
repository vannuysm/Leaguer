using System.Reflection;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using Leaguerly.Api.Models;

namespace Leaguerly.Api
{
    public static class DependencyResolver
    {
        public static IDependencyResolver GetResolver() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(d => new LeaguerlyDbContext()).AsSelf().InstancePerApiRequest();

            var container = builder.Build();
            return new AutofacWebApiDependencyResolver(container);
        }
    }
}