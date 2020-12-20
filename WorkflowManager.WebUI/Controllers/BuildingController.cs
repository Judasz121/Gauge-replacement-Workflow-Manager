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
using Image = iText.Layout.Element.Image;
using Color = iText.Kernel.Colors.Color;
using Table = iText.Layout.Element.Table;
using Style = iText.Layout.Style;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.IO.Font;
using iText.Layout.Layout;
using Microsoft.AspNetCore.Hosting;

namespace WorkflowManager.WebUI.Controllers
{
	public class BuildingController : Controller
	{
		private readonly ILogger<BuildingController> _logger;
		private readonly WorkflowManagerRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

		public BuildingController(ILogger<BuildingController> logger, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			_repository = new WorkflowManagerRepository();
            _webHostEnvironment = webHostEnvironment;

		}
		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Index()
		{
			IMapper mapper = AutoMapperConfigs.BuildingIndex().CreateMapper();
			IEnumerable<Building> buildings;
			if (User.IsInRole("Technician"))
			{
				buildings = _repository.BuildingRepository
					.SearchFor(b => b.UserBuildings.Any(ub => ub.User.UserName == User.Identity.Name))
					.Include(b => b.UserBuildings)
						.ThenInclude(ub => ub.User)
				;
			}
			else {
				buildings = _repository.BuildingRepository
					.GetAll()
					.Include(b => b.UserBuildings)
						.ThenInclude(ub => ub.User)
				;
			}
			var model = new BuildingIndexViewModel()
			{
				Buildings = mapper.Map<IEnumerable<Building>, IEnumerable<BuildingViewModel>>(buildings)
			};
			return View(model);
		}

		[Authorize(Roles = "Admin, Manager, Technician")]
		public IActionResult Details(int? id)
		{
			IMapper mapper = AutoMapperConfigs.BuildingDetails().CreateMapper();

			if (id == null)
				return NotFound();
			var building = _repository.BuildingRepository
				.SearchFor(b => b.Id == id)
				.Include(b => b.UserBuildings)
					.ThenInclude(ub => ub.User)
				.Include(b => b.Jobs)
				.Include(b => b.WaterMeters)
				.Include(b => b.HeatMeters)
				.Include(b => b.CostMeters)
				.FirstOrDefault()
			;
			if (building == null)
				return NotFound();

			var model = mapper.Map<Building, BuildingViewModel>(building);
			return View(model);
		}

