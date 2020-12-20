using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkflowManager.EFCoreLibrary.Entities
{
    public class Role : IdentityRole
    {
        [MaxLength(256)]
		public string DisplayName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
	}
}
