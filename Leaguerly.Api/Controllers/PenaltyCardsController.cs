using Leaguerly.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/penaltycards")]
    public class PenaltyCardsController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get() {
            var penaltyCards = PenaltyCardDefinitions.FindAll();

            return Ok(penaltyCards.Select(p => new { Name = p.Key, Details = p.Value }));
        }
    }
}
