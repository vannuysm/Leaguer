using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Repositories.DataModels
{
    public class Team : IDbEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DivisionId { get; set; }

        [IgnoreDataMember]
        public ICollection<Player> Players { get; set; }

        public Team() {
            if (Players == null) {
                Players = new Collection<Player>();
            }
        }
    }
}
