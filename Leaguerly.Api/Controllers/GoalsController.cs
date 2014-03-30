using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/goals")]
    public class GoalsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public GoalsController(LeaguerlyDbContext db) {
            _db = db;
        }

        [Route("players")]
        public async Task<IHttpActionResult> GetAllPlayers() {
            var goals = await _db.Goals
                .GroupBy(goal => goal.Player)
                .Select(group => new {
                  Player = group.Key,
                  Goals = group.Sum(goal => goal.Count)
                })
                .ToListAsync();

            return Ok(goals);
        }

        [Route("players/{id}")]
        public async Task<IHttpActionResult> GetByPlayer(int id) {
            var player = await _db.Players.SingleOrDefaultAsync(p => p.Id == id);

            if (player == null) {
                return NotFound();
            }

            var goals = await _db.Goals
                .Where(goal => goal.Player.Id == id)
                .SumAsync(goal => goal.Count);

            return Ok(new { Player = player, Goals = goals });
        }

        [Route("divisions")]
        public async Task<IHttpActionResult> GetAllDivisions() {
            var goals = await _db.Goals
                .Join(_db.GameResults,
                    goal => goal.GameResultId,
                    gameResult => gameResult.Id,
                    (goal, gameResult) => new { Goal = goal, GameResult = gameResult }
                )
                .Join(_db.Games,
                    grg => grg.GameResult.GameId,
                    game => game.Id,
                    (grg, game) => new { game.DivisionId, grg.Goal.Player, grg.Goal.Count }
                )
                .Join(_db.Divisions,
                    gg => gg.DivisionId,
                    division => division.Id,
                    (gg, division)=> new { Goal = gg, Division = division }
                )
                .GroupBy(gd => gd.Division)
                .Select(group => new {
                    Division = group.Key,
                    Players = group
                        .GroupBy(gd => gd.Goal.Player)
                        .Select(gd => new { Player = gd.Key, Goals = gd.Sum(g => g.Goal.Count) })
                        .OrderByDescending(gd => gd.Goals)
                })
                .ToListAsync();

            return Ok(goals);
        }

        [Route("divisions/{id}")]
        public async Task<IHttpActionResult> GetByDivision(int id) {
            var goals = await _db.Goals
                .Join(_db.GameResults,
                    goal => goal.GameResultId,
                    gameResult => gameResult.Id,
                    (goal, gameResult) => new { Goal = goal, GameResult = gameResult } 
                )
                .Join(_db.Games,
                    grg => grg.GameResult.GameId,
                    game => game.Id,
                    (grg, game) => new { game.DivisionId, grg.Goal.Player, grg.Goal.Count }
                )
                .Where(gg => gg.DivisionId == id)
                .GroupBy(gg => gg.Player)
                .Select(group => new {
                    Player = group.Key,
                    Goals = group.Sum(goal => goal.Count)
                })
                .OrderByDescending(gd => gd.Goals)
                .ToListAsync();

            return Ok(goals);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] Goal goal) {
            _db.Goals.Add(goal);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = goal.Id }, goal);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Goal goal) {
            goal.Id = id;

            _db.Entry(goal).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id) {
            var goal = new Goal { Id = id };

            _db.Goals.Remove(goal);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}