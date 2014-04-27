using Leaguerly.Api.Extensions;
using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class ManagersController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public ManagersController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> Get() {
            var managers = await _db.Managers.ToListAsync();

            return Ok(managers);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var manager = await _db.Managers.SingleOrDefaultAsync(m => m.Id == id);

            if (manager == null) {
                return NotFound();
            }

            return Ok(manager);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] Manager manager) {
            if (!ModelState.IsValid) {
                return StatusCode(HttpStatusCodeExtensions.UnprocessableEntity);
            }

            var managerExists = await _db.Managers.AnyAsync(m => m.Profile.Email == manager.Profile.Email);
            if (managerExists) {
                return Conflict();
            }

            var existingProfile = await _db.Profiles
                .Where(m => m.Email == manager.Profile.Email)
                .SingleOrDefaultAsync();

            _db.Managers.Add(manager);
            if (existingProfile != null) {
                manager.Profile.Id = existingProfile.Id;
                manager.Profile.UserId = await _db.Users
                    .Where(user => user.Email == manager.Profile.Email)
                    .Select(user => user.Id)
                    .SingleOrDefaultAsync() ?? existingProfile.UserId;

                _db.Entry(manager.Profile).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = manager.Id }, manager);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Manager manager) {
            manager.Id = id;

            _db.Entry(manager).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id) {
            var manager = new Manager { Id = id };

            _db.Managers.Remove(manager);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}