using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkflowManager.EFCoreLibrary.Entities;

namespace WorkflowManager.WebUI.Models
{
    public class JobViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Budynek")]
        public int IdBuilding { get; set; }

        [Display(Name = "Budynek")]
        public BuildingViewModel Building { get; set; }

        [Display(Name = "Opis")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "Data dodania")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Przewidywany czas wykonania")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan PredictedDuration { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Czas wykonania")]
        public TimeSpan Duration { get; set; }

        [Display(Name = "Przewidywana data zrealizowania")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime PredictedDoneDate { get; set; }

        [Display(Name = "Data zrealizowania")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? DoneDate { get; set; }

        [Display(Name = "Priorytet")]
        public int Order { get; set; }


        [Display(Name = "Usunięte")]
        public bool Deleted { get; set; }

        [Display(Name = "Zrealizowane")]
        public bool Done { get; set; }


        [Display(Name = "Przypisani")]
        public IList<UserViewModel> Users { get; set; }
    }

    public class JobIndexViewModel
	{
        public IEnumerable<JobViewModel> Jobs { get; set; }
	}
    public class JobCreateViewModel
	{
        public JobViewModel Job { get; set; }
        
        [Display(Name = "Budynek")]
        public SelectList Buildings { get; set; }
        [Display(Name = "Budynek")]
        public int SelectedBuildingId { get; set; }

        [Display(Name = "Przypisani")]
        public MultiSelectList Users { get; set; }

        [Display(Name = "Przypisani")]
        public IEnumerable<string> SelectedUserIds { get; set; }

	}
    public class JobEditViewModel : JobCreateViewModel { }
}
