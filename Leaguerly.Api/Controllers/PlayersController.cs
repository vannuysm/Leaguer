using Leaguerly.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class PlayersController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public PlayersController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IEnumerable<Player>> Get() {
            var players = await _db.Players.ToListAsync();
            return players;
        }

        public async Task<Player> Get(int id) {
            var player = await _db.Players.FindAsync(id);
            return player;
        }

        public async Task Post([FromBody] Player player) {
            _db.Players.Add(player);
            await _db.SaveChangesAsync();
        }

        public async Task Put(int id, [FromBody] Player player) {
            player.Id = id;

            _db.Entry(player).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var player = new Player { Id = id };

            _db.Players.Remove(player);
            await _db.SaveChangesAsync();
        }
    }
}