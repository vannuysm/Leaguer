using Leaguerly.Api.Models;
using Leaguerly.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Cors;

namespace Leaguerly.Api
{
    public partial class Startup
    {
        public static string PublicClientId { get; private set; }
        public static Func<UserManager<IdentityUser>> UserManagerFactory { get; set; }
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new UserManager<IdentityUser>(
                new UserStore<IdentityUser>(new LeaguerlyDbContext())
            );

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/auth/token"),
                AuthorizeEndpointPath = new PathString("/auth/authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public void ConfigureAuth(IAppBuilder app) {
            var tokenCorsPolicy = new CorsPolicy {
                AllowAnyOrigin = true,
                AllowAnyHeader = true,
                AllowAnyMethod = true
            };

            var corsOptions = new CorsOptions {
                PolicyProvider = new CorsPolicyProvider {
                    PolicyResolver = request => Task.FromResult(
                        request.Path.ToString().StartsWith("/auth/token") ? tokenCorsPolicy : null
                    )
                }
            };
            app.UseCors(corsOptions);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
