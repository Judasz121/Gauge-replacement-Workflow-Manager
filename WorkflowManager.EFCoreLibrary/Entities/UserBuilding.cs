using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkflowManager.EFCoreLibrary.Entities
{
	public class UserBuilding
	{
		[ForeignKey("Building")]
		public int BuildingId { get; set; }
		public virtual Building Building { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }
		public virtual User User { get; set; }
	}
}
