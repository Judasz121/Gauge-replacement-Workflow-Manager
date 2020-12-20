using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkflowManager.WebUI.Models
{
    public class CostMeterViewModel
    {
        [Display(Name = "Nr PKO")]
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Budynek")]
        public int BuildingId { get; set; }
        
        [Display(Name = "Budynek")]
        public BuildingViewModel Building { get; set; }

        private string group;
        [Display(Name = "Grupa")]
        public string Group
        {
            get { if (group != null) return group; else return ""; }
            set { group = value; }
        }

        private string description;
        [Display(Name = "Opis")]
        public string Description
        {
            get { if (description != null) return description; else return ""; }
            set { description = value; }
        }

        private string demountedPKONum;
        [Display(Name = "Nr zdemontowanego miernika")]
        public string DemountedPKONum
        {
            get { if (demountedPKONum != null) return demountedPKONum; else return ""; }
            set { demountedPKONum = value; }
        }

        private string mountedPKONum;
        [Display(Name = "Nr zamontowanego miernika")]
        public string MountedPKONum
        {
            get { if (mountedPKONum != null) return mountedPKONum; else return ""; }
            set { mountedPKONum = value; }
        }

        [Display(Name = "Wskazanie bieżące")]
        public float CurrMeasurement { get; set; }

        [Display(Name = "Wskazanie na koniec okresu")]
        public float CycleEndMeasurement { get; set; }

        private string plate;
        [Display(Name = "Płytka")]
        public string Plate
        {
            get { if (plate != null) return plate; else return ""; }
            set { plate = value; }
        }
    }

    public class CostMeterIndexViewModel
    {
        public ICollection<CostMeterViewModel> CostMeters { get; set; }
        public int BuildingId { get; set; }
        public SelectList BuildingsSelectList { get; set; }
    }

    public class CostMeterCreateViewModel
    {
        public CostMeterViewModel CostMeter { get; set; }

        public SelectList BuildingsSelectList { get; set; }
    }

    public class CostMeterEditViewModel : CostMeterCreateViewModel
    {

    }
}