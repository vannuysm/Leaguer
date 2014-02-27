using System.Reflection;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using Leaguerly.Repositories;
using Leaguerly.Repositories.Ef;
using Leaguerly.Repositories.DataModels;

namespace Leaguerly.Api
{
    public static class DependencyResolver
    {
        public static IDependencyResolver GetResolver() {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            RegisterRepositories(builder);

            var container = builder.Build();

            return new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.Register(c => new LeagueRepository())
                .As<ILeagueRepository>()
                .InstancePerApiRequest();

            builder.Register(c => new DivisionRepository())
                .As<IDivisionRepository>()
                .InstancePerApiRequest();

            builder.Register(c => new TeamRepository())
                .As<ITeamRepository>()
                .InstancePerApiRequest();

            builder.Register(c => new PlayerRepository())
                .As<IPlayerRepository>()
                .InstancePerApiRequest();
        }
    }
}