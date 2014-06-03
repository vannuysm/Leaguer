using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/games")]
    public class GamesController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public GamesController(LeaguerlyDbContext db) {
            _db = db;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGames() {
            var games = await _db.Games
                .WithDetails()
                .ToListAsync();

            return Ok(games);
        }

        [Route("{id}", Name="GetSingleGame")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGame(int id) {
            var game = await _db.Games
                .WithDetails()
                .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }

            return Ok(game);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}/result")]
        [HttpPut]
        public async Task<IHttpActionResult> PutResult(int id, [FromBody] Game model) {
            var game = await _db.Games
                .WithDetails()
                .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }

            game.IncludeInStandings = model.IncludeInStandings;
            game.WasCancelled = model.WasCancelled;
            game.WasForfeited = model.WasForfeited;
            game.ForfeitingTeamId = model.ForfeitingTeamId;

            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Game game) {
            var newGame = new Game {
                AwayTeam = null,
                AwayTeamId = game.AwayTeam.Id,
                HomeTeam = null,
                HomeTeamId = game.HomeTeam.Id,
                Date = game.Date,
                DivisionId = game.DivisionId,
                Location = null,
                LocationId = game.Location.Id
            };

            _db.Games.Add(newGame);

            await _db.SaveChangesAsync();

            game.Id = newGame.Id;

            return CreatedAtRoute("GetSingleGame", new { id = game.Id }, game);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Game game) {
            game.Id = id;

            _db.Entry(game).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id) {
            var game = new Game { Id = id };

            _db.Games.Remove(game);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}