namespace animeServer.DTO
{
    public class AnimeCount
    {
        public required string AnimeName { get; set; }
        public int AnimeId { get; set; }
        public int CharacterCount { get; set; }
        public int VoiceActorCount { get; set; }
    }
}
