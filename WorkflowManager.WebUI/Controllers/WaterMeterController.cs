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
	public class WaterMeterController : Controller
	{
		private readonly ILogger<BuildingController> _logger;
		private readonly WorkflowManagerRepository _repository;

		public WaterMeterController(ILogger<BuildingController> logger)
		{
			_logger = logger;
			_repository = new WorkflowManagerRepository();
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Index(int? BuildingId)
        {
			var model = new WaterMeterIndexViewModel();
			IMapper mapper = AutoMapperConfigs.WaterMeterIndex().CreateMapper();
			IEnumerable<Building> buildings;

			if (User.IsInRole("Technician") && !User.IsInRole("Manager"))
				buildings = _repository.BuildingRepository.SearchFor(b => b.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name) || b.Jobs.Any(j => j.UserJobs.Any(uj => uj.User.UserName == User.Identity.Name)));
			else
				buildings = _repository.BuildingRepository.GetAll();

			if (BuildingId == null)
			{
				ICollection<WaterMeter> waterMeters = _repository.WaterMeterRepository.GetAll().ToList();
				foreach (var item in waterMeters)
				{
					if (buildings.Any(b => b.Id == item.BuildingId))
						item.Building = buildings.Where(b => b.Id == item.BuildingId).First();	
				}                
				model.WaterMeters = mapper.Map<ICollection<WaterMeter>, ICollection<WaterMeterViewModel>>(waterMeters);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress");
			}
			else
			{
				ICollection<WaterMeter> waterMeters = _repository.WaterMeterRepository.SearchFor(wm => wm.BuildingId == BuildingId).ToList();
				foreach (var item in waterMeters)
				{
					if (buildings.Any(b => b.Id == item.BuildingId))
						item.Building = buildings.Where(b => b.Id == item.BuildingId).First();
				}
				model.WaterMeters = mapper.Map<ICollection<WaterMeter>, ICollection<WaterMeterViewModel>>(waterMeters);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", BuildingId);
			}

			return View(model);
        }

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Details(int? id)
		{ 
			if (id == null)
				return NotFound();

			var waterMeter = _repository.WaterMeterRepository.SearchFor(wm => wm.Id == id)
				.Include(hm => hm.Building)
					.ThenInclude(b => b.UserBuildings)
						.ThenInclude(ub => ub.User)
				.First()
			;
			if (waterMeter == null)
				return NotFound();
			if (!waterMeter.Building.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name) && User.IsInRole("Technician"))
				return StatusCode(403);

			IMapper mapper = AutoMapperConfigs.WaterMeterDetails().CreateMapper();
			var model = mapper.Map<WaterMeter, WaterMeterViewModel>(waterMeter);

			return View(model);
        }

		[HttpGet]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Create(int? buildingId)
		{
			var model = new WaterMeterCreateViewModel();
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
		public IActionResult Create(WaterMeterCreateViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.WaterMeterCreate().CreateMapper();
			var waterMeter = mapper.Map<WaterMeterViewModel, WaterMeter>(model.WaterMeter);

			if(waterMeter.MountedWaterMeterNum != null && waterMeter.MountedWaterMeterNum != "" && waterMeter.DemountedWaterMeterNum != null && waterMeter.DemountedWaterMeterNum != "")
			if (_repository.WaterMeterRepository.SearchFor(wm => wm.Id != waterMeter.Id && wm.BuildingId == waterMeter.BuildingId && waterMeter.MountedWaterMeterNum == wm.MountedWaterMeterNum && wm.DemountedWaterMeterNum == waterMeter.DemountedWaterMeterNum).Count() > 0)
			{
				ModelState.AddModelError("WaterMeter.DemountedWaterMeterNum", "Istnieje już miernik o takim numerze");
				ModelState.AddModelError("WaterMeter.MountedWaterMeterNum", "Istnieje już miernik o takim numerze");
			}

			if (ModelState.IsValid)
			{
				_repository.WaterMeterRepository.Insert(waterMeter);
				_repository.SaveChanges();
				return RedirectToAction("Index", "WaterMeter");
			}
			else
			{
				IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.WaterMeter.BuildingId);
				return View(model);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Edit(int? id)
		{
			IMapper mapper = AutoMapperConfigs.WaterMeterEdit().CreateMapper();
			if (id == null)
				return NotFound();
			var waterMeter = _repository.WaterMeterRepository.GetById(id);
			if (waterMeter == null)
				return NotFound();

			var model = new WaterMeterEditViewModel();
			model.WaterMeter = mapper.Map<WaterMeter, WaterMeterViewModel>(waterMeter);
			IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
			model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.WaterMeter.BuildingId);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Edit(WaterMeterEditViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.WaterMeterEdit().CreateMapper();
			var waterMeter = mapper.Map<WaterMeterViewModel, WaterMeter>(model.WaterMeter);

			if (waterMeter.MountedWaterMeterNum != null && waterMeter.MountedWaterMeterNum != "" && waterMeter.DemountedWaterMeterNum != null && waterMeter.DemountedWaterMeterNum != "")
				if (_repository.WaterMeterRepository.SearchFor(wm => wm.BuildingId == waterMeter.BuildingId && waterMeter.MountedWaterMeterNum == wm.MountedWaterMeterNum && wm.DemountedWaterMeterNum == waterMeter.DemountedWaterMeterNum).Count() > 0)
				{
					ModelState.AddModelError("WaterMeter.DemountedWaterMeterNum", "Istnieje już miernik o takim numerze");
					ModelState.AddModelError("WaterMeter.MountedWaterMeterNum", "Istnieje już miernik o takim numerze");
				}

			if (ModelState.IsValid)
			{
				_repository.WaterMeterRepository.Update(waterMeter);
				_repository.SaveChanges();
				return RedirectToAction("Index", "WaterMeter");
			}
			else
			{
				IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.WaterMeter.BuildingId);
				return View(model);
			}
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Delete(int? id)
		{
			if (id == null)
				return NotFound();
			var waterMeter = _repository.WaterMeterRepository.GetById(id);
			if (waterMeter == null)
				return NotFound();

			_repository.WaterMeterRepository.Delete(waterMeter);
			_repository.SaveChanges();
			return RedirectToAction("Index", "WaterMeter");
		}

	}
}
