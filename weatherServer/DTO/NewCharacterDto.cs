namespace animeServer.DTO
{
    public class NewCharacterDto
    {
        public required string CharacterName { get; set; }

        public required IFormFile CharacterImage { get; set; }
    }
}
