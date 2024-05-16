using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CountryModel;
using animeServer.DTO;

namespace animeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AnimeSourceContext _db;
        private readonly UserManager<AnimeVoiceactorCharacterUser> _userManager;

        public SeedController(AnimeSourceContext db, UserManager<AnimeVoiceactorCharacterUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost("User")]
        public async Task<ActionResult> SeedUser([FromBody] Register model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid user data.");
            }

            var user = new AnimeVoiceactorCharacterUser
            {
                UserName = model.Email,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await _db.SaveChangesAsync();

            return Ok("User created successfully.");
        }
    }
}