		[Authorize(Roles = "Admin, Manager")]
		[HttpGet]
		public IActionResult Create()
		{
			IMapper mapper = AutoMapperConfigs.BuildingCreate().CreateMapper();

			IEnumerable<User> users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager"));
			var model = new BuildingCreateViewModel()
			{
				Building = new BuildingViewModel(),
				Users = new MultiSelectList(users, "Id", "FullName")
			};

			return View(model);
		}
		[HttpPost]
		[Authorize(Roles = "Admin, Manager")]
		[ValidateAntiForgeryToken]
		public IActionResult Create(BuildingCreateViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.BuildingCreate().CreateMapper();

			if(_repository.BuildingRepository.SearchFor(b => b.City == model.Building.City && b.Street == model.Building.Street && b.AddressBuildingNum == model.Building.AddressBuildingNum).Count() > 0)
			{
				ModelState.AddModelError("Building.City", "Istnieje już budynek o takim adresie");
				ModelState.AddModelError("Building.Street", "Istnieje już budynek o takim adresie");
				ModelState.AddModelError("Building.AddressBuildingNum", "Istnieje już budynek o takim adresie");
			}
			if (ModelState.IsValid)
			{
				Building building = mapper.Map<BuildingViewModel, Building>(model.Building);
				_repository.BuildingRepository.Insert(building);
				_repository.SaveChanges();

                #region userBuildings
                IEnumerable<UserBuilding> userBuildings = _repository.UserBuildingRepository.SearchFor(ub => ub.BuildingId == building.Id).ToList();
				if (model.SelectedUsersIds != null)
				{
					foreach (string currId in model.SelectedUsersIds)
					{
						if (userBuildings.Where(ub => ub.UserId == currId).Count() == 0)
						{
							UserBuilding UB = new UserBuilding()
							{
								BuildingId = building.Id,
								UserId = currId
							};
							_repository.UserBuildingRepository.Insert(UB);
						}
					}
					foreach(var ub in userBuildings)
                    {
						if(model.SelectedUsersIds.Where(id => id == ub.UserId).Count() == 0)
                        {
							_repository.UserBuildingRepository.Delete(ub);
                        }
                    }
				}
                else
                {
					foreach(var ub in userBuildings)
                    {
						_repository.UserBuildingRepository.Delete(ub);
                    }
                }
				#endregion

				_repository.SaveChanges();
				return RedirectToAction("Index", "Building");
			}
			else
			{
				IEnumerable<User> users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager"));
				if (!model.SelectedUsersIds.IsNullOrEmpty())
					model.Users = new MultiSelectList(users, "Id", "FullName", model.SelectedUsersIds);
				else
					model.Users = new MultiSelectList(users, "Id", "FullName");

				return View(model);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Manager, Technician, Admin")]
		public IActionResult Edit(int? id)
		{
			IMapper mapper = AutoMapperConfigs.BuildingEdit().CreateMapper();
			var model = new BuildingEditViewModel();
			if (id == null)
				return NotFound();
			var building = _repository.BuildingRepository
				.SearchFor(b => b.Id == id)
				.Include(b => b.Jobs)
				.Include(b => b.UserBuildings)
					.ThenInclude(ub => ub.User)

				.Include(b => b.CostMeters)
				.Include(b => b.WaterMeters)
				.Include(b => b.HeatMeters)
				.FirstOrDefault()
			;
			if (building == null)
				return NotFound();
			building.Jobs = building.Jobs.Where(j => !j.Done).ToList();
			model.Building = mapper.Map<Building, BuildingViewModel>(building);
			IEnumerable<User> users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager"));
			model.Users = new MultiSelectList(users, "Id", "FullName", building.UserBuildings.Select(ub => ub.User.Id));
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Manager, Technician, Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(BuildingEditViewModel model)
		{
			IMapper mapper = AutoMapperConfigs.BuildingEdit().CreateMapper();

			if (_repository.BuildingRepository.SearchFor(b => b.City == model.Building.City && b.Street == model.Building.Street && b.AddressBuildingNum == model.Building.AddressBuildingNum && b.Id != model.Building.Id).Count() > 0)
			{
				ModelState.AddModelError("Building.City", "Istnieje już budynek o takim adresie");
				ModelState.AddModelError("Building.Street", "Istnieje już budynek o takim adresie");
				ModelState.AddModelError("Building.AddressBuildingNum", "Istnieje już budynek o takim adresie");
			}
			if (ModelState.IsValid)
			{
				Building building = mapper.Map<BuildingViewModel, Building>(model.Building);
				if(!building.ResidentSign.IsNullOrEmpty())
					building.ResidentSign = await ImgToBytes(model.ResidentSign);
				_repository.BuildingRepository.Update(building);
				_repository.SaveChanges();

                #region userbuildings
                IEnumerable<UserBuilding> userBuildings = _repository.UserBuildingRepository.SearchFor(ub => ub.BuildingId == building.Id).ToList();
				if (model.SelectedUsersIds != null)
				{
					foreach (string currId in model.SelectedUsersIds)
					{
						if (userBuildings.Where(ub => ub.UserId == currId).Count() == 0)
						{
							UserBuilding UB = new UserBuilding()
							{
								BuildingId = building.Id,
								UserId = currId
							};
							_repository.UserBuildingRepository.Insert(UB);
						}
					}
					foreach(var ub in userBuildings)
                    {
						if (model.SelectedUsersIds.Where(id => id == ub.UserId).Count() == 0)
							_repository.UserBuildingRepository.Delete(ub);
                    }
				}
                else
                {
					foreach(var ub in userBuildings)
                    {
						_repository.UserBuildingRepository.Delete(ub);
                    }
                }
				#endregion

				_repository.SaveChanges();
				return RedirectToAction("Index", "Building");
			}
			else
			{
				IEnumerable<User> users = _repository.UserRepository.SearchFor(u => u.UserRoles.Any(ur => ur.Role.Name == "Technician" || ur.Role.Name == "Manager"));
				model.Users = new MultiSelectList(users, "Id", "FullName", model.SelectedUsersIds);
				return View(model);
			}
		}
		public async Task<byte[]> ImgToBytes(IFormFile img)
		{
			using (var ms = new MemoryStream())
			{
				await img.CopyToAsync(ms);
				return ms.ToArray();
			}
		}


        #region PDFdownloads

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Technician")]
        public IActionResult DownloadWaterMetersPDF(int id)
        {
            IMapper mapper = AutoMapperConfigs.BuildingDownload().CreateMapper();
            var building = _repository.BuildingRepository.GetById(id);
            if (building == null)
                return NotFound();
            BuildingViewModel model = mapper.Map<Building, BuildingViewModel>(building);

            var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
			#region nullexception solving
			FontProgram fp = FontProgramFactory.CreateFont(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "fonts", "Calibri Regular.ttf"));
			//string fontFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "fonts", "Calibri Regular.ttf");
   //         byte[] fontFileBytes;
   //         using (FileStream fs = new FileStream(fontFilePath, FileMode.Open, FileAccess.Read))
			//{
   //             byte[] buffer = new byte[fs.Length];
   //             fs.Read(buffer, 0, (int)fs.Length);
   //             fontFileBytes = buffer;
			//}
   //         FontProgram fp = FontProgramFactory.CreateFont(fontFileBytes);
   //         PdfFont CalibriRegular = PdfFontFactory.CreateFont(fp);

            PdfFont CalibriRegular = PdfFontFactory.CreateFont(fp/*, PdfEncodings.CP1250, true*/);
            //PdfFont CalibriRegular = PdfFontFactory.CreateFont(fp);
            #endregion
            fp = FontProgramFactory.CreateFont(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "fonts", "Calibri Bold.ttf"));
            PdfFont CalibriBold = PdfFontFactory.CreateFont(fp, PdfEncodings.CP1250, true);

            Color backgroundGray = new DeviceRgb(208, 206, 206);
            Color headerGray = new DeviceRgb(150, 150, 150);

            float pgphlead;
            Table table;
            Cell infoCell;
            Cell dataCell;


            #region header
            //STYLE
            Style headerStyle = new Style()
                .SetFont(CalibriRegular)
                .SetFontSize(22)
                .SetFontColor(headerGray)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop((float)-20)
            ;
            //CONTENT
            document.Add(new Paragraph()
                .Add(new Text("Karta montażu/wymiany wodomierzy"))
                .AddStyle(headerStyle)
                );
            #endregion

            #region addressTable
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetBackgroundColor(backgroundGray)
                .SetPaddingTop((float)4)
                .SetPaddingBottom((float)0)
                .SetWidth(UnitValue.CreatePercentValue((float)28.75))
            ;
            dataCell = new Cell()
                .SetPaddingTop((float)4)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue((float)28.75))
            ;
            //Image headerLogo = new Image(ImageDataFactory.Create(Server.MapPath(@"/Images/progmatBlackWhiteLogo.png")))
            Image headerLogo = new Image(ImageDataFactory.Create(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "companylogo.png")))
                .SetWidth(UnitValue.CreatePercentValue(100))
            ;
            Cell headerLogoCell = new Cell(4, 1)
                .Add(headerLogo)
                .SetPadding(15)
                .SetWidth(UnitValue.CreatePercentValue((float)42.5))
            ;
            table = new Table(3)
                .UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
            ;
            pgphlead = 9;
            //CONTENT
            table.AddCell(headerLogoCell);
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer obiektu:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.AddressBuildingNum)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Miasto:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.City)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Ulica:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.Street)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr Lokalu:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.AdditionalAddress)).SetFixedLeading(pgphlead)));

            document.Add(table);
            #endregion

            #region waterMetersTable
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(9)
                .SetHeight(30)
                .SetPaddingLeft(5)
                .SetPaddingRight(5)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(9)
            ;
            table = new Table(7).UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetMarginTop(13)
            ;
            pgphlead = 9;
            //CONTENT
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Pom.")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer wodomierza\nzdemontowanego")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Stan \n demontażu")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer\nwodomierza\nzamontowanego")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Stan\nmontażu")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr plomby")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Zaworek \n zwrotny")).SetFixedLeading(pgphlead)));
            foreach (WaterMeterViewModel wm in model.WaterMeters)
            {
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.Measurement.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.DemountedWaterMeterNum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.DemountState.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.MountedWaterMeterNum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.MountState.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.SealNum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(wm.CheckValve.ToString()))));
            }

            document.Add(table);
            #endregion

            #region additionalInfo
            //STYLE
            Cell headerCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(10)
            ;
            infoCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(8)
                .SetHeight(12)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(8)
                .SetHeight(12)
            ;
            table = new Table(2)
                .SetWidth(UnitValue.CreatePercentValue(36))
                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                .SetMarginTop(13)
            ;
            pgphlead = 10;
            //CONTENT
            table.AddCell(headerCell.Clone(false).Add(new Paragraph(new Text("Informacje dodatkowe:")).SetFixedLeading(pgphlead)));
            table.AddCell(new Cell());
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana/montaż śrubunków, szt.")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedOrReplacedFittingsAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana/montaż redukcji, szt./DN")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedOrReplacedReductionsAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana/montaż zaworu, szt./DN")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedOrReplacedValvesAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Montaż zaworków zwrotnych, szt.")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedCheckValvesAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Plombowanie wodomierzy, szt.")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.SealedWaterMetersAmount.ToString())).SetFixedLeading(pgphlead)));

            document.Add(table);

            #endregion
            LayoutResult result = table.CreateRendererSubTree().SetParent(document.GetRenderer()).Layout(new LayoutContext(new LayoutArea(1, new iText.Kernel.Geom.Rectangle(0, 0, 400, 10000.0F))));
            float prevTableHeight = result.GetOccupiedArea().GetBBox().GetHeight();

            #region waterMetersAmounts
            //STYLE
            infoCell = new Cell(2, 1)
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue(30))
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            headerCell = new Cell(1, 2)
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue(10))
            ;
            Cell dataCell2ColSpan = new Cell(1, 2)
                .SetFont(CalibriRegular)
                .SetFontSize(11)
                .SetMinHeight(28)
            ;
            table = new Table(3)
                .SetWidth(UnitValue.CreatePercentValue(57))
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetRelativePosition(0, 0, 0, prevTableHeight - 13);
            ;
            pgphlead = 12;

            //CONTENT
            table.AddCell(headerCell.Clone(false).Add(new Paragraph(new Text("Liczba zamontowanych wodomierzy")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedWaterMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Zdemontowane wodomierze:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text("Przekazane do utylizacji:")).SetFixedLeading(pgphlead)).SetWidth(UnitValue.CreatePercentValue(40)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.DisposedWaterMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text("Pozostawione w lokalu:")).SetFixedLeading(pgphlead)).SetWidth(UnitValue.CreatePercentValue(40)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.LeftWaterMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Uwagi:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell2ColSpan.Add(new Paragraph(new Text(model.WaterMetersRemarks)).SetFixedLeading(pgphlead)));

            document.Add(table);
            #endregion

            LineSeparator LS = new LineSeparator(new SolidLine(1))
                .SetRelativePosition(0, 0, 0, 65)
            //.SetMarginTop(13);
            ;
            document.Add(LS);

            #region dateAndSign
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetBackgroundColor(backgroundGray)
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(13)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetHeight(20)
            ;
            table = new Table(3)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetRelativePosition(0, 0, 0, 49)
            // .SetWidth(UnitValue.CreatePercentValue(50))
            ;
            bool IsResidentSignNull;
            Image ResidentSign = null;
            try
            {
                ImageData ID = ImageDataFactory.Create(model.ResidentSign);
                ResidentSign = new Image(ID);
                IsResidentSignNull = false;
                ResidentSign.SetHeight(UnitValue.CreatePercentValue(100));
                ResidentSign.SetWidth(100);
            }
            catch (iText.IO.IOException)
            {
                IsResidentSignNull = true;
            }
            float infopgphlead = 8;
            pgphlead = 16;

            //CONTENT
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Technik:")).SetFixedLeading(infopgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Data:")).SetFixedLeading(infopgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Podpis lokatora:")).SetFixedLeading(infopgphlead)));

            Cell techniciansCell = dataCell.Clone(false);
            foreach(var item in model.Users.Where(u => u.Roles.Any(r => r == "Technician")))
			{
                techniciansCell.Add(new Paragraph(new Text(item.FullName)).SetFixedLeading(pgphlead));
			}
            table.AddCell(techniciansCell);

            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.DateWorkEnd.ToString())).SetFixedLeading(pgphlead)));

            if (IsResidentSignNull)
                table.AddCell(dataCell.Clone(false));
            else
                table.AddCell(dataCell.Clone(false).Add(ResidentSign));

            document.Add(table);
            #endregion

            document.Close();
            byte[] fileBytes = ms.ToArray();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, "Protokół montażowy wodomierzy");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Technician")]
        public ActionResult DownloadHeatMetersPDF(int id)
        {
            IMapper mapper = AutoMapperConfigs.BuildingDownload().CreateMapper();
            var building = _repository.BuildingRepository.GetById(id);
            if (building == null)
                return NotFound();
            BuildingViewModel model = mapper.Map<Building, BuildingViewModel>(building);

            var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            //FontProgram fp = FontProgramFactory.CreateFont(Server.MapPath(@"/fonts/Calibri Regular.ttf"));
            FontProgram fp = FontProgramFactory.CreateFont(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "fonts", "Calibri Regular.ttf"));
            PdfFont CalibriRegular = PdfFontFactory.CreateFont(fp, PdfEncodings.CP1250, true);
            //fp = FontProgramFactory.CreateFont(Server.MapPath(@"/fonts/Calibri Bold.ttf"));
            fp = FontProgramFactory.CreateFont(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "fonts", "Calibri Bold.ttf"));
            PdfFont CalibriBold = PdfFontFactory.CreateFont(fp, PdfEncodings.CP1250, true);

            Color backgroundGray = new DeviceRgb(208, 206, 206);
            Color headerGray = new DeviceRgb(150, 150, 150);

            float pgphlead;
            Table table;
            Cell infoCell;
            Cell dataCell;

            #region header
            //STYLE
            Style headerStyle = new Style()
                .SetFont(CalibriRegular)
                .SetFontSize(22)
                .SetFontColor(headerGray)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop((float)-20)
            ;
            //CONTENT
            document.Add(new Paragraph()
                .Add(new Text("Karta montażu/wymiany ciepłomierzy"))
                .AddStyle(headerStyle)
                );
            #endregion

            #region addressTable
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetBackgroundColor(backgroundGray)
                .SetPaddingTop((float)4)
                .SetPaddingBottom((float)0)
                .SetWidth(UnitValue.CreatePercentValue((float)28.75))
            ;
            dataCell = new Cell()
                .SetPaddingTop((float)4)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue((float)28.75))
            ;
            //Image headerLogo = new Image(ImageDataFactory.Create(Server.MapPath(@"/Images/progmatBlackWhiteLogo.png")))
            Image headerLogo = new Image(ImageDataFactory.Create(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "companylogo.png")))
                .SetWidth(UnitValue.CreatePercentValue(100))
            ;
            Cell headerLogoCell = new Cell(4, 1)
                .Add(headerLogo)
                .SetPadding(15)
                .SetWidth(UnitValue.CreatePercentValue((float)42.5))
            ;
            table = new Table(3)
                .UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
            ;
            pgphlead = 9;
            //CONTENT
            table.AddCell(headerLogoCell);
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer obiektu:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.AddressBuildingNum)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Miasto:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.City)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Ulica:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.Street)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr Lokalu:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.AdditionalAddress)).SetFixedLeading(pgphlead)));

            document.Add(table);
            #endregion

            #region heatMetersTable
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(9)
                .SetHeight(30)
                .SetPaddingLeft(5)
                .SetPaddingRight(5)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(9)
            ;
            table = new Table(7).UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetMarginTop(13)
            ;
            pgphlead = 9;
            //CONTENT
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Pom.")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer ciepłomierza \n zdemontowanego")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Stan \n demontażu")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer \n ciepłomierza \n zamontowanego")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Stan \n montażu")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr plomby")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr plomby \n czujnika")).SetFixedLeading(pgphlead)));
            foreach (HeatMeterViewModel item in model.HeatMeters)
            {
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.Measurement.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.DemountedHeatMeterNum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.DemountState.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.MountedHeatMeterNum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.MountState.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.SealNum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.SensorSealNum.ToString()))));
            }

            document.Add(table);
            #endregion

            #region additionalInfo
            //STYLE
            Cell headerCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(10)
            ;
            infoCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(8)
                .SetHeight(12)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(8)
                .SetHeight(12)
            ;
            table = new Table(2)
                .SetWidth(UnitValue.CreatePercentValue(36))
                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                .SetMarginTop(13)
            ;
            pgphlead = 10;
            //CONTENT
            table.AddCell(headerCell.Clone(false).Add(new Paragraph(new Text("Informacje dodatkowe:")).SetFixedLeading(pgphlead)));
            table.AddCell(new Cell());
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana/montaż śrubunków, szt.")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedOrReplacedFittingsAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana/montaż redukcji, szt./DN")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedOrReplacedReductionsAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana/montaż zaworu, szt./DN")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedOrReplacedValvesAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wymiana trójnika, szt.")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.ReplacedTeesAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Plombowanie wodomierzy, szt.")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.SealedWaterMetersAmount.ToString())).SetFixedLeading(pgphlead)));

            document.Add(table);

            #endregion
            LayoutResult result = table.CreateRendererSubTree().SetParent(document.GetRenderer()).Layout(new LayoutContext(new LayoutArea(1, new iText.Kernel.Geom.Rectangle(0, 0, 400, 10000.0F))));
            float prevTableHeight = result.GetOccupiedArea().GetBBox().GetHeight();

            #region heatMetersAmounts
            //STYLE
            infoCell = new Cell(2, 1)
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue(30))
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            headerCell = new Cell(1, 2)
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue(10))
            ;
            Cell dataCell2ColSpan = new Cell(1, 2)
                .SetFont(CalibriRegular)
                .SetFontSize(11)
                .SetMinHeight(28)
            ;
            table = new Table(3)
                .SetWidth(UnitValue.CreatePercentValue(57))
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetRelativePosition(0, 0, 0, prevTableHeight - 13);
            ;
            pgphlead = 12;

            //CONTENT
            table.AddCell(headerCell.Clone(false).Add(new Paragraph(new Text("Liczba zamontowanych ciepłomierzy")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedHeatMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Zdemontowane ciepłomierze:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text("Przekazane do utylizacji:")).SetFixedLeading(pgphlead)).SetWidth(UnitValue.CreatePercentValue(40)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.DisposedHeatMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text("Pozostawione w lokalu:")).SetFixedLeading(pgphlead)).SetWidth(UnitValue.CreatePercentValue(40)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.LeftHeatMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Uwagi:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell2ColSpan.Add(new Paragraph(new Text(model.HeatMetersRemarks)).SetFixedLeading(pgphlead)));

            document.Add(table);
            #endregion

            LineSeparator LS = new LineSeparator(new SolidLine(1))
                .SetRelativePosition(0, 0, 0, 65)
            ;
            document.Add(LS);

            #region dateAndSign
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetBackgroundColor(backgroundGray)
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(13)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetHeight(20)
            ;
            table = new Table(3)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetRelativePosition(0, 0, 0, 49)
            // .SetWidth(UnitValue.CreatePercentValue(50))
            ;
            bool IsResidentSignNull;
            Image ResidentSign = null;
            try
            {
                ImageData ID = ImageDataFactory.Create(model.ResidentSign);
                ResidentSign = new Image(ID);
                IsResidentSignNull = false;
                ResidentSign.SetHeight(UnitValue.CreatePercentValue(100));
                ResidentSign.SetWidth(100);
            }
            catch (iText.IO.IOException)
            {
                IsResidentSignNull = true;
            }
            float infopgphlead = 8;
            pgphlead = 16;

            //CONTENT
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Technik:")).SetFixedLeading(infopgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Data:")).SetFixedLeading(infopgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Podpis lokatora:")).SetFixedLeading(infopgphlead)));
            Cell techniciansCell = dataCell.Clone(false);
            foreach (var item in model.Users.Where(u => u.Roles.Any(r => r == "Technician")))
            {
                techniciansCell.Add(new Paragraph(new Text(item.FullName)).SetFixedLeading(pgphlead));
            }
            table.AddCell(techniciansCell);
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.DateWorkEnd.ToString())).SetFixedLeading(pgphlead)));
            if (IsResidentSignNull)
                table.AddCell(dataCell.Clone(false));
            else
                table.AddCell(dataCell.Clone(false).Add(ResidentSign));

            document.Add(table);
            #endregion

            document.Close();
            byte[] fileBytes = ms.ToArray();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, "Protokół montażowy kosztomierzy");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Technician")]
        public ActionResult DownloadCostMetersPDF(int id)
        {
            IMapper mapper = AutoMapperConfigs.BuildingDownload().CreateMapper();
            var building = _repository.BuildingRepository.GetById(id);
            if (building == null)
                return new StatusCodeResult(404);
            BuildingViewModel model = mapper.Map<Building, BuildingViewModel>(building);

            var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            //FontProgram fp = FontProgramFactory.CreateFont(Server.MapPath(@"/fonts/Calibri Regular.ttf"));
            FontProgram fp = FontProgramFactory.CreateFont(Path.Combine(_webHostEnvironment.ContentRootPath, "fonts", "Calibri Regular.ttf"));
            PdfFont CalibriRegular = PdfFontFactory.CreateFont(fp, PdfEncodings.CP1250, true);
            //fp = FontProgramFactory.CreateFont(Server.MapPath(@"/fonts/Calibri Bold.ttf"));
            fp = FontProgramFactory.CreateFont(Path.Combine(_webHostEnvironment.ContentRootPath, "fonts", "Calibri Bold.ttf"));
            PdfFont CalibriBold = PdfFontFactory.CreateFont(fp, PdfEncodings.CP1250, true);

            Color backgroundGray = new DeviceRgb(208, 206, 206);
            Color headerGray = new DeviceRgb(150, 150, 150);

            float pgphlead;
            Table table;
            Cell infoCell;
            Cell dataCell;

            #region header
            //STYLE
            Style headerStyle = new Style()
                .SetFont(CalibriRegular)
                .SetFontSize(22)
                .SetFontColor(headerGray)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop((float)-20)
            ;
            //CONTENT
            document.Add(new Paragraph()
                .Add(new Text("Karta montażu/wymiany podzielników kosztów c.o."))
                .AddStyle(headerStyle)
                );
            #endregion

            #region addressTable
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetBackgroundColor(backgroundGray)
                .SetPaddingTop((float)4)
                .SetPaddingBottom((float)0)
                .SetWidth(UnitValue.CreatePercentValue((float)28.75))
            ;
            dataCell = new Cell()
                .SetPaddingTop((float)4)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue((float)28.75))
            ;
            Image headerLogo = new Image(ImageDataFactory.Create(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "companylogo.png")))
                .SetWidth(UnitValue.CreatePercentValue(100))
            ;
            Cell headerLogoCell = new Cell(4, 1)
                .Add(headerLogo)
                .SetPadding(15)
                .SetWidth(UnitValue.CreatePercentValue((float)42.5))
            ;
            table = new Table(3)
                .UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
            ;
            pgphlead = 9;
            //CONTENT
            table.AddCell(headerLogoCell);
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Numer obiektu:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.AddressBuildingNum)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Miasto:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.City)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Ulica:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.Street)).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr Lokalu:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.AdditionalAddress)).SetFixedLeading(pgphlead)));

            document.Add(table);
            #endregion

            #region heatMetersTable
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(9)
                .SetHeight(30)
                .SetPaddingLeft(5)
                .SetPaddingRight(5)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(9)
            ;
            table = new Table(8).UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetMarginTop(13)
            ;
            pgphlead = 9;
            //CONTENT
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Pom.")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Grupa \n grzejnika")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Opis grzejnika")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr zdemontowanego pko")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wskazanie \n bieżące")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Wskazanie na koniec okresu")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Nr zamontowanego pko")).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Płytka")).SetFixedLeading(pgphlead)));
            foreach (CostMeterViewModel item in model.CostMeters)
            {
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.CurrMeasurement.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.Group.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.Description.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.DemountedPKONum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.CurrMeasurement.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.CycleEndMeasurement.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.MountedPKONum.ToString()))));
                table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(item.Plate.ToString()))));
            }

            document.Add(table);

            var tableLegend = new Paragraph()
                .SetFont(CalibriRegular)
                .SetFontSize(8)
                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                .SetFixedLeading(10)

                .Add(new Text("Opis sposobu oznaczeń grzejnika \n" +
                "NB - grzejnik niewyposażony w podzielnik kosztów ogrzewania; \n" +
                "VP - grzejnik zaplombowany, niepodłączony do instalacji;"))
            ;
            document.Add(tableLegend);
            #endregion

            #region costMetersAmounts
            //STYLE
            infoCell = new Cell(2, 1)
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue(30))
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            Cell headerCell = new Cell(1, 2)
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(11)
                .SetWidth(UnitValue.CreatePercentValue(10))
            ;
            Cell dataCell2ColSpan = new Cell(1, 2)
                .SetFont(CalibriRegular)
                .SetFontSize(11)
                .SetMinHeight(28)
            ;
            table = new Table(3)
                .SetWidth(UnitValue.CreatePercentValue(57))
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetRelativePosition(0, 0, 0, 0);
            ;
            pgphlead = 12;

            //CONTENT
            table.AddCell(headerCell.Clone(false).Add(new Paragraph(new Text("Liczba zamontowanych PKO:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.MountedCostMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Zdemontowane PKO:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text("Przekazane do utylizacji:")).SetFixedLeading(pgphlead)).SetWidth(UnitValue.CreatePercentValue(40)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.DisposedCostMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text("Pozostawione w lokalu:")).SetFixedLeading(pgphlead)).SetWidth(UnitValue.CreatePercentValue(40)));
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.LeftCostMetersAmount.ToString())).SetFixedLeading(pgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Uwagi:")).SetFixedLeading(pgphlead)));
            table.AddCell(dataCell2ColSpan.Add(new Paragraph(new Text(model.CostMetersRemarks)).SetFixedLeading(pgphlead)));

            document.Add(table);
            #endregion

            LineSeparator LS = new LineSeparator(new SolidLine(1))
                .SetMarginTop(13)
            ;
            document.Add(LS);

            #region dateAndSign
            //STYLE
            infoCell = new Cell()
                .SetFont(CalibriBold)
                .SetFontSize(11)
                .SetBackgroundColor(backgroundGray)
                .SetTextAlignment(TextAlignment.CENTER)
            ;
            dataCell = new Cell()
                .SetFont(CalibriRegular)
                .SetFontSize(13)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetHeight(20)
            ;
            table = new Table(3)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetMarginTop(13)
            ;
            bool IsResidentSignNull;
            Image ResidentSign = null;
            try
            {
                ImageData ID = ImageDataFactory.Create(model.ResidentSign);
                ResidentSign = new Image(ID);
                IsResidentSignNull = false;
                ResidentSign.SetHeight(UnitValue.CreatePercentValue(100));
                ResidentSign.SetWidth(100);
            }
            catch (iText.IO.IOException)
            {
                IsResidentSignNull = true;
            }
            float infopgphlead = 8;
            pgphlead = 16;

            //CONTENT
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Technik:")).SetFixedLeading(infopgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Data:")).SetFixedLeading(infopgphlead)));
            table.AddCell(infoCell.Clone(false).Add(new Paragraph(new Text("Podpis lokatora:")).SetFixedLeading(infopgphlead)));
            Cell techniciansCell = dataCell.Clone(false);
            foreach (var item in model.Users.Where(u => u.Roles.Any(r => r == "Technician")))
            {
                techniciansCell.Add(new Paragraph(new Text(item.FullName)).SetFixedLeading(pgphlead));
            }
            table.AddCell(techniciansCell);
            table.AddCell(dataCell.Clone(false).Add(new Paragraph(new Text(model.DateWorkEnd.ToString())).SetFixedLeading(pgphlead)));
            if (IsResidentSignNull)
                table.AddCell(dataCell.Clone(false));
            else
                table.AddCell(dataCell.Clone(false).Add(ResidentSign));

            document.Add(table);
            #endregion

            document.Close();
            byte[] fileBytes = ms.ToArray();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, "Protokół montażowy PKO");
        }


        #endregion

    }
}
