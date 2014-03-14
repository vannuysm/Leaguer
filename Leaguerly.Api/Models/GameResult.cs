using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Leaguerly.Api.Models
{
    public class GameResult
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public bool WasForfeited { get; set; }
        public int? ForfeitingTeamId { get; set; }
        public bool IncludeInStandings { get; set; }
        public ICollection<Goal> Goals { get; set; }

        public GameResult() {
            if (Goals == null)
                Goals = new Collection<Goal>();
        }
    }
}
