using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexa.Models
{
    [Table("devices")]
    public class DBDevice 
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int deviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    
    }
}