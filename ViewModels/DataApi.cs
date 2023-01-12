using Nexa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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



        
        public void SaveNexaTimeschema(NexaTimeSchema schema)
        {
            dbContext.NexaTimeSchema.Add(schema);
            dbContext.SaveChanges();
        }

        public NexaTimeSchema UpdateNexaTimeschema(NexaTimeSchema schema)
        {
            NexaTimeSchema item = dbContext.NexaTimeSchema.SingleOrDefault(p => p.Id == schema.Id);
            if (item != null)
            {
                item.TimePoint = schema.TimePoint;
                item.UpdatedAt = DateTime.Now;
                item.Dayofweek = schema.Dayofweek;
                item.Action = schema.Action;

                dbContext.SaveChanges();

            }

            return item;
        }

        public void WriteConfig()
        {
            List<NexaDevice> devices = dbContext.NexaDeviceObject.ToList();

            List<DeviceObject> deviceObject = new List<DeviceObject>();
            foreach (NexaDevice device in devices)
            {
                deviceObject.Add(new DeviceObject() { Comment = device.DeviceType, DeviceId = device.DeviceId, DeviceName = device.DeviceName });
            }

            NexaDevice aDevice = devices[0];

            List<NexaDevice> dd = devices.ToList();

            XmlSerializer ss = new XmlSerializer(dd.GetType());

            ss.Serialize(Console.Out, ss);

            List<NexaTimeSchema> timeSchemas = aDevice.timeschemas.ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(timeSchemas.GetType());

            xmlSerializer.Serialize(Console.Out, timeSchemas);

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = UTF8Encoding.Default;
            writerSettings.WriteEndDocumentOnClose = true;
            writerSettings.Indent = true;

            MemoryStream ms = new MemoryStream();

            DataContractSerializer serializer = new DataContractSerializer(typeof(DeviceObject));
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateDictionaryWriter(XmlWriter.Create(ms));

            serializer.WriteObject(writer, deviceObject, new MyCustomResolver());

            writer.Flush();
            ms.Position = 0;


            //using(XmlWriter writer = XmlWriter.Create("Result.xml", writerSettings))
            //{
            //    DataContractSerializer dataContractSerializer = new DataContractSerializer(deviceObject.GetType());
            //    dataContractSerializer.WriteObject(writer, aDevice);
            //}


        }

        public bool DeleteTimeSchema(NexaTimeSchema item)
        {
            dbContext.NexaTimeSchema.Remove(item);
            return dbContext.SaveChanges() == 1 ? true : false; ;
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
                    return dbContext.NexaDeviceObject.OrderBy(p => p.DeviceName).ToList();
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



        private List<NexaDevice> GetDevices()
        {
            List<NexaDevice> devices = new List<NexaDevice>();

            NexaDevice device = new NexaDevice() { DeviceName = "Nexa 1", DeviceId = 1 };
            NexaDevice device2 = new NexaDevice() { DeviceName = "Nexa 2", DeviceId = 2 };
            NexaDevice device3 = new NexaDevice() { DeviceName = "Nexa 3", DeviceId = 3 };
            NexaDevice device4 = new NexaDevice() { DeviceName = "Nexa 4", DeviceId = 4 };

            devices.Add(device);
            devices.Add(device2);
            devices.Add(device3);
            devices.Add(device4);

            return devices;
        }

        internal void UpdateDevice(NexaDevice nexaDevice)
        {

            var record = dbContext.NexaDeviceObject.SingleOrDefault(p => p.DeviceId == nexaDevice.DeviceId);
            if (record != null)
            {
                record.NexaId = nexaDevice.NexaId;
                record.DeviceName = nexaDevice.DeviceName;
                record.DeviceType = nexaDevice.DeviceType;

                dbContext.SaveChanges();
            }

        }
    }

    public class MyCustomResolver : DataContractResolver
    {
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            return typeof(DeviceObject);
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (type == typeof(DeviceObject))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("Devices");
                typeNamespace = dictionary.Add("http://tempuri.com");
                return true;
            }
            else
            {
                return knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
            }
        }
    }
}
