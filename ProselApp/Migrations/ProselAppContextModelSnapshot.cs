﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProselApp.Data;

namespace ProselApp.Migrations
{
    [DbContext(typeof(ProselAppContext))]
    partial class ProselAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("ProselApp.Models.AcessCode.AccessCode", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodeType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("GenDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserCpf")
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.HasIndex("UserCpf");

                    b.ToTable("AccessCode");
                });

            modelBuilder.Entity("ProselApp.Models.Message", b =>
                {
                    b.Property<int>("Messagecode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserCpf")
                        .HasColumnType("TEXT");

                    b.HasKey("Messagecode");

                    b.HasIndex("UserCpf");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("ProselApp.Models.User", b =>
                {
                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("AccessType")
                        .HasColumnType("TEXT");

                    b.Property<bool>("AccountStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Receive_emails")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Cpf");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ProselApp.Models.AcessCode.AccessCode", b =>
                {
                    b.HasOne("ProselApp.Models.User", "User")
                        .WithMany("AccessCodes")
                        .HasForeignKey("UserCpf");
                });

            modelBuilder.Entity("ProselApp.Models.Message", b =>
                {
                    b.HasOne("ProselApp.Models.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserCpf");
                });
#pragma warning restore 612, 618
        }
    }
}
