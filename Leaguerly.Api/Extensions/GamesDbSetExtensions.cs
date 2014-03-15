using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Linq;

public static class GamesDbSetExtensions
{
    public static IQueryable<Game> WithDetails(this IDbSet<Game> games) {
        return games
            .Include(game => game.HomeTeam)
            .Include(game => game.AwayTeam)
            .Include(game => game.Location)
            .Include(game => game.Result)
            .Include(game => game.Result.Goals)
            .Include(game => game.Result.Goals.Select(goal => goal.Player))
            .Include(game => game.Result.Goals.Select(goal => goal.Player.Teams));
    }
}