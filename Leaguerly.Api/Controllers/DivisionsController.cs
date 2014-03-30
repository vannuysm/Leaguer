using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/divisions")]
    public class DivisionsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public DivisionsController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> GetByLeagueId(int leagueId) {
            var divisions = await _db.Divisions
                .Where(division => division.LeagueId == leagueId)
                .ToListAsync();

            return Ok(divisions);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var division = await _db.Divisions.FindAsync(id);

            if (division == null) {
                return NotFound();
            }

            return Ok(division);
        }

        [Route("{id}/standings")]
        public async Task<IHttpActionResult> GetDivisionStandings(int id) {
            var games = await _db.Games
                .WithDetails()
                .Where(game => game.DivisionId == id && game.Result.Id > 0)
                .ToListAsync();

            var standings = Standing.CalculateStandings(games);

            return Ok(standings);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] Division division) {
            _db.Divisions.Add(division);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = division.Id }, division);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Division division) {
            division.Id = id;

            _db.Entry(division).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id) {
            var division = new Division { Id = id };

            _db.Divisions.Remove(division);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}