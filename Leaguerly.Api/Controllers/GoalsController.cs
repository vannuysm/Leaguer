using Leaguerly.Api.Models;
using System.Collections.ObjectModel;
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

        [Route("players/{id}", Name = "GetPlayerGoals")]
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
                .Join(_db.Games,
                    goal => goal.GameId,
                    game => game.Id,
                    (goal, game) => new { game.DivisionId, goal.Player, goal.Count }
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

        [Route("divisions/{alias}")]
        public async Task<IHttpActionResult> GetByDivision(string alias) {
            var divisionId = await _db.Divisions
                .Where(division => division.Alias == alias)
                .Select(division => division.Id)
                .SingleOrDefaultAsync();

            if (divisionId == 0) {
                return NotFound();
            }

            var goals = await _db.Goals
                .Join(_db.Games,
                    goal => goal.GameId,
                    game => game.Id,
                    (goal, game) => new { game.DivisionId, goal.Player, goal.Count }
                )
                .Where(gg => gg.DivisionId == divisionId)
                .GroupBy(gg => gg.Player)
                .Select(group => new {
                    Player = group.Key,
                    Goals = group.Sum(goal => goal.Count)
                })
                .OrderByDescending(gd => gd.Goals)
                .ToListAsync();

            return Ok(goals);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGameGoals(int gameId, int teamId) {
            var game = await _db.Games
                .WithDetails()
                .SingleOrDefaultAsync(g => g.Id == gameId);

            if (game == null) {
                return NotFound();
            }

            var team = await _db.Teams
                .Include(t => t.Players)
                .SingleOrDefaultAsync(t => t.Id == teamId);

            if (team == null) {
                return NotFound();
            }

            var players = team.Players.Select(player => player.Id);

            var goals = game.Goals
                .Where(goal => players.Contains(goal.Player.Id));

            return Ok(goals);
        }

        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] AddGoalBindingModel model) {
            var game = await _db.Games
                .WithDetails()
                .SingleOrDefaultAsync(g => g.Id == model.GameId);

            if (game == null) {
                return NotFound();
            }

            var newGoal = new Goal {
                PlayerId = model.PlayerId,
                Count = model.Count
            };

            game.Goals.Add(newGoal);

            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetPlayerGoals", new { id = model.PlayerId }, newGoal);
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

            _db.Goals.Attach(goal);
            _db.Goals.Remove(goal);

            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}