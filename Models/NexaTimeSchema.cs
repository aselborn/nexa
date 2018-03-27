using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexa.Models
{
    [Table("NexaTimeSchema")]
    public class NexaTimeSchema
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("DeviceId")]
        public int DeviceId { get; set; }

        public DateTime TimePoint { get; set; }

        public int Action { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int Dayofweek { get; set; }
    }
}