using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Helpers;
using WorkflowManager.WebUI.Models;

namespace WorkflowManager.WebUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly WorkflowManagerRepository _repository;
		private readonly IMapper _mapper;

		public HomeController(ILogger<HomeController> logger, IMapper mapper)
		{
			_logger = logger;
			_mapper = mapper;
			_repository = new WorkflowManagerRepository();
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Index()
		{
			IMapper mapper = AutoMapperConfigs.HomeIndex().CreateMapper();
			IEnumerable<Job> allJobs = _repository.JobRepository
				.GetAll()
				.Include(job => job.Building)
				.Include(job => job.UserJobs).ThenInclude(uj => uj.User)
			;
			IEnumerable<Job> currJobs = allJobs.Where(job => job.Done == false);
			IEnumerable<Job> doneJobs = allJobs.Where(job => job.Done == true);
			HomeIndexViewModel model = new HomeIndexViewModel
			{
				CurrentJobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(currJobs),
				DoneJobs = mapper.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(doneJobs)
			};

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
