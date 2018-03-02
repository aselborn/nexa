using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    public class DeviceWrapper
    {
        public enum EnumDayOfWeek
        {
            Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
        }
        private readonly Schema _schemaItem;
        public DeviceWrapper(Schema schema)
        {
            _schemaItem = schema;
        }

        public string DeviceName => _schemaItem.Device.DeviceName;
        public string WeekDay => _schemaItem.WeekDay;
        public string ActionText => _schemaItem.ActionText;
        public string TimePoint => _schemaItem.TimePoint;



    }
}
