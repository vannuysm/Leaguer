using System;

namespace Leaguerly.Api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTime Date { get; set; }
        public Location Location { get; set; }
        public GameResult Result { get; set; }

        public Game() {
            if (HomeTeam == null) {
                HomeTeam = new Team();
            }

            if (AwayTeam == null) {
                AwayTeam = new Team();
            }

            if (Location == null) {
                Location = new Location();
            }

            if (Result == null) {
                Result = new GameResult();
            }
        }
    }
}
