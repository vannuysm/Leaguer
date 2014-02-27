using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Repositories.DataModels
{
    public class Division : IDbEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int LeagueId { get; set; }

        [IgnoreDataMember]
        public ICollection<Team> Teams { get; set; }

        public Division() {
            if (Teams == null) {
                Teams = new Collection<Team>();
            }
        }
    }
}
