using System.Linq;
using Leaguerly.Repositories;
using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class DivisionsController : ApiController
    {
        private readonly IDivisionRepository _divisionRepo;

        public DivisionsController(IDivisionRepository divisionRepo) {
            _divisionRepo = divisionRepo;
        }

        public IEnumerable<Division> Get() {
            var divisions = _divisionRepo.All();
            return divisions;
        }

        public IEnumerable<Division> GetByLeagueId(int leagueId) {
            var divisions = _divisionRepo.Query.Where(division => division.LeagueId == leagueId);
            return divisions;
        }

        public Division Get(int id) {
            var division = _divisionRepo.Get(id);
            return division;
        }

        public void Post([FromBody] Division division) {
            _divisionRepo.Create(division);
        }

        public void Put(int id, [FromBody] Division division) {
            _divisionRepo.Update(division);
        }

        public void Delete(int id) {
            _divisionRepo.Delete(id);
        }
    }
}