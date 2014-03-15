using System.Collections.Generic;
using System.Linq;

namespace Leaguerly.Api.Models
{
    public class Standing
    {
        public Team Team { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public int Forfeits { get; set; }
        public int GamesPlayed { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get { return (3 * Wins) + (Ties) - (Forfeits); } }
        public int GoalDifferential { get { return GoalsFor - GoalsAgainst; } }

        private Standing() { }

        public static List<Standing> CalculateStandings(List<Game> games) {
            var standings = GetStandings(games);

            return Sort(standings, games);
        }

        private static List<Standing> GetStandings(List<Game> games) {
            var teams = games.Select(game => game.HomeTeam)
                .Concat(games.Select(game => game.AwayTeam))
                .Distinct();

            var standings = (from team in teams
                let teamGames = games.Where(game => game.HomeTeam.Id == team.Id || game.AwayTeam.Id == team.Id).ToList()
                let scores = teamGames.Select(Score.GetScore).ToList()
                select new Standing {
                    Team = team,
                    Wins = scores.Count(score => score.WinningTeamId == team.Id),
                    Losses = scores.Count(score => score.LosingTeamId == team.Id),
                    Ties = scores.Count(score => score.IsTie),
                    Forfeits = teamGames.Count(game => game.Result.WasForfeited && game.Result.ForfeitingTeamId == team.Id),
                    GamesPlayed = teamGames.Count,
                    GoalsFor = teamGames.Sum(game => {
                        var score = Score.GetScore(game);
                        return game.HomeTeam.Id == team.Id ? score.HomeTeamScore : score.AwayTeamScore;
                    }),
                    GoalsAgainst = teamGames.Sum(game => {
                        var score = Score.GetScore(game);
                        return game.HomeTeam.Id == team.Id ? score.AwayTeamScore : score.HomeTeamScore;
                    })
                }).ToList();

            return standings;
        }

        private static List<Standing> Sort(List<Standing> standings, List<Game> games) {
            var sortedStandings = new List<Standing>();

            var pointGroups = standings
                .OrderByDescending(standing => standing.Points)
                .GroupBy(standing => standing.Points);

            foreach (var pointGroup in pointGroups) {
                if (pointGroup.Count() == 1) {
                    sortedStandings.Add(pointGroup.Single());
                    continue;
                }

                var headToHeadTeams = pointGroup.Select(standing => standing.Team.Id).ToList();
                var headToHeadStandings = GetStandings(
                    games.Where(game =>
                        headToHeadTeams.Contains(game.HomeTeam.Id) &&
                        headToHeadTeams.Contains(game.AwayTeam.Id)
                    ).ToList()
                )
                .OrderByDescending(standing => standing.Points)
                .ThenByDescending(standing => standing.GoalDifferential)
                .ThenByDescending(standing =>
                    standings.Single(s => s.Team.Id == standing.Team.Id).GoalDifferential
                );

                sortedStandings.AddRange(
                    headToHeadStandings.Select(headToHeadStanding =>
                        standings.Single(standing => standing.Team.Id == headToHeadStanding.Team.Id)
                    )
                );
            }

            return sortedStandings;
        }
    }
}