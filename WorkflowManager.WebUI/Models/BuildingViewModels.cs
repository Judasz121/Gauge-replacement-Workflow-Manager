using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WorkflowManager.WebUI.Models
{
    public class BuildingViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Nr Budynku")]
        public string AddressBuildingNum { get; set; }

        [Display(Name = "Dodatkowy Adres")]
        public string AdditionalAddress { get; set; }

        [Display(Name = "Adres Budynku")]
        public string FullAddress
		{
			get
			{
                return City + " " + Street + " " + AddressBuildingNum + " " + AdditionalAddress;
            }
			set { }
		}


        public List<WaterMeterViewModel> WaterMeters { get; set; }
        public List<HeatMeterViewModel> HeatMeters { get; set; }
        public List<CostMeterViewModel> CostMeters { get; set; }

        [Display(Name = "Data rozpoczęcia")]
        public DateTime DateWorkStart { get; set; }

        [Display(Name = "Przewidywana data zakończenia")]
        public DateTime PredictedWorkEndDate { get; set; }

        [Display(Name = "Data zakończenia")]
        public DateTime DateWorkEnd { get; set; }

        [Display(Name = "Przypisani Użytkownicy")]
        public ICollection<UserViewModel> Users { get; set; }

        [Display(Name = "Zadania")]
        public ICollection<JobViewModel> Jobs { get; set; }

        [Display(Name = "Podpis lokatora")]
        public string ResidentSign { get; set; } // base64

        [Display(Name = "Ukończono prace")]
		public bool Done { get; set; }


		[Display(Name = "Zamontowane")]
        public int? MountedHeatMetersAmount { get; set; }

        [Display(Name = "Zutylizowane")]
        public int? DisposedHeatMetersAmount { get; set; }

        [Display(Name = "Pozostawione w lokalu")]
        public int? LeftHeatMetersAmount { get; set; }

        private string heatMeterRemarks;
        [Display(Name = "Uwagi dodatkowe")]
        public string HeatMetersRemarks
        {
            get { if (heatMeterRemarks != null) return heatMeterRemarks; else return ""; }
            set { heatMeterRemarks = value; }
        }


        [Display(Name = "Zamontowane")]
        public int? MountedWaterMetersAmount { get; set; }

        [Display(Name = "Zutylizowane")]
        public int? DisposedWaterMetersAmount { get; set; }

        [Display(Name = "Pozostawione w lokalu")]
        public int? LeftWaterMetersAmount { get; set; }

        private string waterMetersRemarks;
        [Display(Name = "Uwagi dodatkowe")]
        public string WaterMetersRemarks
        {
            get { if (waterMetersRemarks != null) return waterMetersRemarks; else return ""; }
            set { waterMetersRemarks = value; }
        }


        [Display(Name = "Zamontowane")]
        public int? MountedCostMetersAmount { get; set; }

        [Display(Name = "Zutylizowane")]
        public int? DisposedCostMetersAmount { get; set; }

        [Display(Name = "Pozostawione w lokalu")]
        public int? LeftCostMetersAmount { get; set; }

        private string costMetersRemarks;
        [Display(Name = "Uwagi dodatkowe")]
        public string CostMetersRemarks
        {
            get { if (costMetersRemarks != null) return costMetersRemarks; else return ""; }
            set { costMetersRemarks = value; }
        }


        [Display(Name = "Wymiana/montaż śrubunków, szt.")]
        public int? MountedOrReplacedFittingsAmount { get; set; }

        [Display(Name = "Wymiana/montaż redukcji, szt./DN")]
        public int? MountedOrReplacedReductionsAmount { get; set; }
        
        [Display(Name = "Wymiana/montaż zaworu,szt./DN")]
        public int? MountedOrReplacedValvesAmount { get; set; }

        [Display(Name = "Montaż zaworków zwrotnych, szt. ")]
        public int? MountedCheckValvesAmount { get; set; }

        [Display(Name = "Wymiana trójników szt.")]
        public int? ReplacedJointsAmount { get; set; }

        [Display(Name = "Plombowanie wodomierzy, szt.")]
        public int? SealedWaterMetersAmount { get; set; }
    }

    public class BuildingCreateViewModel
	{
        public BuildingViewModel Building { get; set; }
       
		public MultiSelectList Users { get; set; }

        [Display(Name = "Przypisani")]
        public IEnumerable<string> SelectedUsersIds { get; set; }
	}

    public class BuildingEditViewModel : BuildingCreateViewModel
    {
        public ICollection<HeatMeterViewModel> HeatMeters { get; set; }
        public bool HeatMetersError { get; set; }
        public ICollection<WaterMeterViewModel> WaterMeters{ get; set; }
        public bool WaterMetersError { get; set; }
        public ICollection<CostMeterViewModel> CostMeters { get; set; }
        public bool CostMetersError { get; set; }
        public bool JobError { get; set; }

        [Display(Name = "Podpis Lokatora")]
        public IFormFile ResidentSign { get; set; }
    }

    public class BuildingIndexViewModel
    {
        public IEnumerable<BuildingViewModel> Buildings { get; set; }
    }
}