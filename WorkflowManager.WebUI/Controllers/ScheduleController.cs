using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Helpers;
using WorkflowManager.WebUI.Models;

namespace WorkflowManager.WebUI.Controllers
{
	public class ScheduleController : Controller
	{
		private readonly WorkflowManagerRepository _repository;
		private readonly ILogger _logger;
		public ScheduleController(ILogger<ScheduleController> logger)
		{
			_logger = logger;
			_repository = new WorkflowManagerRepository();
		}

		#region User
		public IActionResult UserIndex(string id, string UserId)
		{
			if (UserId != null)
				id = UserId;

			IMapper mapper = AutoMapperConfigs.ScheduleUserIndex().CreateMapper();
			var model = new ScheduleUserIndexViewModel();

			IEnumerable<User> users;
			IEnumerable<Job> userJobs;
			if (id != null)
			{
				userJobs = _repository.JobRepository.SearchFor(j => j.UserJobs.Any(uj => uj.UserId == id) && !j.Done && !j.Deleted)
					.Include(j => j.Building)
				;
				model.UserJobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(userJobs).ToList();
				model.UserJobs.Sort((j1, j2) => j1.PredictedDoneDate.CompareTo(j2.PredictedDoneDate));

				users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician"));
				model.UserSelectList = new SelectList(users, "Id", "FullName", id);
			}
			else
			{
				model.UserJobs = new List<JobViewModel>();

				users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician"));
				model.UserSelectList = new SelectList(users, "Id", "FullName");
			}
			model.UserId = id;

			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult UserEdit(string id)
		{
			if (id == null)
				return NotFound();
			var user = _repository.UserRepository.SearchFor(u => u.Id == id)
				.Include(u => u.UserJobs)
					.ThenInclude(uj => uj.Job)
						.ThenInclude(j => j.Building)
				.First()
			;
			if (user == null)
				return NotFound();

			IMapper mapper = AutoMapperConfigs.ScheduleUserEdit().CreateMapper();
			var model = new ScheduleUserEditViewModel();

			
			IEnumerable<Job> jobs = _repository.JobRepository.SearchFor(j => !j.Done && !j.Deleted && !j.UserJobs.Any(uj => uj.UserId == id))
				.Include(j => j.UserJobs)
					.ThenInclude(uj => uj.User)
				.Include(j => j.Building)
			;
			model.User = mapper.Map<User, UserViewModel>(user);
			model.User.Jobs = model.User.Jobs.Where(j => !j.Done);
			model.UserId = user.Id;
			model.AllJobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(jobs);

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult UserEdit(ScheduleUserEditViewModel model)
		{

			IMapper mapper = AutoMapperConfigs.ScheduleUserEdit().CreateMapper();

			if (ModelState.IsValid)
			{
				if (model.AssignedJobs == null)
				{
					IEnumerable<UserJob> toDelete = _repository.UserJobRepository.SearchFor(uj => uj.UserId == model.UserId);
					foreach (UserJob item in toDelete)
					{
						_repository.UserJobRepository.Delete(item);
					}
				}
				else
				{
					IEnumerable<string> AJ = model.AssignedJobs.Split('&');
					List<int> jobsAssignedIds = new List<int>();
					foreach (string item in AJ)
					{
						jobsAssignedIds.Add(int.Parse(item.Split("=")[1]));
					}


					IEnumerable<UserJob> userUserJobs = _repository.UserJobRepository.SearchFor(uj => uj.UserId == model.UserId);
					foreach (UserJob item in userUserJobs)
					{
						if (jobsAssignedIds.Where(jobId => jobId == item.JobId).Count() == 0)
						{
							_repository.UserJobRepository.Delete(item);
						}
					}


					foreach (int item in jobsAssignedIds)
					{
						if (userUserJobs.Where(uj => uj.JobId == item).Count() == 0)
						{
							UserJob newUserJob = new UserJob()
							{
								UserId = model.UserId,
								JobId = item
							};
							_repository.UserJobRepository.Insert(newUserJob);
						}
					}
				}

				_repository.SaveChanges();
				return RedirectToAction("UserIndex", "Schedule", new { id = model.UserId });
			}
			else
			{
				var user = _repository.UserRepository.SearchFor(u => u.Id == model.UserId)
					.Include(u => u.UserJobs)
						.ThenInclude(uj => uj.Job)
							.ThenInclude(j => j.Building)
					.First()
				;
				IEnumerable<Job> jobs = _repository.JobRepository.SearchFor(j => !j.Done && !j.Deleted && !j.UserJobs.Any(uj => uj.UserId == model.UserId))
				.Include(j => j.UserJobs)
					.ThenInclude(uj => uj.User)
				.Include(j => j.Building)
			;
				model.User = mapper.Map<User, UserViewModel>(user);
				model.AllJobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(jobs);
				return View(model);
			}
		}
		#endregion

		#region Building

		public IActionResult BuildingIndex(int? id, int? BuildingId)
		{
			if (BuildingId != null)
				id = BuildingId;

			IMapper mapper = AutoMapperConfigs.ScheduleBuildingIndex().CreateMapper();
			var model = new ScheduleBuildingIndexViewModel();

			IEnumerable<Building> buildings;
			IEnumerable<Job> buildingJobs;
			if (id != null)
			{
				buildingJobs = _repository.BuildingRepository.SearchFor(b => b.Id == id && b.Done == false)
					.Include(b => b.Jobs)
						.ThenInclude(j => j.UserJobs)
							.ThenInclude(uj => uj.User)
					.First().Jobs
				;

				model.BuildingJobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(buildingJobs.Where(j => !j.Deleted));
				buildings = _repository.BuildingRepository.GetAll();
				model.BuildingSelectList = new SelectList(buildings, "Id", "FullAddress", id);
			}
			else
			{
				model.BuildingJobs = new List<JobViewModel>();

				buildings = _repository.BuildingRepository.GetAll();
				model.BuildingSelectList = new SelectList(buildings, "Id", "FullAddress");
			}
			model.BuildingId = id;

			return View(model);
		}


		[HttpGet]
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult BuildingEdit(int? id)
		{
			IMapper mapper = AutoMapperConfigs.ScheduleBuildingEdit().CreateMapper();
			var model = new ScheduleBuildingEditViewModel();

			if (id == null)
				return NotFound();
			var building = _repository.BuildingRepository.SearchFor(b => !b.Done && b.Id == id)
				.Include(b => b.Jobs)
					.ThenInclude(j => j.UserJobs)
						.ThenInclude(uj => uj.User)
				.FirstOrDefault()
			;
			building.Jobs = building.Jobs.Where(j => !j.Done).ToArray();
			if (building == null)
				return NotFound();
			model.Building = mapper.Map<Building, BuildingViewModel>(building);
			model.Jobs = mapper.Map<Job[], JobViewModel[]>(ScheduleCalculations.SortJobsBySchedule(building.Jobs.Where(j => !j.Deleted)));
			model.BuildingId = building.Id;
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult BuildingEdit(ScheduleBuildingEditViewModel model)
		{
			if (model.JobsOrder == null)
				ModelState.AddModelError("JobsOrder", "Nie dokonano zmian");
			IMapper mapper = AutoMapperConfigs.ScheduleBuildingEdit().CreateMapper();
			if (ModelState.IsValid)
			{
				string[] jobsOrder = model.JobsOrder.Split('&');
				int i = 1;
				foreach (string item in jobsOrder)
				{
					int id = int.Parse(item.Split("=")[1]);
					Job job = _repository.JobRepository.GetById(id);
					job.Order = i;
					_repository.JobRepository.Update(job);

					i++;
				}
				_repository.SaveChanges();

				ScheduleCalculations.CalcBuilidngWorkSchedule(model.BuildingId);
				return RedirectToAction("BuildingIndex", "Schedule", new { id = model.BuildingId });
			}
			else
			{
				var building = _repository.BuildingRepository.SearchFor(b => !b.Done && b.Id == model.BuildingId)
					.Include(b => b.Jobs)
						.ThenInclude(j => j.UserJobs)
							.ThenInclude(uj => uj.User)
					.FirstOrDefault()
				;
				building.Jobs = building.Jobs.Where(j => !j.Done).ToArray();
				model.Jobs = mapper.Map<Job[], JobViewModel[]>(ScheduleCalculations.SortJobsBySchedule(building.Jobs.Where(j => !j.Deleted)));
				model.Building = mapper.Map<Building, BuildingViewModel>(building);
				return View(model);
			}
		}

		#endregion
	}
}
