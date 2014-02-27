using Leaguerly.Repositories;
using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class PlayersController : ApiController
    {
        private readonly IRepository<Player, int> _playerRepo;

        public PlayersController(IRepository<Player, int> playerRepo) {
            _playerRepo = playerRepo;
        }

        public IEnumerable<Player> Get() {
            var players = _playerRepo.All();
            return players;
        }

        public Player Get(int id) {
            var player = _playerRepo.Get(id);
            return player;
        }

        public void Post([FromBody] Player player) {
            _playerRepo.Create(player);
        }

        public void Put(int id, [FromBody] Player player) {
            _playerRepo.Update(player);
        }

        public void Delete(int id) {
            _playerRepo.Delete(id);
        }
    }
}
