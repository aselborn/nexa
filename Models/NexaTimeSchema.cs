using Nexa.library;
using Nexa.ViewModels;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexa.Models
{
    [Table("NexaTimeSchema")]
    public class NexaTimeSchema : ViewModelBase
    {
        private int _id;
        [Key]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        private int _deviceId;
        
        public int DeviceId
        {
            get => _deviceId;
            set
            {
                _deviceId = value;
                NotifyPropertyChanged(nameof(DeviceId));
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

        private int _action;
        public int Action
        {
            get => _action;
            set
            {
                _action = value;
                NotifyPropertyChanged(nameof(Action));
            }
        }

        private DateTime _updatedAt;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                NotifyPropertyChanged(nameof(UpdatedAt));
                
            }
        }

        private int _dayOfWeek;
        public int Dayofweek
        {
            get => _dayOfWeek;
            set
            {
                _dayOfWeek = value;
                NotifyPropertyChanged(nameof(DayOfWeek));
            }
        }

        public string WeekDayAsText => Enum.GetName(typeof(EnumDayOfWeek), (Dayofweek - 1));
        public string ActionText => Action == 1 ? "PÅ" : "AV";
        public string TimePointAsString => $"{TimePoint:hh:mm}";
    }
}