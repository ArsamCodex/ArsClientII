﻿// <auto-generated />
using ArsClientII;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArsClientII.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230712082345_AddedTraslationWord")]
    partial class AddedTraslationWord
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArsClientII.Information", b =>
                {
                    b.Property<int>("InformationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InformationID"));

                    b.Property<string>("RestartCount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShutDownCount")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InformationID");

                    b.ToTable("Information");
                });

            modelBuilder.Entity("ArsClientII.TranslationWords", b =>
                {
                    b.Property<int>("TranslationWordsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TranslationWordsID"));

                    b.Property<string>("Dutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnglishWord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Farsi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("French")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Spanish")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TranslationWordsID");

                    b.ToTable("TranslationWords");
                });
#pragma warning restore 612, 618
        }
    }
}
