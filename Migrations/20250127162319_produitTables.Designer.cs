﻿// <auto-generated />
using System;
using DaberlyProjet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DaberlyProjet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250127162319_produitTables")]
    partial class produitTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DaberlyProjet.Models.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("EstPhoto")
                        .HasColumnType("boolean");

                    b.Property<int>("ProduitId")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProduitId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Couleur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Couleurs");
                });

            modelBuilder.Entity("DaberlyProjet.Models.PhotoUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BCin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FCin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IdUser")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdUser");

                    b.ToTable("PhotosUser");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Pointure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Taille")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.ToTable("Pointures");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Produit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Marque")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<decimal>("Prix")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Produits");
                });

            modelBuilder.Entity("DaberlyProjet.Models.ProduitPointureCouleur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CouleurId")
                        .HasColumnType("integer");

                    b.Property<int>("PointureId")
                        .HasColumnType("integer");

                    b.Property<int>("ProduitId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CouleurId");

                    b.HasIndex("PointureId");

                    b.HasIndex("ProduitId");

                    b.ToTable("ProduitPointureCouleurs");
                });

            modelBuilder.Entity("DaberlyProjet.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Addresse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("blocked")
                        .HasColumnType("boolean");

                    b.Property<string>("numCin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Album", b =>
                {
                    b.HasOne("DaberlyProjet.Models.Produit", "Produit")
                        .WithMany("Albums")
                        .HasForeignKey("ProduitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produit");
                });

            modelBuilder.Entity("DaberlyProjet.Models.PhotoUser", b =>
                {
                    b.HasOne("DaberlyProjet.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DaberlyProjet.Models.ProduitPointureCouleur", b =>
                {
                    b.HasOne("DaberlyProjet.Models.Couleur", "Couleur")
                        .WithMany("ProduitPointureCouleurs")
                        .HasForeignKey("CouleurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaberlyProjet.Models.Pointure", "Pointure")
                        .WithMany("ProduitPointureCouleurs")
                        .HasForeignKey("PointureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaberlyProjet.Models.Produit", "Produit")
                        .WithMany("ProduitPointureCouleurs")
                        .HasForeignKey("ProduitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Couleur");

                    b.Navigation("Pointure");

                    b.Navigation("Produit");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Couleur", b =>
                {
                    b.Navigation("ProduitPointureCouleurs");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Pointure", b =>
                {
                    b.Navigation("ProduitPointureCouleurs");
                });

            modelBuilder.Entity("DaberlyProjet.Models.Produit", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("ProduitPointureCouleurs");
                });
#pragma warning restore 612, 618
        }
    }
}
