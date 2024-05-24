﻿// <auto-generated />
using System;
using EelegantIot.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EelegantIot.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240524134945_ChangeDateOnlyToTimeOnly")]
    partial class ChangeDateOnlyToTimeOnly
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<double>("Current")
                        .HasColumnType("float")
                        .HasColumnName("current");

                    b.Property<string>("DayOfWeeks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("EndAt")
                        .HasColumnType("time")
                        .HasColumnName("end_at");

                    b.Property<double>("Humidity")
                        .HasColumnType("float")
                        .HasColumnName("humidity");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("identifier");

                    b.Property<bool>("IsOn")
                        .HasColumnType("bit")
                        .HasColumnName("is_on");

                    b.Property<int>("SettingMode")
                        .HasColumnType("int")
                        .HasColumnName("setting_mode");

                    b.Property<TimeOnly>("StartAt")
                        .HasColumnType("time")
                        .HasColumnName("start_at");

                    b.Property<double>("Temperature")
                        .HasColumnType("float")
                        .HasColumnName("temperature");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<double>("Voltage")
                        .HasColumnType("float")
                        .HasColumnName("voltage");

                    b.HasKey("Id");

                    b.ToTable("device", (string)null);
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.DeviceLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<double>("Current")
                        .HasColumnType("float")
                        .HasColumnName("current");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Humidity")
                        .HasColumnType("float")
                        .HasColumnName("humidity");

                    b.Property<double>("Temperature")
                        .HasColumnType("float")
                        .HasColumnName("temperature");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<double>("Voltage")
                        .HasColumnType("float")
                        .HasColumnName("voltage");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("device_log", (string)null);
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("password");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.UserDevices", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("UserId", "DeviceId");

                    b.HasIndex("DeviceId");

                    b.ToTable("user_devices", (string)null);
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.DeviceLog", b =>
                {
                    b.HasOne("EelegantIot.Api.Domain.Entities.Device", "Device")
                        .WithMany("Logs")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.UserDevices", b =>
                {
                    b.HasOne("EelegantIot.Api.Domain.Entities.Device", "Device")
                        .WithMany("DeviceUsers")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EelegantIot.Api.Domain.Entities.User", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.Device", b =>
                {
                    b.Navigation("DeviceUsers");

                    b.Navigation("Logs");
                });

            modelBuilder.Entity("EelegantIot.Api.Domain.Entities.User", b =>
                {
                    b.Navigation("UserDevices");
                });
#pragma warning restore 612, 618
        }
    }
}
