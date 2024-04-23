using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using animeServer.DTO;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
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

   
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoiceActor(int id, VoiceActor voiceActor)
        {
            if (id != voiceActor.VoiceAactorId)
            {
                return BadRequest();
            }

            context.Entry(voiceActor).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


      
        [HttpPost]
        public async Task<ActionResult<VoiceActor>> PostAnime(VoiceActor voiceActor)
        {
            context.VoiceActors.Add(voiceActor);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = voiceActor.VoiceAactorId }, voiceActor);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoiceActor(int id)
        {
            var country = await context.VoiceActors.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            context.VoiceActors.Remove(country);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return context.VoiceActors.Any(e => e.VoiceAactorId == id);
        }

        [HttpGet("AnimesCharacters/{voiceActorId}")]
        public async Task<ActionResult<List<object>>> GetAnimesCharacters(int voiceActorId)
        {
            var results = await context.AnimeVoiceactorCharacters
                .Where(avc => avc.VoiceActorId == voiceActorId)
                .Select(avc => new {
                    VoiceAactorId = avc.VoiceActorId,
                    Character = new
                    {
                        CharacterId = avc.Character.CharacterId,
                        CharacterName = avc.Character.CharacterName,
                        CharacterImage = avc.Character.CharacterImage // Ensure this data is encoded if binary
                    },
                    Anime = new
                    {
                        AnimeId = avc.Anime.AnimeId,
                        AnimeName = avc.Anime.AnimeName,
                        AnimeImage = avc.Anime.AnimeImage // Ensure this data is encoded if binary
                    }
                })
                .ToListAsync();

            return Ok(results);
        }
    }
}


