﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfHR.Models;

#nullable disable

namespace WpfHR.Migrations
{
    [DbContext(typeof(ModuleDbContext))]
    [Migration("20241206093750_AddApproversAndDevelopersStrings")]
    partial class AddApproversAndDevelopersStrings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WpfHR.Models.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApproversString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("DevelopersString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainApprover")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Modules");
                });
#pragma warning restore 612, 618
        }
    }
}
