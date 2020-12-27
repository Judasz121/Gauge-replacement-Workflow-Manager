using WorkflowManager.EFCoreLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkflowManager.EFCoreLibrary.DataAccess
{
	public class WorkflowManagerDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
	{
		public WorkflowManagerDbContext(DbContextOptions<WorkflowManagerDbContext> options) : base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = GaugeReplacementWorkflowManagerEFCoreDB; Integrated Security = True; MultipleActiveResultSets = true; ");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserJob>().HasKey(uj => new { uj.UserId, uj.JobId });
			modelBuilder.Entity<UserJob>()
				.HasOne(uj => uj.User)
				.WithMany(user => user.UserJobs)
				.HasForeignKey(uj => uj.UserId)
			;
			modelBuilder.Entity<UserJob>()
				.HasOne(uj => uj.Job)
				.WithMany(job => job.UserJobs)
				.HasForeignKey(uj => uj.JobId)
			;

			modelBuilder.Entity<UserBuilding>().HasKey(ub => new { ub.UserId, ub.BuildingId });
			modelBuilder.Entity<UserBuilding>()
				.HasOne(ub => ub.User)
				.WithMany(u => u.UserBuildings)
				.HasForeignKey(ub => ub.UserId)
			;
			modelBuilder.Entity<UserBuilding>()
				.HasOne(ub => ub.Building)
				.WithMany(u => u.UserBuildings)
				.HasForeignKey(ub => ub.BuildingId)
			;

			modelBuilder.Entity<User>(User =>
			{
				User.HasMany(u => u.UserRoles)
					.WithOne(ur => ur.User)
					.HasForeignKey(ur => ur.UserId)
					.IsRequired()
				;
            });
			modelBuilder.Entity<Role>()
				.HasMany(r => r.UserRoles)
					.WithOne(ur => ur.Role)
					.HasForeignKey(ur => ur.RoleId)
					.IsRequired()
			;
			#region users and roles data seed
			modelBuilder.Entity<Role>().HasData(
				new Role()
				{
					Id = "admin",
					Name = "Admin",
					NormalizedName = "ADMIN",
					DisplayName = "Administrator",
				},
				new Role()
				{
					Id = "technician",
					Name = "Technician",
					NormalizedName = "TECHNICIAN",
					DisplayName = "Technik"
				},
				new Role()
				{
					Id = "manager",
					Name = "Manager",
					NormalizedName = "MANAGER",
					DisplayName = "Menadżer"
				}
			);


			var passwordHasher = new PasswordHasher<User>();
			User adminUser = new User()
			{
				Id = "admin",
				UserName = "admin",
				NormalizedUserName = "ADMIN",
			};
			User[] users = { adminUser };
			users.FirstOrDefault().PasswordHash = passwordHasher.HashPassword(users[0], "admin");
			modelBuilder.Entity<User>().HasData(users);


			modelBuilder.Entity<UserRole>().HasData(
				new UserRole()
				{
					UserId = "admin",
					RoleId = "admin"
				}
			);
			#endregion

		}


		public DbSet<Job> Jobs { get; set; }
		public new DbSet<User> Users { get; set; }
		public DbSet<UserJob> UserJob { get; set; }
		public new DbSet<Role> Roles { get; set; }
		public DbSet<Building> Buildings { get; set; }
		public DbSet<WaterMeter> WaterMeters { get; set; }
		public DbSet<HeatMeter> HeatMeters { get; set; }
		public DbSet<CostMeter> CostMeters { get; set; }
		public DbSet<UserRole> UserRole { get; set; }
		public DbSet<UserBuilding> UserBuilding { get; set; }

	}

}
