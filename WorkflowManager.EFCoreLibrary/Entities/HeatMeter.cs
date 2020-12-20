using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkflowManager.EFCoreLibrary.Entities
{
    public class HeatMeter
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Building")]
        public int BuildingId { get; set; }
        public virtual Building Building { get; set; }

        public float? Measurement { get; set; }
        [MaxLength(256)]
        public string DemountedHeatMeterNum { get; set; }
        [MaxLength(256)]
        public string MountedHeatMeterNum { get; set; }
        [MaxLength(256)]
        public string DemountState { get; set; }
        [MaxLength(256)]
        public string MountState { get; set; }
        [MaxLength(256)]
        public string SealNum { get; set; }
        [MaxLength(256)]
        public string SensorSealNum { get; set; }
        



    }
}
