﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkflowManager.EFCoreLibrary.DataAccess;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    [DbContext(typeof(WorkflowManagerDbContext))]
    [Migration("20201027134050_BuildingAddedDoneColumn")]
    partial class BuildingAddedDoneColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressAdditional")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("AddressBuildingNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("CostMetersRemarks")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DisposedCostMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("DisposedHeatMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("DisposedWaterMetersAmount")
                        .HasColumnType("int");

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<string>("HeatMetersRemarks")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<int?>("LeftCostMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("LeftHeatMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("LeftWaterMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedCheckValvesAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedCostMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedHeatMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedOrReplacedFittingsAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedOrReplacedReductionsAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedOrReplacedValvesAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MountedWaterMetersAmount")
                        .HasColumnType("int");

                    b.Property<int?>("ReplacedTeesAmount")
                        .HasColumnType("int");

                    b.Property<byte[]>("ResidentSign")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("SealedWaterMetersAmount")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("WaterMetersRemarks")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.ToTable("Buildings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AddressAdditional = "22",
                            AddressBuildingNum = "23A",
                            City = "Katowice",
                            Done = false,
                            Street = "Polańska"
                        },
                        new
                        {
                            Id = 2,
                            AddressBuildingNum = "5A",
                            City = "Kraków",
                            Done = false,
                            Street = "Ruczaj"
                        });
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.CostMeter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("CurrMeasurement")
                        .HasColumnType("real");

                    b.Property<float?>("CycleEndMeasurement")
                        .HasColumnType("real");

                    b.Property<string>("DemountedPKONum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("Group")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("IdBuilding")
                        .HasColumnType("int");

                    b.Property<float?>("Measurement")
                        .HasColumnType("real");

                    b.Property<string>("MountedPKONum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Plate")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IdBuilding");

                    b.ToTable("CostMeters");
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.HeatMeter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DemountState")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("DemountedHeatMeterNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("IdBuilding")
                        .HasColumnType("int");

                    b.Property<float?>("Measurement")
                        .HasColumnType("real");

                    b.Property<string>("MountState")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("MountedHeatMeterNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("SealNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("SensorSealNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IdBuilding");

                    b.ToTable("HeatMeters");
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DoneDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdBuilding")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<DateTime>("PredictedDoneDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("PredictedDuration")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("IdBuilding");

                    b.ToTable("Jobs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Deleted = false,
                            Description = "job1Description",
                            Done = false,
                            DoneDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdBuilding = 1,
                            Name = "job1Name",
                            Order = 1,
                            PredictedDoneDate = new DateTime(2020, 10, 27, 18, 40, 49, 625, DateTimeKind.Local).AddTicks(797),
                            PredictedDuration = new TimeSpan(0, 1, 32, 46, 0)
                        },
                        new
                        {
                            Id = 2,
                            Deleted = false,
                            Description = "job2Description",
                            Done = false,
                            DoneDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IdBuilding = 2,
                            Name = "job2Name",
                            Order = 2,
                            PredictedDoneDate = new DateTime(2020, 10, 27, 20, 40, 49, 631, DateTimeKind.Local).AddTicks(450),
                            PredictedDuration = new TimeSpan(0, 2, 22, 56, 0)
                        });
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("BuildingId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("Lock")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "5961d94d-0e6b-46ba-a8de-88cd4fcc7121",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "3db4ddf0-e7de-40fb-9c89-643a6fe3d5f5",
                            EmailConfirmed = false,
                            FirstName = "Antoni",
                            LastName = "Tyl",
                            Lock = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "9550aed1-d73a-4c54-810e-a55335445f0f",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            Id = "583f5944-ca2e-466d-bf28-961365042b5a",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "16776991-4032-4ecf-abc5-15f408cfb4e2",
                            EmailConfirmed = false,
                            FirstName = "Maciej",
                            LastName = "Waszalski",
                            Lock = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "18147be4-f576-4c28-b144-20f5995a9709",
                            TwoFactorEnabled = false
                        });
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.UserJob", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "JobId");

                    b.HasIndex("JobId");

                    b.ToTable("UserJob");

                    b.HasData(
                        new
                        {
                            UserId = "5961d94d-0e6b-46ba-a8de-88cd4fcc7121",
                            JobId = 2
                        },
                        new
                        {
                            UserId = "583f5944-ca2e-466d-bf28-961365042b5a",
                            JobId = 1
                        });
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.WaterMeter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CheckValve")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("DemountState")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("DemountedWaterMeterNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("IdBuilding")
                        .HasColumnType("int");

                    b.Property<float?>("Measurement")
                        .HasColumnType("real");

                    b.Property<string>("MountState")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("MountedWaterMeterNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("SealNum")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IdBuilding");

                    b.ToTable("WaterMeters");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.CostMeter", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Building", "Building")
                        .WithMany("CostMeters")
                        .HasForeignKey("IdBuilding")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.HeatMeter", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Building", "Building")
                        .WithMany("HeatMeters")
                        .HasForeignKey("IdBuilding")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.Job", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Building", "Building")
                        .WithMany("Jobs")
                        .HasForeignKey("IdBuilding")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.User", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Building", null)
                        .WithMany("Technicians")
                        .HasForeignKey("BuildingId");
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.UserJob", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Job", "Job")
                        .WithMany("UserJobs")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.User", "User")
                        .WithMany("UserJobs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkflowManager.EFCoreLibrary.Entities.WaterMeter", b =>
                {
                    b.HasOne("WorkflowManager.EFCoreLibrary.Entities.Building", "Building")
                        .WithMany("WaterMeters")
                        .HasForeignKey("IdBuilding")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
