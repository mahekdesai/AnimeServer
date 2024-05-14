namespace animeServer.DTO
{
    public class NewVoiceActorDto
    {
        public required string VoiceActorName { get; set; }

        public required IFormFile VoiceActorImage { get; set; }
    }
}
