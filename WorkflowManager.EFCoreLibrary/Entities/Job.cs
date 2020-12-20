using WorkflowManager.EFCoreLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkflowManager.EFCoreLibrary.Entities
{
    public class Job
    {
        public Job()
		{
            UserJobs = new HashSet<UserJob>();
		}
        [Key]
        public int Id { get; set; }

        [ForeignKey("Building")]
        public int IdBuilding { get; set; }
        public virtual Building Building { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public TimeSpan PredictedDuration { get; set; }

        private int order;
        public int Order
		{
			get
			{
                return order;
			}
			set
			{
                if (value != 0)
                    order = value;
                else
                    throw new Exception("Job.order cannot be equal to zero");

			}
		}

        public bool Deleted { get; set; }

        public bool Done { get; set; }
        
        public DateTime DoneDate { get; set; }

        public DateTime PredictedDoneDate { get; set; }

        public virtual ICollection<UserJob> UserJobs { get; set; }
    }
}
