namespace animeServer.DTO
{
    public class VoiceActorCount
    {
        public required string VoiceActorName { get; set; }
        public int VoiceActorId { get; set; }
        public int CharacterCount { get; set; }
        public int AnimeCount { get; set; }
    }
}
