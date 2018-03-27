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
            List<NexaTimeSchema> timeschemas = dbContext.NexaTimeSchema.Where(p => p.DeviceId == deviceId).ToList();
            /*
            List<DBSchema> schemas = dbContext.timeschema.Where(p => p.DeviceId == deviceId)
                .OrderByDescending(x => x.DayOfWeek)
                .OrderByDescending(z => z.TimePoint).ToList();
            */
            List<DBSchema> schemas = dbContext.timeschema.Where(p => p.DeviceId == deviceId).ToList();
            schemas = schemas.OrderBy(p => p.DayOfWeek).ThenBy(z => z.TimePoint).ToList();

            timeschemas.OrderBy(p => p.Dayofweek).ThenBy(a => a.TimePoint).ToList();



            foreach (DBSchema schema in schemas)
            {

                DBDevice d = dbContext.devices.Find(schema.DeviceId);
                
                

                DeviceWrapper wrap = new DeviceWrapper
                    (
                    new Schema
                    (
                        new NexaDevice()
                        {
                            DeviceId = d.deviceID,
                            DeviceType = d.DeviceType,
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

        public void DeleteDevice(NexaDevice nexaDevice)
        {
            dbContext.NexaDeviceObject.Remove(nexaDevice);
            int n = dbContext.SaveChanges();
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
                                new NexaDevice()
                                {
                                    DeviceId = d.deviceID,
                                    DeviceType = d.DeviceType,
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

        public void SaveSchemaForDevice(Schema schema)
        {
            try
            {
                DBSchema dBSchema = new DBSchema() { Action = schema.ActionText == "ON" ? 1 : 0, DayOfWeek = schema.WeekDay, DeviceId = schema.Device.DeviceId, TimePoint = schema.TimePoint };

                NexaTimeSchema timeSchema = new NexaTimeSchema()
                {
                    Action=schema.ActionText == "ON" ? 1: 0,
                    DeviceId=schema.Device.DeviceId,
                    Dayofweek=schema.WeekDay,
                    TimePoint=schema.TimePoint,
                    UpdatedAt=DateTime.Now 
                };

                dbContext.NexaTimeSchema.Add(timeSchema);


                int row = dbContext.SaveChanges();

            }
            catch (Exception ep)
            {
                Exception exception = ep;
            }
        }

        public void SaveNexaTimeschema(NexaTimeSchema schema)
        {

        }

        public void SaveNewDevice(NexaDevice device)
        {
            try
            {
                //dbContext.NexaDeviceObject.Add(new NexaDevice() { DeviceName = device.DeviceName, DeviceType = device.DeviceType });
                dbContext.NexaDeviceObject.Add(device);
                int n = dbContext.SaveChanges();

                dbContext.SaveChanges();
            }
            catch (Exception ep)
            {
                Exception exception = ep;
            }
            
        }

        private List<NexaDevice> _dbDevices { get; } = new List<NexaDevice>();
        public List<NexaDevice> GetDbDevices
        {
            get
            {
                return dbContext.NexaDeviceObject.ToList();
            }
        }

        private List<NexaDevice> mDevices;
        public List<NexaDevice> Devices
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

            List<NexaDevice> someDevices = GetDevices();

            int n = 1;
            foreach (NexaDevice d in someDevices)
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

        private List<NexaDevice> GetDevices()
        {
            List<NexaDevice> devices = new List<NexaDevice>();

            NexaDevice device = new NexaDevice() { DeviceName = "Nexa 1", DeviceId = 1 };
            NexaDevice device2 = new NexaDevice() { DeviceName = "Nexa 2", DeviceId = 2};
            NexaDevice device3 = new NexaDevice() { DeviceName = "Nexa 3", DeviceId = 3};
            NexaDevice device4 = new NexaDevice() { DeviceName = "Nexa 4", DeviceId = 4 };

            devices.Add(device);
            devices.Add(device2);
            devices.Add(device3);
            devices.Add(device4);

            return devices;
        }
    }
}
