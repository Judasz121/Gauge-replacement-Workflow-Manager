using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowManager.EFCoreLibrary.Entities;

namespace WorkflowManager.WebUI.Models
{
	public class HomeIndexViewModel
	{
		public IEnumerable<JobViewModel> CurrentJobs { get; set; }
		public IEnumerable<JobViewModel> DoneJobs { get; set; }
	}
}
