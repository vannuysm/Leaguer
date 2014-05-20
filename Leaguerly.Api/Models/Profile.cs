using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Phone { get; set; }
        public Address MailingAddress { get; set; }

        public string Name { get { return FirstName + " " + LastName; } }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public Profile() {
            if (MailingAddress == null) {
                MailingAddress = new Address();
            }
        }
    }
}