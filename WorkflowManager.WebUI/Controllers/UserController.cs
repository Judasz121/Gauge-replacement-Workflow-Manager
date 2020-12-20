using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using WorkflowManager.EFCoreLibrary.DataAccess;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Helpers;
using WorkflowManager.WebUI.Models;

namespace WorkflowManager.WebUI.Controllers
{
	public class UserController : Controller
	{
		private readonly ILogger _logger;
		private readonly WorkflowManagerRepository _repository;
		public UserController(ILogger<JobController> logger)
		{
			_logger = logger;
			_repository = new WorkflowManagerRepository();
		}
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult Index()
		{
			IMapper mapper = AutoMapperConfigs.UserIndex().CreateMapper();
			IEnumerable<User> users;

			if (User.IsInRole("Manager"))
			{
				users = _repository.UserRepository
					.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician"))
					.Include(u => u.UserJobs)
						.ThenInclude(uj => uj.Job)
					.Include(u => u.UserRoles)
						.ThenInclude(ur => ur.Role)
					.Include(u => u.UserRoles)
						.ThenInclude(ur => ur.RoleId)
				;
			}
			else
			{
				users = _repository.UserRepository
					.GetAll()
					.Include(u => u.UserJobs)
						.ThenInclude(uj => uj.Job)
					.Include(u => u.UserRoles)
						.ThenInclude(ur => ur.Role)
				;
			}
			var model = new UserIndexViewModel()
			{
				Users = mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users)
			};

			return View(model);
		}

		[Authorize(Roles = "Admin, Manager")]
		public IActionResult Details(string id)
		{
			IMapper mapper = AutoMapperConfigs.UserDetails().CreateMapper();
			if (id == null)
			{
				return NotFound();
			}

			User user = _repository.UserRepository.SearchFor(u => u.Id == id)
				.Include(u => u.UserJobs)
					.ThenInclude(uj => uj.Job)
				.Include(u => u.UserRoles)
					.ThenInclude(ur => ur.Role)
				.FirstOrDefault()
			;
			if (user == null)
			{
				return NotFound();
			}
			if (!user.UserRoles.Any(ur => ur.Role.Name == "Technician") && !User.IsInRole("Admin"))
				return StatusCode(403);

			user.UserJobs = _repository.UserJobRepository
				.SearchFor(uj => uj.UserId == user.Id)
				.Include(uj => uj.Job)
				.ToList()
			;

			UserViewModel model = mapper.Map<User, UserViewModel>(user);
			return View(model);
		}

		[Authorize(Roles = "Admin, Manager")]
		[HttpGet]
		public IActionResult Create()
		{
			var model = new UserCreateViewModel();
			IEnumerable<Role> roles = _repository.RoleRepository.GetAll();
			model.Roles = new MultiSelectList(roles, "Id", "DisplayName");

			return View(model);
		}

