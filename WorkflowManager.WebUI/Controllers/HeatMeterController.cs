using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Helpers;
using WorkflowManager.WebUI.Models;

namespace WorkflowManager.WebUI.Controllers
{
	public class HeatMeterController : Controller
	{
		private readonly ILogger<BuildingController> _logger;
		private readonly WorkflowManagerRepository _repository;

		public HeatMeterController(ILogger<BuildingController> logger)
		{
			_logger = logger;
			_repository = new WorkflowManagerRepository();
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Index(int? BuildingId)
        {
			var model = new HeatMeterIndexViewModel();
			IMapper mapper = AutoMapperConfigs.HeatMeterIndex().CreateMapper();
			IEnumerable<Building> buildings;

			if (User.IsInRole("Technician") && !User.IsInRole("Manager"))
				buildings = _repository.BuildingRepository.SearchFor(b => b.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name) || b.Jobs.Any(j => j.UserJobs.Any(uj => uj.User.UserName == User.Identity.Name)));
			else
				buildings = _repository.BuildingRepository.GetAll();


			if (BuildingId == null)
			{
				ICollection<HeatMeter> HeatMeters = _repository.HeatMeterRepository.GetAll().ToList();
				foreach (var item in HeatMeters)
				{
					if (buildings.Any(b => b.Id == item.BuildingId))
						item.Building = buildings.Where(b => b.Id == item.BuildingId).First();
				}
				model.HeatMeters = mapper.Map<ICollection<HeatMeter>, ICollection<HeatMeterViewModel>>(HeatMeters);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress");
			}
			else
			{
				ICollection<HeatMeter> HeatMeters = _repository.HeatMeterRepository.SearchFor(wm => wm.BuildingId == BuildingId).ToList();
				foreach (var item in HeatMeters)
				{
					if (buildings.Any(b => b.Id == item.BuildingId))
						item.Building = buildings.Where(b => b.Id == item.BuildingId).First();
				}
				model.HeatMeters = mapper.Map<ICollection<HeatMeter>, ICollection<HeatMeterViewModel>>(HeatMeters);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", BuildingId);
			}

			return View(model);
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Details(int? id)
		{ 
			if (id == null)
				return NotFound();

			var heatMeter = _repository.HeatMeterRepository.SearchFor(hm => hm.Id == id)
				.Include(hm => hm.Building)
					.ThenInclude(b => b.UserBuildings)
						.ThenInclude(ub => ub.User)
				.First()
			;
			if (heatMeter == null)
				return NotFound();
			if (!heatMeter.Building.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name) && User.IsInRole("Technician"))
				return StatusCode(403);

			IMapper mapper = AutoMapperConfigs.HeatMeterDetails().CreateMapper();
			var model = mapper.Map<HeatMeter, HeatMeterViewModel>(heatMeter);

			return View(model);
        }

		[HttpGet]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Create(int? buildingId)
		{
			var model = new HeatMeterCreateViewModel();
			IEnumerable<Building> buildings;

			if (User.IsInRole("Technician") && !User.IsInRole("Manager"))
				buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false && b.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name));
			else
				buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);

			if (buildingId != null)
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", buildingId);
			else
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress");

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Create(HeatMeterCreateViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.HeatMeterCreate().CreateMapper();
			var HeatMeter = mapper.Map<HeatMeterViewModel, HeatMeter>(model.HeatMeter);

			if(HeatMeter.MountedHeatMeterNum != null && HeatMeter.MountedHeatMeterNum != "" && HeatMeter.DemountedHeatMeterNum != null && HeatMeter.DemountedHeatMeterNum != "")
			if (_repository.HeatMeterRepository.SearchFor(wm => wm.BuildingId == HeatMeter.BuildingId && HeatMeter.MountedHeatMeterNum == wm.MountedHeatMeterNum && wm.DemountedHeatMeterNum == HeatMeter.DemountedHeatMeterNum).Count() > 0)
			{
				ModelState.AddModelError("HeatMeter.DemountedHeatMeterNum", "Istnieje już miernik o takim numerze");
				ModelState.AddModelError("HeatMeter.MountedHeatMeterNum", "Istnieje już miernik o takim numerze");
			}

			if (ModelState.IsValid)
			{
				_repository.HeatMeterRepository.Insert(HeatMeter);
				_repository.SaveChanges();
				return RedirectToAction("Index", "HeatMeter");
			}
			else
			{
				IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.HeatMeter.BuildingId);
				return View(model);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Edit(int? id)
		{
			IMapper mapper = AutoMapperConfigs.HeatMeterEdit().CreateMapper();
			if (id == null)
				return NotFound();
			var HeatMeter = _repository.HeatMeterRepository.GetById(id);
			if (HeatMeter == null)
				return NotFound();

			var model = new HeatMeterEditViewModel();
			model.HeatMeter = mapper.Map<HeatMeter, HeatMeterViewModel>(HeatMeter);
			IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
			model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.HeatMeter.BuildingId);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Edit(HeatMeterEditViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.HeatMeterEdit().CreateMapper();
			var HeatMeter = mapper.Map<HeatMeterViewModel, HeatMeter>(model.HeatMeter);

			if (HeatMeter.MountedHeatMeterNum != null && HeatMeter.MountedHeatMeterNum != "" && HeatMeter.DemountedHeatMeterNum != null && HeatMeter.DemountedHeatMeterNum != "")
				if (_repository.HeatMeterRepository.SearchFor(hm => hm.Id != HeatMeter.Id && hm.BuildingId == HeatMeter.BuildingId && HeatMeter.MountedHeatMeterNum == hm.MountedHeatMeterNum && hm.DemountedHeatMeterNum == HeatMeter.DemountedHeatMeterNum).Count() > 0)
				{
					ModelState.AddModelError("HeatMeter.DemountedHeatMeterNum", "Istnieje już miernik o takim numerze");
					ModelState.AddModelError("HeatMeter.MountedHeatMeterNum", "Istnieje już miernik o takim numerze");
				}

			if (ModelState.IsValid)
			{
				_repository.HeatMeterRepository.Update(HeatMeter);
				_repository.SaveChanges();
				return RedirectToAction("Index", "HeatMeter");
			}
			else
			{
				IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.HeatMeter.BuildingId);
				return View(model);
			}
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Delete(int? id)
		{
			if (id == null)
				return NotFound();
			var HeatMeter = _repository.HeatMeterRepository.GetById(id);
			if (HeatMeter == null)
				return NotFound();

			_repository.HeatMeterRepository.Delete(HeatMeter);
			_repository.SaveChanges();
			return RedirectToAction("Index", "HeatMeter");
		}

	}
}
