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

        public NexaDevice GetDeviceByDeviceId(int deviceId)
        {
            return dbContext.NexaDeviceObject.Find(deviceId);
        }
        
        public NexaDevice GetNexaDevice(int deviceId)
        {
            NexaDevice device = dbContext.NexaDeviceObject.Find(deviceId);
            return device;
        }

        
        public void DeleteDevice(NexaDevice nexaDevice)
        {
            dbContext.NexaDeviceObject.Remove(nexaDevice);
            int n = dbContext.SaveChanges();
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
                try
                {
                    return dbContext.NexaDeviceObject.ToList();
                }
                catch (Exception ep)
                {

                    return null;
                }
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

     
        //private List<DeviceWrapper> GetAllFakeItems()
        //{
        //    List<DeviceWrapper> wrapper = new List<DeviceWrapper>();

        //    List<NexaDevice> someDevices = GetDevices();

        //    int n = 1;
        //    foreach (NexaDevice d in someDevices)
        //    {
        //        Schema schema = null;
        //        if (n % 2 == 0)
        //            schema = new Schema(d) { ActionText = "ON", TimePoint = DateTime.Now.Add(new TimeSpan(1, 1, 1, 1, 1)), WeekDay = 1 };
        //        else
        //            schema = new Schema(d) { ActionText = "OFF", TimePoint = DateTime.Now.Add(new TimeSpan(-1, 1, -1, 1, -1)), WeekDay = 2 };

        //        DeviceWrapper wrp = new DeviceWrapper(schema);

        //        wrapper.Add(wrp);
        //        n++;
        //    }



        //    return wrapper;
        //}

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
