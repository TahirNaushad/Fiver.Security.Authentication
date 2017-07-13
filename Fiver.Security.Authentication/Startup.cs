using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fiver.Security.Authentication
{
    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AccessDeniedPath = new PathString("/Security/Access"),
                AuthenticationScheme = "FiverSecurityCookie",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ClaimsIssuer = "Fiver Security",
                //CookieDomain = "",
                CookieHttpOnly = true,
                CookieName = ".Fiver.Security.Cookie",
                CookiePath = "/",
                CookieSecure = CookieSecurePolicy.SameAsRequest,
                Events = new CookieAuthenticationEvents
                {
                    OnSignedIn = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                          "OnSignedIn", context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnSigningOut = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                          "OnSigningOut", context.HttpContext.User.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnValidatePrincipal = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                          "OnValidatePrincipal", context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    },
                },
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
                LoginPath = new PathString("/Security/Login"),
                ReturnUrlParameter = "RequestPath",
                SlidingExpiration = true,
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
