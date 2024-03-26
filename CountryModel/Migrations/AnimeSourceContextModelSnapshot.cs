﻿// <auto-generated />
using CountryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CountryModel.Migrations
{
    [DbContext(typeof(AnimeSourceContext))]
    partial class AnimeSourceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CountryModel.Anime", b =>
                {
                    b.Property<int>("AnimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnimeId"));

                    b.Property<byte[]>("AnimeImage")
                        .IsRequired()
                        .HasColumnType("image");

                    b.Property<string>("AnimeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("AnimeId")
                        .HasName("PK__tmp_ms_x__AF82112AB7EE1730");

                    b.ToTable("Anime");
                });

            modelBuilder.Entity("CountryModel.AnimeVoiceactorCharacter", b =>
                {
                    b.Property<int>("AnimeId")
                        .HasColumnType("int");

                    b.Property<int>("VoiceActorId")
                        .HasColumnType("int");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.HasKey("AnimeId", "VoiceActorId", "CharacterId");

                    b.HasIndex("CharacterId");

                    b.HasIndex("VoiceActorId");

                    b.ToTable("AnimeVoiceactorCharacter");
                });

            modelBuilder.Entity("CountryModel.Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CharacterId"));

                    b.Property<byte[]>("CharacterImage")
                        .IsRequired()
                        .HasColumnType("image");

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CharacterId")
                        .HasName("PK__tmp_ms_x__757BC9A0CB74E8F6");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("CountryModel.VoiceActor", b =>
                {
                    b.Property<int>("VoiceAactorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoiceAactorId"));

                    b.Property<byte[]>("VoiceActorImage")
                        .IsRequired()
                        .HasColumnType("image");

                    b.Property<string>("VoiceActorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("VoiceAactorId")
                        .HasName("PK__tmp_ms_x__B015C2913D9BDA38");

                    b.ToTable("VoiceActor");
                });

            modelBuilder.Entity("CountryModel.AnimeVoiceactorCharacter", b =>
                {
                    b.HasOne("CountryModel.Anime", "Anime")
                        .WithMany("AnimeVoiceactorCharacters")
                        .HasForeignKey("AnimeId")
                        .IsRequired()
                        .HasConstraintName("FK_AnimeVoiceactorCharacter_Anime");

                    b.HasOne("CountryModel.Character", "Character")
                        .WithMany("AnimeVoiceactorCharacters")
                        .HasForeignKey("CharacterId")
                        .IsRequired()
                        .HasConstraintName("FK_AnimeVoiceactorCharacter_Character");

                    b.HasOne("CountryModel.VoiceActor", "VoiceActor")
                        .WithMany("AnimeVoiceactorCharacters")
                        .HasForeignKey("VoiceActorId")
                        .IsRequired()
                        .HasConstraintName("FK_AnimeVoiceactorCharacter_VoiceActor");

                    b.Navigation("Anime");

                    b.Navigation("Character");

                    b.Navigation("VoiceActor");
                });

            modelBuilder.Entity("CountryModel.Anime", b =>
                {
                    b.Navigation("AnimeVoiceactorCharacters");
                });

            modelBuilder.Entity("CountryModel.Character", b =>
                {
                    b.Navigation("AnimeVoiceactorCharacters");
                });

            modelBuilder.Entity("CountryModel.VoiceActor", b =>
                {
                    b.Navigation("AnimeVoiceactorCharacters");
                });
#pragma warning restore 612, 618
        }
    }
}