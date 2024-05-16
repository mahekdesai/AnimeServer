using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using animeServer.DTO;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

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

        [Authorize]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Anime>> GetAnime(int id)
        {
            var country = await context.Animes.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        [Authorize]
        [HttpGet("VoiceactorsCharacters/{animeId}")]
        public async Task<ActionResult<List<object>>> GetVoiceactorsCharacters(int animeId)
        {
            var results = await context.AnimeVoiceactorCharacters
                .Where(avc => avc.AnimeId == animeId)
                .Select(avc => new {
                    AnimeId = avc.AnimeId,
                    Character = new
                    {
                        CharacterId = avc.Character.CharacterId,
                        CharacterName = avc.Character.CharacterName,
                        CharacterImage = avc.Character.CharacterImage // Ensure this data is encoded if binary
                    },
                    VoiceActor = new
                    {
                        VoiceActorId = avc.VoiceActor.VoiceAactorId,
                        VoiceActorName = avc.VoiceActor.VoiceActorName,
                        VoiceActorImage = avc.VoiceActor.VoiceActorImage // Ensure this data is encoded if binary
                    }
                })
                .ToListAsync();

            return Ok(results);
        }

        // PUT: api/Animes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnime(int id, Anime anime)
        {
            if (id != anime.AnimeId)
            {
                return BadRequest();
            }

            context.Entry(anime).State = EntityState.Modified;

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


        // POST: api/Animes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Anime>> PostAnime([FromForm] NewAnimeDto newAnimeDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Read the image data from the uploaded file
            byte[] imageData;
            using (var stream = newAnimeDto.AnimeImage.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            var query = $"INSERT INTO Anime (AnimeName, AnimeImage) VALUES ('{newAnimeDto.AnimeName}', @imageData)";
            await context.Database.ExecuteSqlRawAsync(query, new SqlParameter("@imageData", imageData));

            return Ok();
        }

        [HttpPost("{id}/AddVoiceactorCharacter")]
        public async Task<ActionResult> AddVoiceactorCharacter(int id, [FromForm] NewVoiceactorCharacterDto newVoiceactorCharacterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] characterImageData;
            byte[] voiceActorImageData;
            using (var characterStream = newVoiceactorCharacterDto.CharacterImage.OpenReadStream())
            using (var characterMs = new MemoryStream())
            using (var voiceActorStream = newVoiceactorCharacterDto.VoiceActorImage.OpenReadStream())
            using (var voiceActorMs = new MemoryStream())
            {
                await characterStream.CopyToAsync(characterMs);
                await voiceActorStream.CopyToAsync(voiceActorMs);
                characterImageData = characterMs.ToArray();
                voiceActorImageData = voiceActorMs.ToArray();
            }

            var character = new Character
            {
                CharacterName = newVoiceactorCharacterDto.CharacterName,
                CharacterImage = characterImageData
            };
            context.Characters.Add(character);
            await context.SaveChangesAsync();

            var characterId = character.CharacterId;

            var voiceActor = new VoiceActor
            {
                VoiceActorName = newVoiceactorCharacterDto.VoiceActorName,
                VoiceActorImage = voiceActorImageData
            };
            context.VoiceActors.Add(voiceActor);
            await context.SaveChangesAsync();

            var voiceActorId = voiceActor.VoiceAactorId;

            var animeVoiceactorCharacter = new AnimeVoiceactorCharacter
            {
                AnimeId = id,
                VoiceActorId = voiceActorId,
                CharacterId = characterId
            };
            context.AnimeVoiceactorCharacters.Add(animeVoiceactorCharacter);
            await context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Animes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnime(int id)
        {
            var country = await context.Animes.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            context.Animes.Remove(country);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return context.Animes.Any(e => e.AnimeId == id);
        }
    }
}