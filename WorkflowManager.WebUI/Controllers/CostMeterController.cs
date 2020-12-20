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
	public class CostMeterController : Controller
	{
		private readonly ILogger<BuildingController> _logger;
		private readonly WorkflowManagerRepository _repository;

		public CostMeterController(ILogger<BuildingController> logger)
		{
			_logger = logger;
			_repository = new WorkflowManagerRepository();
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Index(int? BuildingId)
        {
			var model = new CostMeterIndexViewModel();
			IMapper mapper = AutoMapperConfigs.CostMeterIndex().CreateMapper();
			IEnumerable<Building> buildings;

			if (User.IsInRole("Technician") && !User.IsInRole("Manager"))
				buildings = _repository.BuildingRepository.SearchFor(b => b.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name) || b.Jobs.Any(j => j.UserJobs.Any(uj => uj.User.UserName == User.Identity.Name)));
			else
				buildings = _repository.BuildingRepository.GetAll();

			if (BuildingId == null)
			{
				ICollection<CostMeter> CostMeters = _repository.CostMeterRepository.GetAll().ToList();
				foreach (var item in CostMeters)
				{
					if (buildings.Any(b => b.Id == item.BuildingId))
						item.Building = buildings.Where(b => b.Id == item.BuildingId).First();	
				}                
				model.CostMeters = mapper.Map<ICollection<CostMeter>, ICollection<CostMeterViewModel>>(CostMeters);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress");
			}
			else
			{
				ICollection<CostMeter> CostMeters = _repository.CostMeterRepository.SearchFor(cm => cm.BuildingId == BuildingId).ToList();
				foreach (var item in CostMeters)
				{
					if (buildings.Any(b => b.Id == item.BuildingId))
						item.Building = buildings.Where(b => b.Id == item.BuildingId).First();
				}
				model.CostMeters = mapper.Map<ICollection<CostMeter>, ICollection<CostMeterViewModel>>(CostMeters);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", BuildingId);
			}

			return View(model);
        }

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Details(int? id)
		{ 
			if (id == null)
				return NotFound();

			var CostMeter = _repository.CostMeterRepository.SearchFor(cm => cm.Id == id)
				.Include(hm => hm.Building)
					.ThenInclude(b => b.UserBuildings)
						.ThenInclude(ub => ub.User)
				.First()
			;
			if (CostMeter == null)
				return NotFound();
			if (!CostMeter.Building.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name) && User.IsInRole("Technician"))
				return StatusCode(403);

			IMapper mapper = AutoMapperConfigs.CostMeterDetails().CreateMapper();
			var model = mapper.Map<CostMeter, CostMeterViewModel>(CostMeter);

			return View(model);
        }

		[HttpGet]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Create(int? buildingId)
		{
			var model = new CostMeterCreateViewModel();
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
		public IActionResult Create(CostMeterCreateViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.CostMeterCreate().CreateMapper();
			var CostMeter = mapper.Map<CostMeterViewModel, CostMeter>(model.CostMeter);

			if(CostMeter.MountedPKONum != null && CostMeter.MountedPKONum != "" && CostMeter.DemountedPKONum != null && CostMeter.DemountedPKONum != "")
			if (_repository.CostMeterRepository.SearchFor(wm => wm.Id != CostMeter.Id && wm.BuildingId == CostMeter.BuildingId && CostMeter.MountedPKONum == wm.MountedPKONum && wm.DemountedPKONum == CostMeter.DemountedPKONum).Count() > 0)
			{
				ModelState.AddModelError("CostMeter.DeMountedPKONum", "Istnieje już miernik o takim numerze");
				ModelState.AddModelError("CostMeter.MountedPKONum", "Istnieje już miernik o takim numerze");
			}

			if (ModelState.IsValid)
			{
				_repository.CostMeterRepository.Insert(CostMeter);
				_repository.SaveChanges();
				return RedirectToAction("Index", "CostMeter");
			}
			else
			{
				IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.CostMeter.BuildingId);
				return View(model);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Edit(int? id)
		{
			IMapper mapper = AutoMapperConfigs.CostMeterEdit().CreateMapper();
			if (id == null)
				return NotFound();
			var CostMeter = _repository.CostMeterRepository.GetById(id);
			if (CostMeter == null)
				return NotFound();

			var model = new CostMeterEditViewModel();
			model.CostMeter = mapper.Map<CostMeter, CostMeterViewModel>(CostMeter);
			IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
			model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.CostMeter.BuildingId);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Edit(CostMeterEditViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.CostMeterEdit().CreateMapper();
			var CostMeter = mapper.Map<CostMeterViewModel, CostMeter>(model.CostMeter);

			if (CostMeter.MountedPKONum != null && CostMeter.MountedPKONum != "" && CostMeter.DemountedPKONum != null && CostMeter.DemountedPKONum != "")
				if (_repository.CostMeterRepository.SearchFor(wm => wm.BuildingId == CostMeter.BuildingId && CostMeter.MountedPKONum == wm.MountedPKONum && wm.DemountedPKONum == CostMeter.DemountedPKONum).Count() > 0)
				{
					ModelState.AddModelError("CostMeter.DeMountedPKONum", "Istnieje już miernik o takim numerze");
					ModelState.AddModelError("CostMeter.MountedPKONum", "Istnieje już miernik o takim numerze");
				}

			if (ModelState.IsValid)
			{
				_repository.CostMeterRepository.Update(CostMeter);
				_repository.SaveChanges();
				return RedirectToAction("Index", "CostMeter");
			}
			else
			{
				IEnumerable<Building> buildings = _repository.BuildingRepository.SearchFor(b => b.Done == false);
				model.BuildingsSelectList = new SelectList(buildings, "Id", "FullAddress", model.CostMeter.BuildingId);
				return View(model);
			}
		}

		[Authorize(Roles = "Admin, Technician, Manager")]
		public IActionResult Delete(int? id)
		{
			if (id == null)
				return NotFound();
			var CostMeter = _repository.CostMeterRepository.GetById(id);
			if (CostMeter == null)
				return NotFound();

			_repository.CostMeterRepository.Delete(CostMeter);
			_repository.SaveChanges();
			return RedirectToAction("Index", "CostMeter");
		}

	}
}
