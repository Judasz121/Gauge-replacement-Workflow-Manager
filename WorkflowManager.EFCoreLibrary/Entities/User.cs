using WorkflowManager.EFCoreLibrary.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace WorkflowManager.EFCoreLibrary.Entities
{
	public class User : IdentityUser
	{
		public User()
		{
			UserJobs = new HashSet<UserJob>();
			UserRoles = new HashSet<UserRole>();
			UserBuildings = new HashSet<UserBuilding>();
		}
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }

        [MaxLength(256)]
		public string FirstName { get; set; }
		[MaxLength(256)]
		public string LastName { get; set; }
		[NotMapped]
		public string FullName {
			get {
				string fn = FirstName + " " + LastName;
				if (fn == " ")
					return UserName;
				else
					return fn;
			} 
			set { } 
		}

		[MaxLength(256)]
		new public string Email { get; set; }

		[MaxLength(12)]
		new public string PhoneNumber { get; set; }

		public bool Lock { get; set; }

		public virtual ICollection<UserJob> UserJobs { get; set; }
		public virtual ICollection<UserBuilding> UserBuildings { get; set; }
		public virtual ICollection<UserRole> UserRoles { get; set; }
	}
}
