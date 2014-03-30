using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public Contact Contact { get; set; }

        [IgnoreDataMember]
        public ICollection<Team> Teams { get; set; }

        public Manager() {
            if (Teams == null) {
                Teams = new Collection<Team>();
            }

            if (Contact == null) {
                Contact = new Contact();
            }
        }
    }
}