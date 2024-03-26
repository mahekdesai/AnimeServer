using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using animeServer.DTO;

namespace animeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimesController(AnimeSourceContext context) : ControllerBase
    {
        // GET: api/Animes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes()
        {
            return await context.Animes.ToListAsync();
        }

        [HttpGet("GetCount")]
        public async Task<ActionResult<IEnumerable<AnimeCount>>> GetCharacterCount()
        {
            IQueryable<AnimeCount> x = context.Animes.Select(c =>
            new AnimeCount
            {
                AnimeName = c.AnimeName,
                AnimeId = c.AnimeId,
                CharacterCount = c.AnimeVoiceactorCharacters
                                .Select(avc => avc.CharacterId)
                                .Distinct()
                                .Count(),
                VoiceActorCount = c.AnimeVoiceactorCharacters
                                .Select(avc => avc.VoiceActorId)
                                .Distinct()
                                .Count(),
            });
            return await x.ToListAsync();
        }
    }
}
