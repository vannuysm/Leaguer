using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int LeagueId { get; set; }

        [IgnoreDataMember]
        public ICollection<Team> Teams { get; set; }

        [IgnoreDataMember]
        public ICollection<Game> Games { get; set; }

        public Division() {
            if (Teams == null) {
                Teams = new Collection<Team>();
            }

            if (Games == null) {
                Games = new Collection<Game>();
            }
        }
    }
}
