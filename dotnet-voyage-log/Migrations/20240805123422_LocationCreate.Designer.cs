﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using dotnet_voyage_log.Context;

#nullable disable

namespace dotnet_voyage_log.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240805123422_LocationCreate")]
    partial class LocationCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("dotnet_voyage_log.Models.Country", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("country_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country_name");

                    b.HasKey("Id");

                    b.ToTable("countries");
                });

            modelBuilder.Entity("dotnet_voyage_log.Models.Region", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("region_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<long>("CountryId")
                        .HasColumnType("bigint")
                        .HasColumnName("country_fk");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("region_name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("regions");
                });

            modelBuilder.Entity("dotnet_voyage_log.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("AppRole")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("app_role");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswdHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AppRole = "admin",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@localhost.com",
                            PasswdHash = "$2a$11$LWhhkffnmPNLGTuz4FZ3WettPfsVrsJm9LxkhtFZqRjusAm4BjdxW",
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("dotnet_voyage_log.Models.Voyage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("voyage_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<long>("CountryId")
                        .HasColumnType("bigint")
                        .HasColumnName("countryFK");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<List<string>>("Images")
                        .HasColumnType("text[]")
                        .HasColumnName("images");

                    b.Property<double?>("LocationLatitude")
                        .HasColumnType("double precision")
                        .HasColumnName("location_latitude");

                    b.Property<double?>("LocationLongitude")
                        .HasColumnType("double precision")
                        .HasColumnName("location_longitude");

                    b.Property<string>("Notes")
                        .HasColumnType("text")
                        .HasColumnName("notes");

                    b.Property<long>("RegionId")
                        .HasColumnType("bigint")
                        .HasColumnName("regionFK");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("topic");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("voyages");
                });

            modelBuilder.Entity("dotnet_voyage_log.Models.Region", b =>
                {
                    b.HasOne("dotnet_voyage_log.Models.Country", "Country")
                        .WithMany("Regions")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("dotnet_voyage_log.Models.Country", b =>
                {
                    b.Navigation("Regions");
                });
#pragma warning restore 612, 618
        }
    }
}
