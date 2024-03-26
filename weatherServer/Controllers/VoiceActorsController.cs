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
    public class VoiceActorsController(AnimeSourceContext context) : ControllerBase
    {
        // GET: api/VoiceActors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoiceActor>>> GetVoiceActors()
        {
            return await context.VoiceActors.ToListAsync();
        }

        [HttpGet("GetCount")]
        public async Task<ActionResult<IEnumerable<VoiceActorCount>>> GetCharacterCount()
        {
            IQueryable<VoiceActorCount> x = context.VoiceActors.Select(c =>
            new VoiceActorCount
            {
                VoiceActorName = c.VoiceActorName,
                VoiceActorId = c.VoiceAactorId,
                CharacterCount = c.AnimeVoiceactorCharacters
                                .Select(avc => avc.CharacterId)
                                .Distinct()
                                .Count(),
                AnimeCount = c.AnimeVoiceactorCharacters
                                .Select(avc => avc.AnimeId)
                                .Distinct()
                                .Count(),
            });
            return await x.ToListAsync();
        }
    }
}

