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
using Microsoft.Data.SqlClient;

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
        public async Task<ActionResult<VoiceActor>> PostVoiceActor([FromForm] NewVoiceActorDto newVoiceActorDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Read the image data from the uploaded file
            byte[] imageData;
            using (var stream = newVoiceActorDto.VoiceActorImage.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            // Construct the SQL query to insert the image into the database
            var query = $"INSERT INTO VoiceActor (VoiceActorName, VoiceActorImage) VALUES ('{newVoiceActorDto.VoiceActorName}', @imageData)";
            await context.Database.ExecuteSqlRawAsync(query, new SqlParameter("@imageData", imageData));

            return Ok();
        }

        [HttpPost("{id}/AddAnimeCharacter")]
        public async Task<ActionResult> AddAnimeCharacter(int id, [FromForm] NewAnimeCharacterDto newAnimeCharacterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] animeImageData;
            byte[] characterImageData;
            using (var animeStream = newAnimeCharacterDto.AnimeImage.OpenReadStream())
            using (var animeMs = new MemoryStream())
            using (var characterStream = newAnimeCharacterDto.CharacterImage.OpenReadStream())
            using (var characterMs = new MemoryStream())
            {
                await animeStream.CopyToAsync(animeMs);
                await characterStream.CopyToAsync(characterMs);
                animeImageData = animeMs.ToArray();
                characterImageData = characterMs.ToArray();
            }

            var anime = new Anime
            {
                AnimeName = newAnimeCharacterDto.AnimeName,
                AnimeImage = animeImageData
            };
            context.Animes.Add(anime);
            await context.SaveChangesAsync();

            var animeId = anime.AnimeId;

            var character = new Character
            {
                CharacterName = newAnimeCharacterDto.CharacterName,
                CharacterImage = characterImageData
            };
            context.Characters.Add(character);
            await context.SaveChangesAsync();

            var characterId = character.CharacterId;

            var animeVoiceactorCharacter = new AnimeVoiceactorCharacter
            {
                AnimeId = animeId,
                VoiceActorId = id,
                CharacterId = characterId,
            };
            context.AnimeVoiceactorCharacters.Add(animeVoiceactorCharacter);
            await context.SaveChangesAsync();

            return Ok();
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

        [Authorize]
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


