using Nexa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    public class Device : ViewModelBase
    {
        private string _deviceType;

        public string DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; }
        }


        private int _deviceId;
        public int DeviceId {
            get => _deviceId;
            set
            {
                _deviceId = value;
                NotifyPropertyChanged(nameof(DeviceId));
            }
        }
        private string _name;
        public string DeviceName {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(DeviceName));
            }
        }
        private string _comment;
        public string DeviceComment {
            get => _comment;
            set
            {
                _comment = value;
                NotifyPropertyChanged(nameof(DeviceComment));
            }
        }

        //private string _description;
        //public string DeviceDescription
        //{
        //    get => _description;
        //    set
        //    {
        //        _description = value;
        //        NotifyPropertyChanged(nameof(DeviceDescription));
        //    }
        //}
    }
}
