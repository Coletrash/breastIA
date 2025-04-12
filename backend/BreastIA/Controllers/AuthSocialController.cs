using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BreastIA.Controllers
{
    [Route("api/auth/social")]
    [ApiController]
    public class AuthSocialController : ControllerBase
    {
        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/social/google/callback" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded) return Unauthorized();

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });

            return Ok(claims);
        }

        [HttpGet("facebook")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/social/facebook/callback" };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("facebook/callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded) return Unauthorized();

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });

            return Ok(claims);
        }

        [HttpGet("instagram")]
        public IActionResult InstagramLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/social/instagram/callback" };
            return Challenge(properties, "Instagram");
        }

        [HttpGet("instagram/callback")]
        public async Task<IActionResult> InstagramCallback()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded) return Unauthorized();

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });

            return Ok(claims);
        }
    }
}
