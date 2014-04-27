using System.ComponentModel.DataAnnotations;

namespace Leaguerly.Api.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Phone { get; set; }
        public Address MailingAddress { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public Profile() {
            if (MailingAddress == null) {
                MailingAddress = new Address();
            }
        }
    }
}