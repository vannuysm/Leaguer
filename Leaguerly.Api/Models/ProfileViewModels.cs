using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Leaguerly.Api.Models
{
    public class ViewProfileModel
    {
        public Profile Profile { get; set; }
        public ICollection<Team> ManagerTeams { get; set; }
        public ICollection<Team> PlayerTeams { get; set; }

        public ViewProfileModel() {
            if (Profile == null) {
                Profile = new Profile();
            }

            if (ManagerTeams == null) {
                ManagerTeams = new Collection<Team>();
            }

            if (PlayerTeams == null) {
                PlayerTeams = new Collection<Team>();
            }
        }
    }
}