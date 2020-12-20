
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkflowManager.EFCoreLibrary.Entities;
using WorkflowManager.WebUI.Models;

namespace WorkflowManager.WebUI.Models
{
    public class UserViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Minimalna długość pola {0} wynosi {1} znaków.")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość pola {0} wynosi {1} znaków.")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        public string FullName
		{
            get {
                string fn = FirstName + " " + LastName;
				if (fn == " ")
					return UserName;
				else
					return fn;
			}
			set { }
		}

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Blokada")]
        public bool Lock { get; set; }

        [Display(Name = "Zadania")]
        public IEnumerable<JobViewModel> Jobs { get; set; }

        [Display(Name = "Uprawnienia")]
        public IEnumerable<string> Roles { get; set; }
    }

    public class UserIndexViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }

    public class UserCreateViewModel
    {
        public UserViewModel User { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimalna długość pola {0} wynosi {1} znaków.")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość pola {0} wynosi {1} znaków.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Hasło i Potwierdzenie muszą być takie same.")]
		[Display(Name = "Potwierdź hasło")]
		public string ConfirmPassword { get; set; }

        [Display(Name = "Uprawnienia")]
        public MultiSelectList Roles { get; set; }

        [Display(Name = "Uprawnienia")]
        public IEnumerable<string> SelectedRolesIds { get; set; }

    }

    public class UserEditViewModel
    {
        public UserViewModel User { get; set; }

        [Display(Name = "Uprawnienia")]
        public MultiSelectList Roles { get; set; }

        public MultiSelectList Buildings { get; set; }

        [Display(Name = "Wybrane Role")]
        public IEnumerable<string> SelectedRolesIds { get; set; }

        [Display(Name = "Przypisany do")]
        public IEnumerable<int> SelectedBuildingsIds { get; set; }
    }


}