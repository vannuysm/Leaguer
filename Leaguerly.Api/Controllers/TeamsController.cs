using Leaguerly.Repositories;
using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class TeamsController : ApiController
    {
        private readonly IRepository<Team, int> _teamRepo;

        public TeamsController(IRepository<Team, int> teamRepo) {
            _teamRepo = teamRepo;
        }

        public IEnumerable<Team> Get() {
            var teams = _teamRepo.All();
            return teams;
        }

        public Team Get(int id) {
            var team = _teamRepo.Get(id);
            return team;
        }

        public void Post([FromBody] Team team) {
            _teamRepo.Create(team);
        }

        public void Put(int id, [FromBody] Team team) {
            _teamRepo.Update(team);
        }

        public void Delete(int id) {
            _teamRepo.Delete(id);
        }
    }
}
