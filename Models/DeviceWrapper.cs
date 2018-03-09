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
            Söndag, Måndag, Tisdag, Onsdag, Torsdag, Fredag, Lördag
        }

        private readonly Schema _schemaItem;
        public DeviceWrapper(Schema schema)
        {
            _schemaItem = schema;
        }

        public string DeviceName => _schemaItem.Device.DeviceName;
        public string DeviceDescription => _schemaItem.Device.DeviceDescription;
        public int WeekDay => _schemaItem.WeekDay;
        public string ActionText => _schemaItem.ActionText == "1" ? "PÅ" : "AV";
        public DateTime TimePoint => _schemaItem.TimePoint;
        public string TimePointAsString => $"{_schemaItem.TimePoint:hh:mm}";
        public string WeekDayAsText => Enum.GetName(typeof(EnumDayOfWeek), (_schemaItem.WeekDay -1));
        public Device GetDevice
        {
            get => _schemaItem.Device;
        }

    }
}
