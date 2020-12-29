using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkflowManager.WebUI.Models;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using ValidateAntiForgeryTokenAttribute = Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;

namespace WorkflowManager.WebUI.Controllers
{
    public class JobController : Controller
    {
        private readonly ILogger<JobController> _logger;
		private readonly WorkflowManagerRepository _repository;


        public JobController(ILogger<JobController> logger)
        {
            _logger = logger;
            _repository = new WorkflowManagerRepository();
        }

		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Index()
        {
			IMapper mapper = AutoMapperConfigs.JobIndex().CreateMapper();
			IEnumerable<Job> jobs;

			if (User.IsInRole("Technician"))
			{
				jobs = _repository.JobRepository
					.SearchFor(j => j.UserJobs.Any(uj => uj.User.UserName == User.Identity.Name))
					.Include(job => job.Building)
					.Include(job => job.UserJobs)
						.ThenInclude(uj => uj.User)
					.ToList()
				;
			}
			else 
			{
				jobs = _repository.JobRepository
					.GetAll()
					.Include(job => job.Building)
					.Include(job => job.UserJobs)
						.ThenInclude(uj => uj.User)
					.ToList()
				;
			}

			JobIndexViewModel model = new JobIndexViewModel()
			{
				Jobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(jobs)
			};
            return View(model);
        }

		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Details(int? id)
		{
			IMapper mapper = AutoMapperConfigs.JobDetails().CreateMapper();
			if (id == null)
			{
				return NotFound();
			}

			Job job = _repository.JobRepository.GetById(id);
			if (job == null)
			{
				return NotFound();
			}
			if (!job.UserJobs.Any(uj => uj.User.UserName == User.Identity.Name) && !User.IsInRole("Manager") && !User.IsInRole("Admin"))
				return StatusCode(403);
				
			
			job.Building = _repository.BuildingRepository.GetById(job.IdBuilding);
			job.UserJobs = _repository.UserJobRepository
				.SearchFor(uj => uj.JobId == job.Id)
				.Include(uj => uj.User)
				.ToList()
			;

			JobViewModel model = mapper.Map<Job, JobViewModel>(job);

			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Create()
		{
			IMapper mapper = AutoMapperConfigs.JobCreate().CreateMapper();
			IEnumerable<BuildingViewModel> buildings = mapper.Map<List<Building>, List<BuildingViewModel>>(_repository.BuildingRepository.GetAll().ToList());
			IEnumerable<UserViewModel> users = mapper.Map<List<User>, List<UserViewModel>>(_repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager")).ToList());
			JobCreateViewModel model = new JobCreateViewModel()
			{
				Buildings = new SelectList(buildings, "Id", "FullAddress"),
				Users = new SelectList(users, "Id", "FullName"),
				Job = new JobViewModel()
			};
            return View(model);
		}



		// POST: Jobs/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Manager, Technician")]
		public async Task<IActionResult> Create(JobCreateViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.JobCreate().CreateMapper();

			if (_repository.JobRepository.SearchFor(p => p.Name == model.Job.Name && !p.Deleted && p.IdBuilding != model.SelectedBuildingId).Count() > 0)
				ModelState.AddModelError("Job.Name", "Istnieje już zadanie o takiej nazwie");
			if (ModelState.IsValid)
			{
				if (_repository.JobRepository.SearchFor(j => j.IdBuilding == model.SelectedBuildingId && !j.Deleted).Any())
				{
					var jobList = _repository.JobRepository.SearchFor(j => j.IdBuilding == model.SelectedBuildingId && !j.Deleted).ToList();
					if (jobList.Count() > 1)
						model.Job.Order = jobList.Aggregate((j1, j2) => j1.Order > j2.Order ? j1 : j2).Order + 1;
					else
						model.Job.Order = jobList.FirstOrDefault().Order + 1;
				}
				else
					model.Job.Order = 1;

				Job job = mapper.Map<JobViewModel, Job>(model.Job);

				Task<Job> scheduleCalcTask = ScheduleCalculations.CalcNewAddedJobPredictedDoneDate(job);
				job.DateAdded = DateTime.Now;
				job.IdBuilding = model.SelectedBuildingId;
				Job calcJob = await scheduleCalcTask;
				job.PredictedDoneDate = calcJob.PredictedDoneDate;
				_repository.JobRepository.Insert(job);
				_repository.SaveChanges(); // to populate job.Id

				// userjobs 
				if(model.SelectedUserIds != null)
				foreach (string currId in model.SelectedUserIds)
				{
					UserJob UJ = new UserJob()
					{
						UserId = currId,
						JobId = job.Id
					};
					_repository.UserJobRepository.Insert(UJ);
				}
				_repository.SaveChanges();
				ScheduleCalculations.CalcBuilidngWorkSchedule(job.IdBuilding);
				return RedirectToAction("Index", "Job");
			}
			else
			{
				IEnumerable<BuildingViewModel> buildings = mapper.Map<List<Building>, List<BuildingViewModel>>(_repository.BuildingRepository.GetAll().ToList());
				IEnumerable<UserViewModel> users = mapper.Map<List<User>, List<UserViewModel>>(_repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager")).ToList());
				model.Buildings = new SelectList(buildings, "Id", "FullAddress");
				model.Users = new SelectList(users, "Id", "FullName");
				return View(model);
			}
		}

		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Edit(int? id)
		{
			IMapper mapper = AutoMapperConfigs.JobEdit().CreateMapper();
			if (id == null)
			{
				return NotFound();
			}

			Job job = _repository.JobRepository
				.SearchFor(j => j.Id == id)
				.Include(j => j.UserJobs)
					.ThenInclude(uj => uj.User)
				.First()
			;
			if (job == null)
			{
				return NotFound();
			}
			IEnumerable<Building> buildings = _repository.BuildingRepository.GetAll();
			IEnumerable<User> users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager"));
			JobEditViewModel model = new JobEditViewModel()
			{
				Job = mapper.Map<Job, JobViewModel>(job),
				Buildings = new SelectList(buildings, "Id", "FullAddress", job.IdBuilding),
				Users = new MultiSelectList(users, "Id", "FullName", job.UserJobs.Select(uj => uj.User.Id))
			};
			model.SelectedBuildingId = job.IdBuilding;
			return View(model);
		}

		// POST: Jobs/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Edit(JobEditViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.JobEdit().CreateMapper();
			if (_repository.JobRepository.SearchFor(p => p.Name == model.Job.Name && !p.Deleted && p.IdBuilding != model.SelectedBuildingId && p.Id != model.Job.Id).Count() > 0)
				ModelState.AddModelError("Name", "Istnieje już zadanie o takiej nazwie");
			if (ModelState.IsValid)
			{
				TimeSpan oldPredictedDuration = _repository.JobRepository.SearchFor(j => j.Id == model.Job.Id)
					.AsNoTracking()
					.FirstOrDefault()
					.PredictedDuration
				;
				Job job = mapper.Map<JobViewModel, Job>(model.Job);
				job.IdBuilding = model.SelectedBuildingId;
				_repository.JobRepository.Update(job);

				#region userJobs

				IEnumerable<UserJob> userJobs = _repository.UserJobRepository.SearchFor(uj => uj.JobId == model.Job.Id).ToList();
				IEnumerable<string> userIds = userJobs.Select(uj => uj.UserId);
				bool recalcSchedule = model.SelectedUserIds != null && !new HashSet<string>(model.SelectedUserIds).SetEquals(userIds) || oldPredictedDuration != job.PredictedDuration;


				if (model.SelectedUserIds != null)
				{
					foreach (string currId in model.SelectedUserIds)
					{
						if (userJobs.Where(uj => uj.UserId == currId).Count() == 0)
						{
							UserJob UJ = new UserJob()
							{
								UserId = currId,
								JobId = model.Job.Id
							};
							_repository.UserJobRepository.Insert(UJ);
						}
					}
					foreach(var uj in userJobs)
                    {
						if (model.SelectedUserIds.Where(id => id == uj.UserId).Count() == 0)
							_repository.UserJobRepository.Delete(uj);
                    }
				}
                else
                {
					foreach(var uj in userJobs)
                    {
						_repository.UserJobRepository.Delete(uj);
                    }
                }
				#endregion
				_repository.SaveChanges();
				
				
				if (recalcSchedule)
					ScheduleCalculations.CalcBuilidngWorkSchedule(job.IdBuilding);
				return RedirectToAction(nameof(Index));
			}
			else
			{
				Job job = _repository.JobRepository
					.SearchFor(j => j.Id == model.Job.Id)
					.Include(j => j.UserJobs)
						.ThenInclude(uj => uj.User)
					.First()
				;
				IEnumerable<Building> buildings = _repository.BuildingRepository.GetAll();
				IEnumerable<User> users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician"));
				model.Job = mapper.Map<Job, JobViewModel>(job);
				model.Buildings = new SelectList(buildings, "Id", "FullAddress", job.IdBuilding);
				model.Users = new MultiSelectList(users, "Id", "FullName", model.SelectedUserIds);
				return View(model);
			}
		}

		[Authorize(Roles = "Manager, Admin")]
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var job = _repository.JobRepository.GetById(id);
			if (job == null)
			{
				return NotFound();
			}
			job.Deleted = true;
			_repository.JobRepository.Update(job);
			_repository.SaveChanges();
			return RedirectToAction("Index", "Job");
		}

		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Done(int? id)
		{
			if (id == null)
				return NotFound();
			var job = _repository.JobRepository.SearchFor(j => j.Id == id && !j.Done)
				.First()
			;
			if (job == null)
				return NotFound();

			job.Done = true;
			job.DoneDate = DateTime.Now;
			_repository.JobRepository.Update(job);
			_repository.SaveChanges();

			return RedirectToAction("Index", "Job");
		}
	}
}
