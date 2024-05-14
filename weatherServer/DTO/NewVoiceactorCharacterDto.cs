namespace animeServer.DTO
{
    public class NewVoiceactorCharacterDto
    {
        public required string CharacterName { get; set; }

        public required IFormFile CharacterImage { get; set; }

        public required string VoiceActorName { get; set; }

        public required IFormFile VoiceActorImage { get; set; }
    }
}
