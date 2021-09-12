﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProteinCase;
using ProteinCase.Infrastructure;

namespace ProteinCase.Migrations
{
    [DbContext(typeof(CurrencyDbContext))]
    [Migration("20210910102654_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ProteinCase.Entities.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal?>("BanknoteBuying")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("BanknoteSelling")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("CrossRateOther")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("CrossRateUSD")
                        .HasColumnType("numeric");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("text");

                    b.Property<string>("CurrencyName")
                        .HasColumnType("text");

                    b.Property<decimal>("ForexBuying")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ForexSelling")
                        .HasColumnType("numeric");

                    b.Property<string>("Isim")
                        .HasColumnType("text");

                    b.Property<int>("Unit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });
#pragma warning restore 612, 618
        }
    }
}
