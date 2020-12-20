
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkflowManager.WebUI.Models
{
    public class HeatMeterViewModel
    {

        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Budynek")]
        public int BuildingId { get; set; }

        [Display(Name = "Budynek")]
        public BuildingViewModel Building { get; set; }

        [Display(Name = "Pomiar")]
        public float Measurement { get; set; }

        private string demountedHeatMeterNum;
        [Display(Name = "Nr Zdemontowanego miernika")]
        public string DemountedHeatMeterNum
        {
            get { if (demountedHeatMeterNum != null) return demountedHeatMeterNum; else return ""; }
            set { demountedHeatMeterNum = value; }
        }

        private string mountedHeatMeterNum;
        [Display(Name = "Nr Zamontowanego miernika")]
        public string MountedHeatMeterNum
        {
            get { if (mountedHeatMeterNum != null) return mountedHeatMeterNum; else return ""; }
            set { mountedHeatMeterNum = value; }
        }

        private string demountState;
        [Display(Name = "Stan Demotażu")]
        public string DemountState
        {
            get { if (demountState != null) return demountState; else return ""; }
            set { demountState = value; }
        }

        private string mountState;
        [Display(Name = "Stan Montażu")]
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

        private string sensorSealNum;
        [Display(Name = "Nr plomby czujnika")]
        public string SensorSealNum
        {
            get { if (sensorSealNum != null) return sensorSealNum; else return ""; }
            set { sensorSealNum = value; }
        }

    }

    public class HeatMeterIndexViewModel
    {
        public ICollection<HeatMeterViewModel> HeatMeters { get; set; }

        public SelectList BuildingsSelectList { get; set; }

        public int BuildingId { get; set; }
    }

    public class HeatMeterCreateViewModel
    {
        public HeatMeterViewModel HeatMeter { get; set; }

        public SelectList BuildingsSelectList { get; set; }
    }

    public class HeatMeterEditViewModel : HeatMeterCreateViewModel
    {

    }
}