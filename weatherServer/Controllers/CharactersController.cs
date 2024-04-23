using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;

namespace animeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController(AnimeSourceContext context) : ControllerBase
    {
 

        // GET: api/Characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            return await context.Characters.ToListAsync();
        }

        // GET: api/Characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(int id)
        {
            var character = await context.Characters.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        [HttpGet("AnimesVoiceactors/{characterId}")]
        public async Task<ActionResult<List<object>>> GetAnimesVoiceactors(int characterId)
        {
            var results = await context.AnimeVoiceactorCharacters
                .Where(avc => avc.CharacterId == characterId)
                .Select(avc => new {
                    CharacterId = avc.CharacterId,
                    VoiceActor = new
                    {
                        VoiceActorId = avc.VoiceActor.VoiceAactorId,
                        VoiceActorName = avc.VoiceActor.VoiceActorName,
                        VoiceActorImage = avc.VoiceActor.VoiceActorImage // Ensure this data is encoded if binary
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

        // PUT: api/Characters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, Character character)
        {
            if (id != character.CharacterId)
            {
                return BadRequest();
            }

            context.Entry(character).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
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

        // POST: api/Characters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Character>> PostCharacter(Character character)
        {
            context.Characters.Add(character);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter", new { id = character.CharacterId }, character);
        }

        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            context.Characters.Remove(character);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacterExists(int id)
        {
            return context.Characters.Any(e => e.CharacterId == id);
        }
    }
}
