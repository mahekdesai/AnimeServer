using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[Table("VoiceActor")]
public partial class VoiceActor
{
    [Key]
    public int VoiceAactorId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string VoiceActorName { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[] VoiceActorImage { get; set; } = null!;

    [InverseProperty("VoiceActor")]
    public virtual ICollection<AnimeVoiceactorCharacter> AnimeVoiceactorCharacters { get; set; } = new List<AnimeVoiceactorCharacter>();
}
