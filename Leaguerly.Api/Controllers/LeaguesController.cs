using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/leagues")]
    public class LeaguesController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public LeaguesController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> Get() {
            var leagues = await _db.Leagues.ToListAsync();

            return Ok(leagues);
        }

        [Route("{alias}/divisions")]
        public async Task<IHttpActionResult> GetLeagueDivisions(string alias) {
            var leagueId = await _db.Leagues
                .Where(league => league.Alias == alias)
                .Select(league => league.Id)
                .SingleOrDefaultAsync();

            var divisions = await _db.Divisions
                .Where(division => division.LeagueId == leagueId)
                .ToListAsync();

            return Ok(divisions);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] League league) {
            _db.Leagues.Add(league);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = league.Id }, league);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] League league) {
            league.Id = id;

            _db.Entry(league).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id) {
            var league = new League { Id = id };

            _db.Leagues.Remove(league);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}