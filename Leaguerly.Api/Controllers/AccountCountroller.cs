using System.Data.Entity;
using System.Linq;
using Leaguerly.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly LeaguerlyDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(LeaguerlyDbContext db, UserManager<IdentityUser> userManager) {
            _db = db;
            _userManager = userManager;
        }

        // GET api/account/current
        [Route("current")]
        public async Task<IHttpActionResult> GetCurrent() {
            var userId = User.Identity.GetUserId();
            var viewProfile = new ViewProfileModel {
                Profile = await _db.Profiles.SingleOrDefaultAsync(c => c.UserId == userId) ??
                    new Profile { UserId = userId },

                ManagerTeams = await _db.Teams
                    .Include(team => team.Division)
                    .Include(team => team.Manager)
                    .Where(team => team.Manager.Profile.UserId == userId)
                    .ToListAsync(),

                PlayerTeams = await _db.Players
                    .Include(player => player.Teams.Select(team => team.Division))
                    .Include(player => player.Teams.Select(team => team.Manager))
                    .Where(player => player.Profile.UserId == userId)
                    .SelectMany(player => player.Teams)
                    .ToListAsync()
            };

            return Ok(viewProfile);
        }

        // GET api/account/roles
        [Authorize(Roles = "Admin")]
        [Route("roles")]
        public async Task<IHttpActionResult> GetRoles(string userName) {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user.Id);

            return Ok(roles);
        }

        // PUT api/account/roles
        [Authorize(Roles = "Admin")]
        [Route("roles")]
        public async Task<IHttpActionResult> AddRoleToUser(AddRoleBindingModel model) {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null) {
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(user.Id, model.Role);
            var errorResult = GetErrorResult(result);

            if (errorResult != null) {
                return errorResult;
            }

            return Ok();
        }

        // POST api/account/register
        [Authorize(Roles = "Admin, Registrar")]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.UserName };

            var result = await _userManager.CreateAsync(user, model.Password);
            var errorResult = GetErrorResult(result);

            if (errorResult != null) {
                return errorResult;
            }

            return Ok();
        }

        // POST api/account/changepassword
        [Route("changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await _userManager.ChangePasswordAsync(
                User.Identity.GetUserId(), model.OldPassword, model.NewPassword
            );
            var errorResult = GetErrorResult(result);

            if (errorResult != null) {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _userManager.Dispose();
            }

            base.Dispose(disposing);
        }

        private IAuthenticationManager Authentication {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result) {
            if (result == null) {
                return InternalServerError();
            }

            if (result.Succeeded) {
                return null;
            }

            if (result.Errors != null) {
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid) {
                // No ModelState errors are available to send, so just return an empty BadRequest.
                return BadRequest();
            }

            return BadRequest(ModelState);
        }
    }
}
