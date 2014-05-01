using System;
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [IgnoreDataMember]
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        [IgnoreDataMember]
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        [IgnoreDataMember]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [IgnoreDataMember]
        public int? ResultId { get; set; }
        public GameResult Result { get; set; }
        public bool HasResult { get { return Result != null && Result.Id > 0; } }

        public int DivisionId { get; set; }
    }
}
