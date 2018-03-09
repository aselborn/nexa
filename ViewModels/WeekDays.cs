namespace Nexa.ViewModels
{
    public class WeekDays : ViewModelBase
    {
        private string _weekDayName;
        public string WeekDayName
        {
            get => _weekDayName;
            set
            {
                _weekDayName = value;
                NotifyPropertyChanged(nameof(WeekDayName));
            }
        }

        private int _weekDayId;
        public int WeekDayId
        {
            get => _weekDayId;
            set
            {
                _weekDayId = value;
                NotifyPropertyChanged(nameof(WeekDayId));
            }
        }

        
        public override string ToString()
        {
            return WeekDayName;
        }
        
    }
}