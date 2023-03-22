using Domain;
using Exercise.Core;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Exercise.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomAuthenticationService _customAuthenticationService;

        public AuthController(ICustomAuthenticationService customAuthenticationService)
        {
            _customAuthenticationService = customAuthenticationService;
        }

        [HttpPost]
        public IActionResult Authenticate(Credential credential)
        {
            if (!string.IsNullOrWhiteSpace(credential.UserName) && !string.IsNullOrWhiteSpace(credential.Password))
            {
                var jwt = _customAuthenticationService.Authenticate(credential.UserName, credential.Password);
                return Ok(new
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(jwt),
                    expires_at = jwt.ValidTo
                });
            }

            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint.");
            return Unauthorized(ModelState);
        }
    }
}
