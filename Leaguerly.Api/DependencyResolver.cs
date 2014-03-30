using System.Reflection;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using Leaguerly.Api.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Leaguerly.Api
{
    public static class DependencyResolver
    {
        public static IDependencyResolver GetResolver() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(d => new LeaguerlyDbContext()).AsSelf().InstancePerApiRequest();
            builder.RegisterType<UserStore<IdentityUser>>().As<IUserStore<IdentityUser>>();
            builder.RegisterType<UserManager<IdentityUser>>();

            var container = builder.Build();
            return new AutofacWebApiDependencyResolver(container);
        }
    }
}