using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowManager.EFCoreLibrary.Entities;

namespace WorkflowManager.EFCoreLibrary.DataAccess
{
	public class WorkflowManagerRepository
	{
		public readonly WorkflowManagerDbContext _context;

		public WorkflowManagerRepository()
		{
			var optionsBuilder = new DbContextOptionsBuilder<WorkflowManagerDbContext>();
			optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WorkflowManagerEFCoreDB; Integrated Security=True;");
			_context = new WorkflowManagerDbContext(optionsBuilder.Options);
			
			BuildingRepository = new EFRepository<Building>(_context);
			CostMeterRepository = new EFRepository<CostMeter>(_context);
			HeatMeterRepository = new EFRepository<HeatMeter>(_context);
			WaterMeterRepository = new EFRepository<WaterMeter>(_context);
			RoleRepository = new EFRepository<Role>(_context);
			JobRepository = new EFRepository<Job>(_context);
			UserRepository = new EFRepository<User>(_context);
			UserJobRepository = new EFRepository<UserJob>(_context);
			UserRoleRepository = new EFRepository<UserRole>(_context);
			UserBuildingRepository = new EFRepository<UserBuilding>(_context);
		}

		public EFRepository<Building> BuildingRepository { get; private set; }
		public EFRepository<CostMeter> CostMeterRepository { get; private set; }
		public EFRepository<HeatMeter> HeatMeterRepository { get; private set; }
		public EFRepository<WaterMeter> WaterMeterRepository { get; private set; }
		public EFRepository<Role> RoleRepository { get; private set; }
		public EFRepository<Job> JobRepository { get; private set; }
		public EFRepository<User> UserRepository { get; private set; }
		public EFRepository<UserJob> UserJobRepository { get; private set; }
		public EFRepository<UserRole> UserRoleRepository { get; private set; }
		public EFRepository<UserBuilding> UserBuildingRepository { get; private set; }

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
