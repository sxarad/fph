using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise.WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }

        public void OnGet()
        {            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Verify the credential
            if (!string.IsNullOrWhiteSpace(Credential.UserName) && !string.IsNullOrWhiteSpace(Credential.Password))
            {
                // Creating the security context
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, Credential.UserName),
                    new Claim("NormalUser", "true"),
                    new Claim("UserName", Credential.UserName),
                    new Claim("Password", Credential.Password),
                };
                var identity = new ClaimsIdentity(claims, "AuthCookie");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("AuthCookie", claimsPrincipal, authProperties);

                return RedirectToPage(HttpContext.Request.Query["ReturnUrl"].ToString() ?? "/Index");
            }

            return Page();
        }
    }    
}
