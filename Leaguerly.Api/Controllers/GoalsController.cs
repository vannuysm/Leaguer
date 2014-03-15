using System.Linq;
using Leaguerly.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;

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
        public async Task<IHttpActionResult> GetByPlayer() {
            var goals = await _db.Goals
                .GroupBy(goal => goal.Player)
                .Select(group => new {
                  Player = group.Key,
                  Goals = group.Sum(goal => goal.Count)
                })
                .ToListAsync();

            return Ok(goals);
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

        public async Task Post([FromBody] Goal goal) {
            _db.Goals.Add(goal);
            await _db.SaveChangesAsync();
        }

        public async Task Put(int id, [FromBody] Goal goal) {
            goal.Id = id;

            _db.Entry(goal).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var goal = new Goal { Id = id };

            _db.Goals.Remove(goal);
            await _db.SaveChangesAsync();
        }
    }
}