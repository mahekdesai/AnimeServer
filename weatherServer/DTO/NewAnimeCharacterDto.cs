namespace animeServer.DTO
{
    public class NewAnimeCharacterDto
    {
        public required string AnimeName { get; set; }

        public required IFormFile AnimeImage { get; set; }

        public required string CharacterName { get; set; }

        public required IFormFile CharacterImage { get; set; }
    }
}
