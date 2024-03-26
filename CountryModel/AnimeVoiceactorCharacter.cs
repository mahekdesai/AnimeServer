using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[PrimaryKey("AnimeId", "VoiceActorId", "CharacterId")]
[Table("AnimeVoiceactorCharacter")]
public partial class AnimeVoiceactorCharacter
{
    [Key]
    public int AnimeId { get; set; }

    [Key]
    public int VoiceActorId { get; set; }

    [Key]
    public int CharacterId { get; set; }

    [ForeignKey("AnimeId")]
    [InverseProperty("AnimeVoiceactorCharacters")]
    public virtual Anime Anime { get; set; } = null!;

    [ForeignKey("CharacterId")]
    [InverseProperty("AnimeVoiceactorCharacters")]
    public virtual Character Character { get; set; } = null!;

    [ForeignKey("VoiceActorId")]
    [InverseProperty("AnimeVoiceactorCharacters")]
    public virtual VoiceActor VoiceActor { get; set; } = null!;
}
