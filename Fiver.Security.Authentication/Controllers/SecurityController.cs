using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fiver.Security.Authentication.Models.Security;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authentication;

namespace Fiver.Security.Authentication.Controllers
{
    public class SecurityController : Controller
    {
        public IActionResult Login(string requestPath)
        {
            ViewBag.RequestPath = requestPath ?? "/";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (!IsAuthentic(inputModel.Username, inputModel.Password))
                return View();
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sean Connery"),
                new Claim(ClaimTypes.Email, inputModel.Username)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                    scheme: "FiverSecurityScheme",
                    principal: principal,
                    properties: new AuthenticationProperties
                    {
                        //IsPersistent = true,
                        //ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    });

            return Redirect(inputModel.RequestPath ?? "/");
            //return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                    scheme: "FiverSecurityScheme");

            return RedirectToAction("Login");
        }

        public IActionResult Access()
        {
            return View();
        }

        #region " Private "

        private bool IsAuthentic(string username, string password)
        {
            return (username == "james" && password == "bond");
        }

        #endregion
    }
}
