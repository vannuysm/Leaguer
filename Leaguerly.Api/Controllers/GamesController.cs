using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class GamesController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public GamesController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> Get() {
            var games = await _db.Games
                .WithDetails()
                .ToListAsync();

            return Ok(games);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var game = await _db.Games
                .WithDetails()
                .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }

            return Ok(game);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] Game game) {
            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = game.Id }, game);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Game game) {
            game.Id = id;

            _db.Entry(game).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id) {
            var game = new Game { Id = id };

            _db.Games.Remove(game);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}