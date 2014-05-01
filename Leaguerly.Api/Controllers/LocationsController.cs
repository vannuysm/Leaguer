using Leaguerly.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class LocationsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public LocationsController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> GetLocations() {
            var locations = await _db.Locations.ToListAsync();

            return Ok(locations);
        }

        public async Task<IHttpActionResult> GetLocation(int id) {
            var location = await _db.Locations.SingleOrDefaultAsync(g => g.Id == id);

            if (location == null) {
                return NotFound();
            }

            return Ok(location);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] Location location) {
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = location.Id }, location);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Location location) {
            location.Id = id;

            _db.Entry(location).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id) {
            var location = new Location { Id = id };

            _db.Locations.Remove(location);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
