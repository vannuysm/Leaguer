using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Division Division { get; set; }
        public Manager Manager { get; set; }

        [IgnoreDataMember]
        public ICollection<Player> Players { get; set; }

        public Team() {
            if (Division == null) {
                Division = new Division();
            }

            if (Manager == null) {
                Manager = new Manager();
            }

            if (Players == null) {
                Players = new Collection<Player>();
            }
        }
    }
}
