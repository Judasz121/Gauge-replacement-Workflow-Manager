using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkflowManager.EFCoreLibrary.Entities
{
    public class Building
    {
        public Building()
		{
            WaterMeters = new HashSet<WaterMeter>();
            HeatMeters = new HashSet<HeatMeter>();
            CostMeters = new HashSet<CostMeter>();
            Jobs = new HashSet<Job>();
		}

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string City { get; set; }
        [Required]
        [MaxLength(256)]
        public string Street { get; set; }
        [MaxLength(256)]
        public string AddressBuildingNum { get; set; }
        [MaxLength(256)]
        public string AddressAdditional { get; set; }
        [NotMapped]
        public string FullAddress
        {
            get {
                return City + " " + Street + " " + AddressBuildingNum + " " + AddressAdditional;
            }
            set { }
        }

        public virtual ICollection<WaterMeter> WaterMeters { get; set; }
        public virtual ICollection<HeatMeter> HeatMeters { get; set; }
        public virtual ICollection<CostMeter> CostMeters { get; set; }

        public DateTime? Date { get; set; }

        public virtual ICollection<UserBuilding> UserBuildings { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }

        public DateTime DateWorkStart { get; set; }
        public DateTime PredictedWorkEndDate { get; set; }
        public DateTime DateWorkEnd { get; set; }
        public bool Done { get; set; }
        public byte[] ResidentSign { get; set; }


        public int? MountedWaterMetersAmount { get; set; }
        public int? DisposedWaterMetersAmount { get; set; }
        public int? LeftWaterMetersAmount { get; set; }
        [MaxLength(2048)]
        public string WaterMetersRemarks { get; set; }


        public int? MountedHeatMetersAmount { get; set; }
        public int? DisposedHeatMetersAmount { get; set; }
        public int? LeftHeatMetersAmount { get; set; }
        [MaxLength(2048)]
        public string HeatMetersRemarks { get; set; }


        public int? MountedCostMetersAmount { get; set; }
        public int? DisposedCostMetersAmount { get; set; }
        public int? LeftCostMetersAmount { get; set; }
        [MaxLength(2048)]
        public string CostMetersRemarks { get; set; }

        public int? MountedOrReplacedFittingsAmount { get; set; }
        public int? MountedOrReplacedReductionsAmount { get; set; }
        public int? MountedOrReplacedValvesAmount { get; set; }
        public int? MountedCheckValvesAmount { get; set; }
        public int? ReplacedTeesAmount { get; set; }
        public int? SealedWaterMetersAmount { get; set; }



    }
}
