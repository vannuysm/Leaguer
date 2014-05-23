using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Linq;

public static class GamesDbSetExtensions
{
    public static IQueryable<Game> WithDetails(this IDbSet<Game> games) {
        return games
            .Include(game => game.HomeTeam)
            .Include(game => game.HomeTeam.Players)
            .Include(game => game.AwayTeam)
            .Include(game => game.AwayTeam.Players)
            .Include(game => game.Location)
            .Include(game => game.Goals)
            .Include(game => game.Goals.Select(goal => goal.Player))
            .Include(game => game.Goals.Select(goal => goal.Player.Profile))
            .Include(game => game.Goals.Select(goal => goal.Player.Teams))
            .Include(game => game.Bookings)
            .Include(game => game.Bookings.Select(booking => booking.Player))
            .Include(game => game.Bookings.Select(booking => booking.Player.Profile));
    }
}