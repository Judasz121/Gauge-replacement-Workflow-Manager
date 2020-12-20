using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkflowManager.EFCoreLibrary.Entities
{
    public class CostMeter
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Building")]
        public int BuildingId { get; set; }
        public virtual Building Building { get; set; }

        [MaxLength(256)]
        public string Group { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [MaxLength(256)]
        public string DemountedPKONum { get; set; }
        [MaxLength(256)]
        public string MountedPKONum { get; set; }
        public float? CurrMeasurement { get; set; }
        public float? CycleEndMeasurement { get; set; }
        [MaxLength(256)]
        public string Plate { get; set; }

    }
}
