using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leaguerly.Api.Models
{
    public class AddGoalBindingModel
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int Count { get; set; }
    }
}