﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Yourthome.Data;

namespace Yourthome.Migrations
{
    [DbContext(typeof(YourthomeContext))]
    [Migration("20200728104819_deleteavatar")]
    partial class deleteavatar
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Yourthome.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EvictionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("GuestName")
                        .HasColumnType("text");

                    b.Property<int>("RentalID")
                        .HasColumnType("integer");

                    b.HasKey("BookingID");

                    b.HasIndex("RentalID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Yourthome.Models.Facilities", b =>
                {
                    b.Property<int>("FacilitiesID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool?>("AirConditioning")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Balcony")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Internet")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Kitchen")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Phone")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Refrigerator")
                        .HasColumnType("boolean");

                    b.Property<int>("RentalID")
                        .HasColumnType("integer");

                    b.Property<bool?>("TV")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Washer")
                        .HasColumnType("boolean");

                    b.HasKey("FacilitiesID");

                    b.HasIndex("RentalID")
                        .IsUnique();

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("Yourthome.Models.Idsafer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("SafedID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("Idsafer");
                });

            modelBuilder.Entity("Yourthome.Models.ImageModel", b =>
                {
                    b.Property<int>("ImageModelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<int?>("RentalID")
                        .HasColumnType("integer");

                    b.HasKey("ImageModelID");

                    b.HasIndex("RentalID");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Yourthome.Models.Infrastructure", b =>
                {
                    b.Property<int>("InfrastructureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool?>("BusStop")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Cafe")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Hospital")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Kindergarten")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Park")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Parking")
                        .HasColumnType("boolean");

                    b.Property<int>("RentalID")
                        .HasColumnType("integer");

                    b.Property<bool?>("Supermarket")
                        .HasColumnType("boolean");

                    b.HasKey("InfrastructureID");

                    b.HasIndex("RentalID")
                        .IsUnique();

                    b.ToTable("Infrastructure");
                });

            modelBuilder.Entity("Yourthome.Models.Rental", b =>
                {
                    b.Property<int>("RentalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Cost")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<int>("PropertyType")
                        .HasColumnType("integer");

                    b.Property<int>("Region")
                        .HasColumnType("integer");

                    b.Property<int>("RentTime")
                        .HasColumnType("integer");

                    b.Property<int>("Rooms")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.HasKey("RentalID");

                    b.ToTable("Rental");
                });

            modelBuilder.Entity("Yourthome.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("AvatarStored")
                        .HasColumnType("bytea");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<int>("Phone")
                        .HasColumnType("integer");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Yourthome.Models.Booking", b =>
                {
                    b.HasOne("Yourthome.Models.Rental", "Rental")
                        .WithMany("Bookings")
                        .HasForeignKey("RentalID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Yourthome.Models.Facilities", b =>
                {
                    b.HasOne("Yourthome.Models.Rental", "Rental")
                        .WithOne("Facilities")
                        .HasForeignKey("Yourthome.Models.Facilities", "RentalID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Yourthome.Models.ImageModel", b =>
                {
                    b.HasOne("Yourthome.Models.Rental", null)
                        .WithMany("Photos")
                        .HasForeignKey("RentalID");
                });

            modelBuilder.Entity("Yourthome.Models.Infrastructure", b =>
                {
                    b.HasOne("Yourthome.Models.Rental", "Rental")
                        .WithOne("Infrastructure")
                        .HasForeignKey("Yourthome.Models.Infrastructure", "RentalID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
