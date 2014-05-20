using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Player
    {
        public int Id { get; set; }
        public Profile Profile { get; set; }

        [IgnoreDataMember]
        public ICollection<Team> Teams { get; set; }

        public Player() {
            if (Teams == null) {
                Teams = new Collection<Team>();
            }
        }
    }
}
