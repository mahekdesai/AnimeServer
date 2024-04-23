using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CountryModel;

public partial class AnimeSourceContext : IdentityDbContext<AnimeVoiceactorCharacterUser>
{
    public AnimeSourceContext()
    {
    }

    public AnimeSourceContext(DbContextOptions<AnimeSourceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anime> Animes { get; set; }

    public virtual DbSet<AnimeVoiceactorCharacter> AnimeVoiceactorCharacters { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<VoiceActor> VoiceActors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.AnimeId).HasName("PK__tmp_ms_x__AF82112AB7EE1730");
        });

        modelBuilder.Entity<AnimeVoiceactorCharacter>(entity =>
        {
            entity.HasOne(d => d.Anime).WithMany(p => p.AnimeVoiceactorCharacters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnimeVoiceactorCharacter_Anime");

            entity.HasOne(d => d.Character).WithMany(p => p.AnimeVoiceactorCharacters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnimeVoiceactorCharacter_Character");

            entity.HasOne(d => d.VoiceActor).WithMany(p => p.AnimeVoiceactorCharacters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnimeVoiceactorCharacter_VoiceActor");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.CharacterId).HasName("PK__tmp_ms_x__757BC9A0CB74E8F6");
        });

        modelBuilder.Entity<VoiceActor>(entity =>
        {
            entity.HasKey(e => e.VoiceAactorId).HasName("PK__tmp_ms_x__B015C2913D9BDA38");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
