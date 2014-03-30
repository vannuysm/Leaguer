using System.ComponentModel.DataAnnotations;
namespace Leaguerly.Api.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address MailingAddress { get; set; }

        public Contact() {
            if (MailingAddress == null) {
                MailingAddress = new Address();
            }
        }
    }
}