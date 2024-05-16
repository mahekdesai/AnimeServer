using animeServer.DTO;
using CountryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace weatherServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<AnimeVoiceactorCharacterUser> userManager, JwtHandler jwtHandler, AnimeSourceContext db) : ControllerBase
    {

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            AnimeVoiceactorCharacterUser? user = await userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            bool success = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!success)
            {
                return Unauthorized("Invalid Password");
            }

            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);
            string jwtString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResult
            {
                Success = true,
                Message = "Login successful",
                Token = jwtString,
            });
        }
    }
}
