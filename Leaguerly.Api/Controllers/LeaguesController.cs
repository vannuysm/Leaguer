using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Net;
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

        public async Task<IHttpActionResult> Get() {
            var leagues = await _db.Leagues.ToListAsync();

            return Ok(leagues);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var league = await _db.Leagues.FindAsync(id);

            if (league == null) {
                return NotFound();
            }

            return Ok(league);
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