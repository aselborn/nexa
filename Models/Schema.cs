using Nexa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    public class Schema : ViewModelBase
    {
        private readonly Device myDevice;

        

        public Schema(Device device)
        {
            myDevice = device;
        }

        public Device Device { get => myDevice;  }

        private int _weekDay;
        public int WeekDay
        {
            get => _weekDay;
            set
            {
                _weekDay = value;
                NotifyPropertyChanged(nameof(WeekDay));
            }
        }

        private string _actionText;
        public string ActionText
        {
            get => _actionText;
            set
            {
                _actionText = value;
                NotifyPropertyChanged(nameof(ActionText));
            }
        }

        private DateTime _timePoint;
        public DateTime TimePoint
        {
            get => _timePoint;
            set
            {
                _timePoint = value;
                NotifyPropertyChanged(nameof(TimePoint));
            }
        }

    }
}
