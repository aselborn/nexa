﻿using Nexa.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nexa.Models 
{
    
    [Table("NexaDevice")]
    public class NexaDevice : ViewModelBase
    {
       
        private int _deviceId;
        [XmlElement]
        [Key]
        public int DeviceId
        {
            get => _deviceId;
            set
            {
                _deviceId = value;
                NotifyPropertyChanged(nameof(DeviceId));
            }
        }

        private string _deviceName;
        public string DeviceName
        {
            get => _deviceName;
            set
            {
                _deviceName = value;
                NotifyPropertyChanged(nameof(DeviceName));
            }
        }

        [XmlElement]
        private string _deviceType;
        public string DeviceType
        {
            get => _deviceType;
            set
            {
                _deviceType = value;
                NotifyPropertyChanged(nameof(DeviceType));
            }
        }

        [XmlElement]
        private int? _nexaId;
        public int? NexaId
        {
            get => _nexaId;
            set
            {
                _nexaId = value;
                NotifyPropertyChanged(nameof(NexaId));
            }
        }
 
        
        public virtual ICollection<NexaTimeSchema> timeschemas { get; set; }
        
    }
}
