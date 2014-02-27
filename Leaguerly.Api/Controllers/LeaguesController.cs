using System.Linq;
using Leaguerly.Repositories;
using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class LeaguesController : ApiController
    {
        private readonly ILeagueRepository _leagueRepo;
        private readonly IDivisionRepository _divisionRepo;

        public LeaguesController(ILeagueRepository leagueRepo, IDivisionRepository divisionRepo) {
            _leagueRepo = leagueRepo;
            _divisionRepo = divisionRepo;
        }

        public IEnumerable<League> Get() {
            var leagues = _leagueRepo.All();
            return leagues;
        }

        public League Get(int id) {
            var league = _leagueRepo.Get(id);
            return league;
        }

        public void Post([FromBody] League league) {
            _leagueRepo.Create(league);
        }

        public void Put(int id, [FromBody] League league) {
            _leagueRepo.Update(league);
        }

        public void Delete(int id) {
            _leagueRepo.Delete(id);
        }
    }
}