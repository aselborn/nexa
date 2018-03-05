using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    [Table("timeschema")]
    public class DBSchema
    {
        [Key]
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime TimePoint { get; set; }
        public int Action { get; set; }
        public int DayOfWeek { get; set; }
        
        
        //public DateTime updatedAt { get; set; }


    }

}
