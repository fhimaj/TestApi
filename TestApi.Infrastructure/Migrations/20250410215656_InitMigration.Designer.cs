﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using TestApi.Infrastructure.DbContexts;

#nullable disable

namespace TestApi.Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20250410215656_InitMigration")]
    partial class InitMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestApi.Domain.Entities.Posts.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<decimal>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasDefaultValue(0m);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("date");

                    b.Property<int>("InsertedBy")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("date");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("UserId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("TestApi.Domain.Entities.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DOB")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<decimal>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER")
                        .HasDefaultValue(0m);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("date");

                    b.Property<int>("InsertedBy")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Preferences")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("date");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TestApi.Domain.Entities.Posts.Post", b =>
                {
                    b.HasOne("TestApi.Domain.Entities.Users.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestApi.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
