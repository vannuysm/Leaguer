using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Repositories.DataModels
{
    public class Player : IDbEntity<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [IgnoreDataMember]
        public ICollection<Team> Teams { get; set; }

        public Player() {
            if (Teams == null) {
                Teams = new Collection<Team>();
            }
        }
    }
}
