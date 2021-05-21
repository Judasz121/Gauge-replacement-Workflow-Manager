using AutoMapper;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Models;

namespace WorkflowManager.WebUI.Helpers
{
	public static class AutoMapperConfigs
	{
		private static readonly WorkflowManagerRepository _repository = new WorkflowManagerRepository();
		#region Home
		public static MapperConfiguration HomeIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Where(uj => uj.JobId == src.Id).Select(uj => uj.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		#endregion

		#region Schedule 
		public static MapperConfiguration ScheduleUserIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Job, JobViewModel>();
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}

		public static MapperConfiguration ScheduleBuildingIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}

		public static MapperConfiguration ScheduleBuildingEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Building, BuildingViewModel>();
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
			});
		}

		public static MapperConfiguration ScheduleUserEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<User, UserViewModel>()
					.ForMember(dst => dst.Jobs, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.Job)))
				
				;
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.User)))
				;
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}

		#endregion

		#region Job
		public static MapperConfiguration JobIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.User)))
				;
				cfg.CreateMap<Building, BuildingViewModel>();
				cfg.CreateMap<User, UserViewModel>();
			});
		}
		public static MapperConfiguration JobCreate()
		{
			return new MapperConfiguration(cfg =>
			{
				//GET
				cfg.CreateMap<Building, BuildingViewModel>();
				cfg.CreateMap<User, UserViewModel>();
				//POST
				cfg.CreateMap<JobViewModel, Job>();
				cfg.CreateMap<UserViewModel, User>();
			});
		}
		public static MapperConfiguration JobDetails()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Where(uj => uj.JobId == src.Id).Select(uj => uj.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		public static MapperConfiguration JobEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				//GET
				cfg.CreateMap<Job, JobViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
				cfg.CreateMap<User, UserViewModel>();
				//POST
				cfg.CreateMap<JobViewModel, Job>();

			});
		}
		#endregion

		#region Building
		public static MapperConfiguration BuildingIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Building, BuildingViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserBuildings.Select(ub => ub.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
			});
		}
		public static MapperConfiguration BuildingDetails()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Building, BuildingViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserBuildings.Select(ub => ub.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Job, JobViewModel>();

				cfg.CreateMap<WaterMeter, WaterMeterViewModel>();
				cfg.CreateMap<HeatMeter, HeatMeterViewModel>();
				cfg.CreateMap<CostMeter, CostMeterViewModel>();
			});
		}
		public static MapperConfiguration BuildingCreate()
		{
			return new MapperConfiguration(cfg =>
			{
				//GET
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Job, JobViewModel>();

				//POST
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}

		public static MapperConfiguration BuildingEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				//GET
				cfg.CreateMap<Building, BuildingViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserBuildings.Select(ub => ub.User)))
				;
				cfg.CreateMap<User, UserViewModel>();
				cfg.CreateMap<Job, JobViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.User)))
				;
				cfg.CreateMap<WaterMeter, WaterMeterViewModel>();
				cfg.CreateMap<HeatMeter, HeatMeterViewModel>();
				cfg.CreateMap<CostMeter, CostMeterViewModel>();
				//POST
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		#endregion

		#region User
		public static MapperConfiguration UserIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<User, UserViewModel>()
					.ForMember(dst => dst.Jobs, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.Job)))
					.ForMember(dst => dst.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.DisplayName)))
				;
				cfg.CreateMap<Job, JobViewModel>();
			});
		}
		public static MapperConfiguration UserDetails()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<User, UserViewModel>()
					.ForMember(dst => dst.Jobs, opt => opt.MapFrom(src => src.UserJobs.Select(uj => uj.Job)))
					.ForMember(dst => dst.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.DisplayName)))
				;
				cfg.CreateMap<Job, JobViewModel>();
			});
		}
		public static MapperConfiguration UserCreate()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<UserViewModel, User>();
			});
		}
		public static MapperConfiguration UserEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				//GET
				cfg.CreateMap<User, UserViewModel>();

				//POST
				cfg.CreateMap<UserViewModel, User>()
					.ForMember(dst => dst.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
					.ForMember(dst => dst.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
				;
			});
		}

        #endregion

        #region WaterMeter
		public static MapperConfiguration WaterMeterIndex()
        {
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<WaterMeter, WaterMeterViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
        }
		public static MapperConfiguration WaterMeterDetails()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<WaterMeter, WaterMeterViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		public static MapperConfiguration WaterMeterCreate()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<WaterMeterViewModel, WaterMeter>();
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		public static MapperConfiguration WaterMeterEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<WaterMeterViewModel, WaterMeter>();
				cfg.CreateMap<WaterMeter, WaterMeterViewModel>();
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		#endregion

		#region HeatMeter
		public static MapperConfiguration HeatMeterIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<HeatMeter, HeatMeterViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		public static MapperConfiguration HeatMeterDetails()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<HeatMeter, HeatMeterViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		public static MapperConfiguration HeatMeterCreate()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<HeatMeterViewModel, HeatMeter>();
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		public static MapperConfiguration HeatMeterEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<HeatMeterViewModel, HeatMeter>();
				cfg.CreateMap<HeatMeter, HeatMeterViewModel>();
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		#endregion

		#region CostMeter
		public static MapperConfiguration CostMeterIndex()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CostMeter, CostMeterViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		public static MapperConfiguration CostMeterDetails()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CostMeter, CostMeterViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>();
			});
		}
		public static MapperConfiguration CostMeterCreate()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CostMeterViewModel, CostMeter>();
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		public static MapperConfiguration CostMeterEdit()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CostMeterViewModel, CostMeter>();
				cfg.CreateMap<CostMeter, CostMeterViewModel>();
				cfg.CreateMap<BuildingViewModel, Building>();
			});
		}
		#endregion

		#region PDFdownloads
		public static MapperConfiguration BuildingDownload()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Job, JobViewModel>();
				cfg.CreateMap<Building, BuildingViewModel>()
					.ForMember(dst => dst.Users, opt => opt.MapFrom(src => src.UserBuildings.Select(ub => ub.User)))
				;
				cfg.CreateMap<User, UserViewModel>()
					.ForMember(dst => dst.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
				;

				cfg.CreateMap<WaterMeter, WaterMeterViewModel>();
				cfg.CreateMap<HeatMeter, HeatMeterViewModel>();
				cfg.CreateMap<CostMeter, CostMeterViewModel>();
			});
		}

		#endregion
	}
}
