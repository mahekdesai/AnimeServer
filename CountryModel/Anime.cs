using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[Table("Anime")]
public partial class Anime
{
    [Key]
    public int AnimeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AnimeName { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[] AnimeImage { get; set; } = null!;

    [InverseProperty("Anime")]
    public virtual ICollection<AnimeVoiceactorCharacter> AnimeVoiceactorCharacters { get; set; } = new List<AnimeVoiceactorCharacter>();
}
