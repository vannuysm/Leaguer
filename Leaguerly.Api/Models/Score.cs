using System.Linq;

namespace Leaguerly.Api.Models
{
    public class Score
    {
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public bool IsTie { get { return HomeTeamScore == AwayTeamScore; } }
        public int WinningTeamId { get; set; }
        public int LosingTeamId { get; set; }

        private Score() { }

        public static Score GetScore(Game game) {
            var homeTeamPlayers = game.HomeTeam.Players.Select(player => player.Id);
            var awayTeamPlayers = game.AwayTeam.Players.Select(player => player.Id);

            var score = new Score {
                HomeTeamScore = game.Result.Goals
                    .Where(goal => homeTeamPlayers.Contains(goal.Player.Id))
                    .Sum(goal => goal.Count),
                AwayTeamScore = game.Result.Goals
                    .Where(goal => awayTeamPlayers.Contains(goal.Player.Id))
                    .Sum(goal => goal.Count)
            };

            if (score.IsTie) {
                return score;
            }

            score.WinningTeamId = score.HomeTeamScore > score.AwayTeamScore
                ? game.HomeTeam.Id
                : game.AwayTeam.Id;

            score.LosingTeamId = score.HomeTeamScore < score.AwayTeamScore
                ? game.HomeTeam.Id
                : game.AwayTeam.Id;

            return score;
        }
    }
}