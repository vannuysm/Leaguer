
using System.Runtime.Serialization;

namespace Leaguerly.Api.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
