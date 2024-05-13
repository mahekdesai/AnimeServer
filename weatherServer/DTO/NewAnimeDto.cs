namespace animeServer.DTO
{
    public class NewAnimeDto
    {
        public required string AnimeName { get; set; }

        public required IFormFile AnimeImage { get; set; }
    }
}