		[Authorize(Roles = "Admin, Manager")]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public IActionResult Create(UserCreateViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.UserCreate().CreateMapper();
			var passwordHasher = new PasswordHasher<User>();

			if (_repository.UserRepository.SearchFor(u => u.UserName.ToUpper() == model.User.UserName.ToUpper()).Count() > 0)
			{
				ModelState.AddModelError("UserName", "Istnieje już użytkownik o takiej nazwie");
			}
			if (_repository.UserRepository.SearchFor(u => u.FirstName == model.User.FirstName && u.LastName == model.User.LastName).Count() > 0 && model.User.FirstName != null && model.User.LastName != null)
			{
				ModelState.AddModelError("FirstName", "Istnieje już użytkownik o takim imieniu i nazwisku");
				ModelState.AddModelError("LastName", "Istnieje już użytkownik o takim imieniu i nazwisku");
			}
			if (_repository.UserRepository.SearchFor(u => u.Email == model.User.Email).Count() > 0 && model.User.Email != null && model.User.Email.Trim() != "")
				ModelState.AddModelError("User.Email", "Istnieje już użytkownik z takim adresem e-mail");

			if (ModelState.IsValid)
			{

				User user = mapper.Map<UserViewModel, User>(model.User);
				if(user.Email != null)
					user.NormalizedEmail = model.User.Email.ToUpper();
				user.NormalizedUserName = model.User.UserName.ToUpper();
				user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
				_repository.UserRepository.Insert(user);
				_repository.SaveChanges();

				if(model.SelectedRolesIds != null)
				foreach (string currId in model.SelectedRolesIds)
				{
					UserRole UR = new UserRole()
					{
						RoleId = currId,
						UserId = user.Id
					};
					_repository.UserRoleRepository.Insert(UR);
				}
				_repository.SaveChanges();
				return RedirectToAction("Index", "User");
			}
			else
			{
				IEnumerable<Role> roles = _repository.RoleRepository.GetAll();
				model.Roles = new MultiSelectList(roles, "Id", "DisplayName", model.SelectedRolesIds);
				return View(model);
			}
		}
		[HttpGet]
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult Edit(string id)
		{
			IMapper mapper = AutoMapperConfigs.UserEdit().CreateMapper();
			if (id == null)
				return NotFound();
			User user;
			if (User.IsInRole("Admin"))
			{
				user = _repository.UserRepository.SearchFor(u => u.Id == id)
					.Include(u => u.UserJobs)
						.ThenInclude(uj => uj.Job)
					.Include(u => u.UserRoles)
						.ThenInclude(ur => ur.Role)
					.Include(u => u.UserBuildings)
						.ThenInclude(ub => ub.Building)
					.FirstOrDefault()
				;
			}
			else
			{
				user = _repository.UserRepository
					.SearchFor(u => u.Id == id && u.UserRoles.Any(ur => ur.Role.Name == "Technician"))
					.Include(u => u.UserJobs)
						.ThenInclude(uj => uj.Job)
					.Include(u => u.UserRoles)
						.ThenInclude(ur => ur.Role)
					.Include(u => u.UserBuildings)
					.FirstOrDefault()
				;
			}
			if (user == null)
				return NotFound();

			UserEditViewModel model = new UserEditViewModel();
			model.User = mapper.Map<User, UserViewModel>(user);

			IEnumerable<string> selectedRoleIds = _repository.UserRoleRepository.SearchFor(ur => ur.UserId == id).Select(ur => ur.Role.Id);
			IEnumerable<int> selectedBuildingsIds = user.UserBuildings.Select(ub => ub.BuildingId);

			model.Roles = new MultiSelectList(_repository.RoleRepository.GetAll(), "Id", "DisplayName", selectedRoleIds);
			model.Buildings = new MultiSelectList(_repository.BuildingRepository.GetAll(), "Id", "FullAddress", selectedBuildingsIds);
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Manager")]
		public IActionResult Edit(UserEditViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.UserEdit().CreateMapper();

			if (_repository.UserRepository.SearchFor(u => u.UserName == model.User.UserName && u.Id != model.User.Id).Count() > 0)
				ModelState.AddModelError("User.UserName", "Istnieje już użytkownik o takim loginie");
			if(_repository.UserRepository.SearchFor(u => u.FirstName == model.User.FirstName && u.LastName == model.User.LastName && u.Id != model.User.Id).Count() > 0)
			{
				ModelState.AddModelError("User.FirstName", "Istnieje już użytkownik o takim imieniu i nazwisku");
				ModelState.AddModelError("User.LastName", "Istnieje już użytkownik o takim imieniu i nazwisku");
			}
			if (_repository.UserRepository.SearchFor(u => u.Email == model.User.Email && u.Id != model.User.Id).Count() > 0 && model.User.Email != null)
				ModelState.AddModelError("User.Email", "Istnieje już użytkownik z takim adresem e-mail");

			if (ModelState.IsValid)
			{
				User user = _repository.UserRepository.SearchFor(u => u.Id == model.User.Id).First();
				//User user = mapper.Map<UserViewModel, User>(model.User);
				user.FirstName = model.User.FirstName;
				user.LastName = model.User.LastName;
				user.UserName = model.User.UserName;
				user.NormalizedUserName = model.User.UserName.ToUpper();
				if (model.User.Email != null)
				{
					user.Email = model.User.Email;
					user.NormalizedEmail = model.User.Email.ToUpper();
				}
				user.PhoneNumber = model.User.PhoneNumber;

				
				_repository.UserRepository.Update(user);

				#region userBuildings
				IEnumerable<UserBuilding> userBuildings = _repository.UserBuildingRepository.SearchFor(u => u.UserId == user.Id);
				if (model.SelectedBuildingsIds != null)
				{
					foreach (int currid in model.SelectedBuildingsIds)
					{
						if (userBuildings.Where(ub => ub.BuildingId == currid).Count() == 0)
						{
							UserBuilding UB = new UserBuilding()
							{
								UserId = user.Id,
								BuildingId = currid
							};
							_repository.UserBuildingRepository.Insert(UB);
						}
					}
					foreach(var ub in userBuildings)
                    {
						if (model.SelectedBuildingsIds.Where(id => id == ub.BuildingId).Count() == 0)
							_repository.UserBuildingRepository.Delete(ub);
                    }
				}
				else
				{
					foreach (var ur in userBuildings)
					{
						_repository.UserBuildingRepository.Delete(ur);
					}
				}
				#endregion

				#region userRoles


				IEnumerable<UserRole> userRoles = _repository.UserRoleRepository.SearchFor(ur => ur.UserId == user.Id);
				if (model.SelectedRolesIds != null)
				{
					foreach (string currid in model.SelectedRolesIds)
					{
						if (userRoles.Where(ub => ub.RoleId == currid).Count() == 0)
						{
							UserRole UR = new UserRole()
							{
								UserId = user.Id,
								RoleId = currid
							};
							_repository.UserRoleRepository.Insert(UR);
						}
					}
					foreach (var ur in userRoles)
					{
						if (model.SelectedRolesIds.Where(id => id == ur.RoleId).Count() == 0)
							_repository.UserRoleRepository.Delete(ur);
					}
				}
				else
				{
					userRoles = _repository.UserRoleRepository.SearchFor(ur => ur.UserId == user.Id);
					foreach (var ur in userRoles)
					{
						_repository.UserRoleRepository.Delete(ur);
					}
				}
				
				#endregion

				_repository.SaveChanges();
				return RedirectToAction("Index", "User");
			}
			else
			{
				model.Roles = new MultiSelectList(_repository.RoleRepository.GetAll(), "Id", "DisplayName", model.SelectedRolesIds);
				model.Buildings = new MultiSelectList(_repository.BuildingRepository.GetAll(), "Id", "FullAddress", model.SelectedBuildingsIds);
				return View(model);
			}
		}

		public IActionResult Lock(string id)
		{
			if (id == null)
				return NotFound();

			var user = _repository.UserRepository.GetById(id);
			if (user == null)
				return NotFound();

			if (user.Lock)
				user.Lock = false;
			else
				user.Lock = true;
			_repository.UserRepository.Update(user);
			_repository.SaveChanges();

			return RedirectToAction("Index", "User");
		}
	}
}
