using Leaguerly.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class LeaguesController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public LeaguesController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IEnumerable<League>> Get() {
            var leagues = await _db.Leagues.ToListAsync();
            return leagues;
        }

        public async Task<League> Get(int id) {
            var league = await _db.Leagues.FindAsync(id);
            return league;
        }

        public async Task Post([FromBody] League league) {
            _db.Leagues.Add(league);
            await _db.SaveChangesAsync();
        }

        public async Task Put(int id, [FromBody] League league) {
            league.Id = id;

            _db.Entry(league).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var league = new League { Id = id };

            _db.Leagues.Remove(league);
            await _db.SaveChangesAsync();
        }
    }
}