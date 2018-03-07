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
        DeviceContext dbContext;
        public DataApi()
        {
            dbContext = new DeviceContext();
        }

        public List<DeviceWrapper> GetWrappers(int deviceId)
        {
            List<DeviceWrapper> wrappers = new List<DeviceWrapper>();

            /*
            List<DBSchema> schemas = dbContext.timeschema.Where(p => p.DeviceId == deviceId)
                .OrderByDescending(x => x.DayOfWeek)
                .OrderByDescending(z => z.TimePoint).ToList();
            */
            List<DBSchema> schemas = dbContext.timeschema.Where(p => p.DeviceId == deviceId).ToList();
            foreach (DBSchema schema in schemas)
            {

                DBDevice d = dbContext.devices.Find(schema.DeviceId);
                
                DeviceWrapper wrap = new DeviceWrapper
                    (
                    new Schema
                    (
                        new Device()
                        {
                            DeviceId = d.deviceID,
                            DeviceDescription = d.DeviceType,
                            DeviceName = d.DeviceName
                        }
                        )
                    {
                        WeekDay = schema.DayOfWeek,
                        ActionText = schema.Action.ToString(),
                        TimePoint = schema.TimePoint
                    });

                wrappers.Add(wrap);
            }

            return wrappers;
        }
        public List<DeviceWrapper> GetAllConfiguration
        {
            get
            {
                List<DeviceWrapper> wrappers = new List<DeviceWrapper>();
                foreach (DBDevice d in dbContext.devices)
                {

                    

                    var selection = dbContext.timeschema.Where(p => p.DeviceId == d.deviceID);
                    int n = dbContext.timeschema.Count();

                    foreach (DBSchema schema in selection)
                    {
                        
                        DeviceWrapper wrap = new DeviceWrapper
                            (
                            new Schema
                            (
                                new Device()
                                {
                                    DeviceId = d.deviceID,
                                    DeviceDescription = d.DeviceType,
                                    DeviceName = d.DeviceName
                                }
                                )
                            {
                                WeekDay = schema.DayOfWeek,
                                ActionText = schema.Action.ToString(),
                                TimePoint = schema.TimePoint
                            });

                        wrappers.Add(wrap);
                    }
                }

                return wrappers;
            }
        }

        public void SaveNewDevice(Device device)
        {
            try
            {

                dbContext.devices.Add(new DBDevice() { DeviceName = device.DeviceName, DeviceType = device.DeviceDescription , deviceID=device.DeviceId});
                
                dbContext.SaveChanges();
            }
            catch (Exception ep)
            {
                Exception exception = ep;
            }
            
        }

        private List<Device> _dbDevices { get; } = new List<Device>();
        public List<Device> GetDbDevices
        {
            get
            {
                foreach (DBDevice d in dbContext.devices)
                {
                    _dbDevices.Add(new Device() { DeviceId = d.deviceID, DeviceDescription = d.DeviceType, DeviceName = d.DeviceName });
                }

                return _dbDevices;
            }
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
                return GetAllFakeItems();
            }
        }

        private List<DeviceWrapper> GetAllFakeItems()
        {
            List<DeviceWrapper> wrapper = new List<DeviceWrapper>();

            List<Device> someDevices = GetDevices();

            int n = 1;
            foreach (Device d in someDevices)
            {
                Schema schema = null;
                if (n % 2 == 0)
                    schema = new Schema(d) { ActionText = "ON", TimePoint = DateTime.Now.Add(new TimeSpan(1, 1, 1, 1, 1)), WeekDay = 1 };
                else
                    schema = new Schema(d) { ActionText = "OFF", TimePoint = DateTime.Now.Add(new TimeSpan(-1, 1, -1, 1, -1)), WeekDay = 2 };

                DeviceWrapper wrp = new DeviceWrapper(schema);

                wrapper.Add(wrp);
                n++;
            }



            return wrapper;
        }

        private List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();

            Device device = new Device() { DeviceName = "Nexa 1", DeviceId = 1, DeviceComment = "Första kommentaren kommer här", DeviceDescription = "Första beskrivningen" };
            Device device2 = new Device() { DeviceName = "Nexa 2", DeviceId = 2, DeviceComment = "Andra kommentaren kommer här!!", DeviceDescription = "Andra Beskrivningen" };
            Device device3 = new Device() { DeviceName = "Nexa 3", DeviceId = 3, DeviceComment = "Tredjekommentaren kommer här!", DeviceDescription = "Tredje beskrivningen!" };
            Device device4 = new Device() { DeviceName = "Nexa 4", DeviceId = 4, DeviceComment = "Fjärde kommentaren kommer här!", DeviceDescription = "Fjärde beskrivningen!" };

            devices.Add(device);
            devices.Add(device2);
            devices.Add(device3);
            devices.Add(device4);

            return devices;
        }
    }
}
