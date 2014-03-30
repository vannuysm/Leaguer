using Leaguerly.Api.Extensions;
using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

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

            var managerExists = await _db.Managers.AnyAsync(m => m.Contact.Email == manager.Contact.Email);
            if (managerExists) {
                return Conflict();
            }

            manager.Contact.Id = await _db.Contacts
                .Where(m => m.Email == manager.Contact.Email)
                .Select(m => m.Id)
                .SingleOrDefaultAsync();
            
            _db.Managers.Add(manager);
            if (manager.Contact.Id > 0) {
                _db.Entry(manager.Contact).State = EntityState.Modified;
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