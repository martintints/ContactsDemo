﻿// <auto-generated />
using Contacts.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Contacts.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200127113642_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("Contacts.Domain.Contact.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(254);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Sequence")
                        .HasColumnType("NUMERIC(32,16)");

                    b.HasKey("Id");

                    b.HasIndex("Sequence");

                    b.ToTable("Contact");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "juhan.juurikas@gmail.com",
                            FirstName = "Juhan",
                            LastName = "Juurikas",
                            Phone = "+3725123456",
                            Sequence = 4000m
                        },
                        new
                        {
                            Id = 2,
                            Email = "mari.maasikas@gmail.com",
                            FirstName = "Mari",
                            LastName = "Maasikas",
                            Phone = "+3725223456",
                            Sequence = 2000m
                        },
                        new
                        {
                            Id = 3,
                            Email = "john.doe@gmail.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Phone = "+1-202-555-0139",
                            Sequence = 1000m
                        },
                        new
                        {
                            Id = 4,
                            Email = "jane.doe@gmail.com",
                            FirstName = "Jane",
                            LastName = "Doe",
                            Phone = "+1-202-555-0182",
                            Sequence = 9000m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}