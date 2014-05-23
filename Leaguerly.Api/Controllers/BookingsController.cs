using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [RoutePrefix("api/bookings")]
    public class BookingsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public BookingsController(LeaguerlyDbContext db) {
            _db = db;
        }

        [Route("{id}", Name = "GetSingleBooking")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id) {
            var booking = await _db.Bookings
                .Include(b => b.Player)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (booking == null) {
                return NotFound();
            }

            return Ok(booking);
        }

        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(Booking booking) {
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetSingleBooking", new { id = booking.Id }, booking);
        }

        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Booking booking) {
            booking.Id = id;

            _db.Entry(booking).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
