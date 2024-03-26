using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryModel.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anime",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AnimeImage = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__AF82112AB7EE1730", x => x.AnimeId);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CharacterImage = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__757BC9A0CB74E8F6", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "VoiceActor",
                columns: table => new
                {
                    VoiceAactorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoiceActorName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    VoiceActorImage = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__B015C2913D9BDA38", x => x.VoiceAactorId);
                });

            migrationBuilder.CreateTable(
                name: "AnimeVoiceactorCharacter",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    VoiceActorId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeVoiceactorCharacter", x => new { x.AnimeId, x.VoiceActorId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_AnimeVoiceactorCharacter_Anime",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "AnimeId");
                    table.ForeignKey(
                        name: "FK_AnimeVoiceactorCharacter_Character",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "CharacterId");
                    table.ForeignKey(
                        name: "FK_AnimeVoiceactorCharacter_VoiceActor",
                        column: x => x.VoiceActorId,
                        principalTable: "VoiceActor",
                        principalColumn: "VoiceAactorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeVoiceactorCharacter_CharacterId",
                table: "AnimeVoiceactorCharacter",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeVoiceactorCharacter_VoiceActorId",
                table: "AnimeVoiceactorCharacter",
                column: "VoiceActorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeVoiceactorCharacter");

            migrationBuilder.DropTable(
                name: "Anime");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "VoiceActor");
        }
    }
}
