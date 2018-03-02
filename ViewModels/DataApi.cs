using Nexa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.ViewModels
{
    public class DataApi
    {
        
        public DataApi()
        {

        }

        private List<Device> mDevices;
        public List<Device> Devices
        {
            get
            {
                return GetDevices();
            }
            set
            {
                mDevices = value;
            }
        }
            
        public List<DeviceWrapper> AllItems
        {
            get
            {
                return GetAllItems();
            }
        }

        private List<DeviceWrapper> GetAllItems()
        {
            List<DeviceWrapper> list = new List<DeviceWrapper>();

            Device device = new Device() { DeviceName = "Nexa 1", DeviceId = 1, DeviceComment = "Första Devicekommentaren" };
            //Schema schema1 = new Schema(device) { ActionText = "ON", TimePoint = DateTime.Now, WeekDay = "Söndag" };
            //Schema schema2 = new Schema(device) { ActionText = "OFF", TimePoint = DateTime.Now.Add(new TimeSpan(1, 1, 1)), WeekDay = "Söndag" };

            //DeviceWrapper wrapper = new DeviceWrapper(schema1);
            //DeviceWrapper wrapper2 = new DeviceWrapper(schema2);

            //// list.Add(wrapper2);

            
            return list;
        }

        private List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();

            Device device = new Device() { DeviceName = "Nexa 1", DeviceId = 1, DeviceComment = "Första kommentaren kommer här", DeviceDescription="Första beskrivningen" };
            Device device2 = new Device() { DeviceName = "Nexa 2", DeviceId = 2, DeviceComment = "Andra kommentaren kommer här!!", DeviceDescription="Andra Beskrivningen"};
            Device device3 = new Device() { DeviceName = "Nexa 3", DeviceId = 3, DeviceComment = "Tredjekommentaren kommer här!", DeviceDescription="Tredje beskrivningen!" };

            devices.Add(device);
            devices.Add(device2);
            devices.Add(device3);

            return devices;
        }
    }
}
