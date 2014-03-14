using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Leaguerly.Api.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Field> Fields { get; set; }

        public Location() {
            if (Fields == null) {
                Fields = new Collection<Field>();
            }
        }
    }
}
