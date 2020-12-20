using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkflowManager.WebUI.Models
{
    public class WaterMeterViewModel
    {
        [Display(Name = "Nr miernika")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Budynek")]
        public int BuildingId { get; set; }

        [Display(Name = "Budynek")]
        public BuildingViewModel Building {get;set;}

        [Display(Name = "Pomiar")]
        public float? Measurement { get; set; }

        
        private string demountedWaterMeterNum;
        [Display(Name = "Nr zdemontowanego miernika")]
        public string DemountedWaterMeterNum
        {
            get { if (demountedWaterMeterNum != null) return demountedWaterMeterNum; else return ""; }
            set { demountedWaterMeterNum = value; }
        }

        
        private string mountedWaterMeterNum;
        [Display(Name = "Nr zamontowanego miernika")]
        public string MountedWaterMeterNum
        {
            get { if (mountedWaterMeterNum != null) return mountedWaterMeterNum; else return ""; }
            set { mountedWaterMeterNum = value; }
        }

        
        private string demountState;
        [Display(Name = "Stan demontażu")]
        public string DemountState
        {
            get { if (demountState != null) return demountState; else return ""; }
            set { demountState = value; }
        }

        
        private string mountState;
        [Display(Name = "Stan montażu")]
        public string MountState
        {
            get { if (mountState != null) return mountState; else return ""; }
            set { mountState = value; }
        }

        
        private string sealNum;
        [Display(Name = "Nr plomby")]
        public string SealNum
        {
            get { if (sealNum != null) return sealNum; else return ""; }
            set { sealNum = value; }
        }


        private string checkValve;
        [Display(Name = "Zaworek zwrotny")]
        public string CheckValve
        {
            get { if (checkValve != null) return checkValve; else return ""; }
            set { checkValve = value; }
        }
    }

    public class WaterMeterIndexViewModel
    {
        public ICollection<WaterMeterViewModel> WaterMeters { get; set; }

        [Display(Name = "Pokaż wodomierze dla")]
        public SelectList BuildingsSelectList { get; set; }
        public int BuildingId { get; set; }
    }

    public class WaterMeterCreateViewModel
	{
        public WaterMeterViewModel WaterMeter { get; set; }

        public SelectList BuildingsSelectList { get; set; }
	}

    public class WaterMeterEditViewModel : WaterMeterCreateViewModel
	{

	}

}