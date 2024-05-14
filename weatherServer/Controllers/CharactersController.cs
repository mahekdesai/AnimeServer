using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using Microsoft.AspNetCore.Authorization;
using animeServer.DTO;
using Microsoft.Data.SqlClient;

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

        [Authorize]
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
        public async Task<ActionResult<Character>> PostCharacter([FromForm] NewCharacterDto newCharacterDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Read the image data from the uploaded file
            byte[] imageData;
            using (var stream = newCharacterDto.CharacterImage.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            // Construct the SQL query to insert the image into the database
            var query = $"INSERT INTO Character (CharacterName, CharacterImage) VALUES ('{newCharacterDto.CharacterName}', @imageData)";
            await context.Database.ExecuteSqlRawAsync(query, new SqlParameter("@imageData", imageData));

            return Ok();
        }

        [HttpPost("{id}/AddAnimeVoiceactor")]
        public async Task<ActionResult> AddAnimeVoiceactor(int id, [FromForm] NewAnimeVoiceactorDto newAnimeVoiceactorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] animeImageData;
            byte[] voiceActorImageData;
            using (var animeStream = newAnimeVoiceactorDto.AnimeImage.OpenReadStream())
            using (var animeMs = new MemoryStream())
            using (var voiceActorStream = newAnimeVoiceactorDto.VoiceActorImage.OpenReadStream())
            using (var voiceActorMs = new MemoryStream())
            {
                await animeStream.CopyToAsync(animeMs);
                await voiceActorStream.CopyToAsync(voiceActorMs);
                animeImageData = animeMs.ToArray();
                voiceActorImageData = voiceActorMs.ToArray();
            }

            var anime = new Anime
            {
                AnimeName = newAnimeVoiceactorDto.AnimeName,
                AnimeImage = animeImageData
            };
            context.Animes.Add(anime);
            await context.SaveChangesAsync();

            var animeId = anime.AnimeId;

            var voiceActor = new VoiceActor
            {
                VoiceActorName = newAnimeVoiceactorDto.VoiceActorName,
                VoiceActorImage = voiceActorImageData
            };
            context.VoiceActors.Add(voiceActor);
            await context.SaveChangesAsync();

            var voiceActorId = voiceActor.VoiceAactorId;

            var animeVoiceactorCharacter = new AnimeVoiceactorCharacter
            {
                AnimeId = animeId,
                VoiceActorId = voiceActorId,
                CharacterId = id,
            };
            context.AnimeVoiceactorCharacters.Add(animeVoiceactorCharacter);
            await context.SaveChangesAsync();

            return Ok();
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
