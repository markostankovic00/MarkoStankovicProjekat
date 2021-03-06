// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace Projekat.Migrations
{
    [DbContext(typeof(FitnessContext))]
    [Migration("20220119125837_V3")]
    partial class V3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Models.Klijent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("brKartice")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Klijenti");
                });

            modelBuilder.Entity("Models.Spoj", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Cena")
                        .HasColumnType("int");

                    b.Property<int?>("KlijentID")
                        .HasColumnType("int");

                    b.Property<int>("Ormaric")
                        .HasColumnType("int");

                    b.Property<int?>("TeretanaID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Termin")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TreningID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("KlijentID");

                    b.HasIndex("TeretanaID");

                    b.HasIndex("TreningID");

                    b.ToTable("KlijentiTreninzi");
                });

            modelBuilder.Entity("Models.Teretana", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CenaPoSatu")
                        .HasColumnType("int");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Teretane");
                });

            modelBuilder.Entity("Models.Trening", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Duzina")
                        .HasColumnType("int");

                    b.Property<string>("Tip")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ID");

                    b.ToTable("Treninzi");
                });

            modelBuilder.Entity("Models.Spoj", b =>
                {
                    b.HasOne("Models.Klijent", "Klijent")
                        .WithMany("KlijentTrening")
                        .HasForeignKey("KlijentID");

                    b.HasOne("Models.Teretana", "Teretana")
                        .WithMany("KlijentiTreninzi")
                        .HasForeignKey("TeretanaID");

                    b.HasOne("Models.Trening", "Trening")
                        .WithMany("TreningKlijent")
                        .HasForeignKey("TreningID");

                    b.Navigation("Klijent");

                    b.Navigation("Teretana");

                    b.Navigation("Trening");
                });

            modelBuilder.Entity("Models.Klijent", b =>
                {
                    b.Navigation("KlijentTrening");
                });

            modelBuilder.Entity("Models.Teretana", b =>
                {
                    b.Navigation("KlijentiTreninzi");
                });

            modelBuilder.Entity("Models.Trening", b =>
                {
                    b.Navigation("TreningKlijent");
                });
#pragma warning restore 612, 618
        }
    }
}
