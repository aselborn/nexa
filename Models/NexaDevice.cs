using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    [Table("NexaDevice")]
    public class NexaDevice
    {
        [Key]
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }
}
