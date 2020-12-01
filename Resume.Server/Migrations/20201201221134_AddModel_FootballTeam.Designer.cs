﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Resume.Server.Migrations
{
    [DbContext(typeof(ResumeDbContext))]
    [Migration("20201201221134_AddModel_FootballTeam")]
    partial class AddModel_FootballTeam
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Resume.Domain.FootballTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("JSListOfFootBallMatch_Matches")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdateOfSeasonMatches")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeasonIdForFootballMatches")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FootballTeam");
                });

            modelBuilder.Entity("Resume.Domain.PaymentReceipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExternalRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InternalRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NotificationWasSent")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("PaymentReceipt");
                });

            modelBuilder.Entity("Resume.Domain.PaymentReceipt", b =>
                {
                    b.OwnsOne("Resume.Domain.ValueObjects.Money", "Amount", b1 =>
                        {
                            b1.Property<int>("PaymentReceiptId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .UseIdentityColumn();

                            b1.HasKey("PaymentReceiptId");

                            b1.ToTable("PaymentReceipt");

                            b1.WithOwner()
                                .HasForeignKey("PaymentReceiptId");
                        });

                    b.Navigation("Amount");
                });
#pragma warning restore 612, 618
        }
    }
}
