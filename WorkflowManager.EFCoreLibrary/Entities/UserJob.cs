using WorkflowManager.EFCoreLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Security.Policy;

namespace WorkflowManager.EFCoreLibrary.Entities
{
	public class UserJob
	{
		[ForeignKey("User")]
		public string UserId { get; set; }
		public virtual User User { get; set; }

		[ForeignKey("Job")]
		public int JobId { get; set; }
		public virtual Job Job { get; set; }
	}
}
