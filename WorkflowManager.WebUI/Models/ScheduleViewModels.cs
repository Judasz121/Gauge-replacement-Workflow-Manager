using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowManager.WebUI.Models
{

	#region User
	public class ScheduleUserIndexViewModel
	{

		public List<JobViewModel> UserJobs { get; set; }
		public SelectList UserSelectList { get; set; }
		public string UserId { get; set; }

	}

	public class ScheduleUserEditViewModel
	{
		public UserViewModel User { get; set; }
		public IEnumerable<JobViewModel> AllJobs { get; set; }

		public string UserId { get; set; }
		public string AssignedJobs { get; set; }
	}

	#endregion

	#region Building
	public class ScheduleBuildingIndexViewModel
	{
		public IEnumerable<JobViewModel> BuildingJobs { get; set; }
		public SelectList BuildingSelectList { get; set; }
		public int? BuildingId { get; set; }
	}

	public class ScheduleBuildingEditViewModel
	{
		public BuildingViewModel Building { get; set; }
		public JobViewModel[] Jobs { get; set; }
		public int BuildingId { get; set; }
		public string JobsOrder { get; set; }
	}
	#endregion
}
