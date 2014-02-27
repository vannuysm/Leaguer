using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Leaguerly.Repositories.DataModels
{
    public class League : IDbEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        [IgnoreDataMember]
        public ICollection<Division> Divisions { get; set; }

        public League() {
            if (Divisions == null) {
                Divisions = new Collection<Division>();
            }
        }
    }
}
