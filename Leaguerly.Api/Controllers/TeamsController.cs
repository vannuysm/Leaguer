using Leaguerly.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<IEnumerable<Team>> Get() {
            var teams = await _db.Teams.ToListAsync();
            return teams;
        }

        public async Task<Team> Get(int id) {
            var team = await _db.Teams.FindAsync(id);
            return team;
        }

        public async Task Post([FromBody] Team team) {
            _db.Teams.Add(team);
            await _db.SaveChangesAsync();
        }

        public async Task Put(int id, [FromBody] Team team) {
            team.Id = id;

            _db.Entry(team).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var team = new Team { Id = id };

            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
        }
    }
}