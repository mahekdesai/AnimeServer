namespace animeServer.DTO
{
    public class NewAnimeVoiceactorDto
    {
        public required string AnimeName { get; set; }

        public required IFormFile AnimeImage { get; set; }

        public required string VoiceActorName { get; set; }

        public required IFormFile VoiceActorImage { get; set; }
    }
}
