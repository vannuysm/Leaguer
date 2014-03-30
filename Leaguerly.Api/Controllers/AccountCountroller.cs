using Leaguerly.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        public UserManager<IdentityUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public AccountController() : this(Startup.UserManagerFactory(),
            Startup.OAuthOptions.AccessTokenFormat
        ) {}

        public AccountController(UserManager<IdentityUser> userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat
        ) {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        // GET api/account/roles
        [Authorize(Roles = "Admin")]
        [Route("roles")]
        public async Task<IHttpActionResult> GetRoles(string userName) {
            var user = await UserManager.FindByNameAsync(userName);

            if (user == null) {
                return NotFound();
            }

            var roles = await UserManager.GetRolesAsync(user.Id);

            return Ok(roles);
        }

        // PUT api/account/roles
        [Authorize(Roles = "Admin")]
        [Route("roles")]
        public async Task<IHttpActionResult> AddRoleToUser(AddRoleBindingModel model) {
            var user = await UserManager.FindByNameAsync(model.UserName);

            if (user == null) {
                return NotFound();
            }

            var result = await UserManager.AddToRoleAsync(user.Id, model.Role);
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

            var result = await UserManager.CreateAsync(user, model.Password);
            var errorResult = GetErrorResult(result);

            if (errorResult != null) {
                return errorResult;
            }

            return Ok();
        }

        // POST api/account/logout
        [Route("logout")]
        public IHttpActionResult Logout() {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/account/changepassword
        [Route("changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await UserManager.ChangePasswordAsync(
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
                UserManager.Dispose();
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
