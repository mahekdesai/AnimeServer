using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[Table("Character")]
public partial class Character
{
    [Key]
    public int CharacterId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CharacterName { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[] CharacterImage { get; set; } = null!;

    [InverseProperty("Character")]
    public virtual ICollection<AnimeVoiceactorCharacter> AnimeVoiceactorCharacters { get; set; } = new List<AnimeVoiceactorCharacter>();
}
