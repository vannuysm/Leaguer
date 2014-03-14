using System;

namespace Leaguerly.Api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime Date { get; set; }
        public int? GameResultId { get; set; }
    }
}
