using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexa.Models
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int PersonID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }


    }

    
}