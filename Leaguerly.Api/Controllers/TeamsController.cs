using Leaguerly.Api.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/teams")]
    public class TeamsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public TeamsController(LeaguerlyDbContext db) {
            _db = db;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get() {
            var teams = await _db.Teams.ToListAsync();

            return Ok(teams);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id) {
            var team = await _db.Teams.FindAsync(id);

            if (team == null) {
                return NotFound();
            }

            return Ok(team);
        }

        [Route("{id}/players")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamPlayers(int id) {
            var team = await _db.Teams
                .Include(t => t.Players)
                .Include(t => t.Players.Select(p => p.Profile))
                .SingleOrDefaultAsync(t => t.Id == id);

            return Ok(team.Players);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}/players")]
        [HttpPost]
        public async Task<IHttpActionResult> PostTeamPlayer(int id, TeamPlayerBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var player = await _db.Players
                .Include(p => p.Teams)
                .Include(p => p.Profile)
                .Where(p =>
                    p.Profile.FirstName == model.FirstName &&
                    p.Profile.LastName == model.LastName
                )
                .FirstOrDefaultAsync();

            if (player == null) {
                var profile = _db.Profiles.Add(new Profile {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                });

                player = _db.Players.Add(new Player {
                    Profile = profile
                });

                await _db.SaveChangesAsync();
            }

            if (player.Teams.Any(t => t.Id == id)) {
                return BadRequest();
            }

            var team = await _db.Teams.FindAsync(id);
            player.Teams.Add(team);

            await _db.SaveChangesAsync();

            return Ok(player);
        }

        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Team team) {
            _db.Teams.Add(team);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = team.Id }, team);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Team team) {
            team.Id = id;

            _db.Entry(team).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id) {
            var team = new Team { Id = id };

            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}