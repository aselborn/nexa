using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nexa.Models
{
    [XmlRoot("Devices")]
    public class DeviceObject
    {
        [XmlElement("DeviceConfig")]
        public string DeviceName { get; set; }

        [XmlElement("DeviceId")]
        public int DeviceId { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

    }
}
