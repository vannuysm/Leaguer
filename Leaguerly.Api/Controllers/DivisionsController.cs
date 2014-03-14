using Leaguerly.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class DivisionsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public DivisionsController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IEnumerable<Division>> Get() {
            var divisions = await _db.Divisions.ToListAsync();
            return divisions;
        }

        public async Task<IEnumerable<Division>> GetByLeagueId(int leagueId) {
            var divisions = await _db.Divisions
                .Where(division => division.LeagueId == leagueId)
                .ToListAsync();

            return divisions;
        }

        public async Task<Division> Get(int id) {
            var division = await _db.Divisions.FindAsync(id);
            return division;
        }

        public async Task Post([FromBody] Division division) {
            _db.Divisions.Add(division);
            await _db.SaveChangesAsync();
        }

        public async Task Put(int id, [FromBody] Division division) {
            division.Id = id;

            _db.Entry(division).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var division = new Division { Id = id };

            _db.Divisions.Remove(division);
            await _db.SaveChangesAsync();
        }
    }
}