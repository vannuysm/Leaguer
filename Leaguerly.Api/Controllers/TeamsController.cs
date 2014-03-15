using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class TeamsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public TeamsController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> Get() {
            var teams = await _db.Teams.ToListAsync();

            return Ok(teams);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var team = await _db.Teams.FindAsync(id);

            if (team == null) {
                return NotFound();
            }

            return Ok(team);
        }

        public async Task<IHttpActionResult> Post([FromBody] Team team) {
            _db.Teams.Add(team);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = team.Id }, team);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] Team team) {
            team.Id = id;

            _db.Entry(team).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete(int id) {
            var team = new Team { Id = id };

            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}