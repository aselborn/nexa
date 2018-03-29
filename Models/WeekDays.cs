namespace Nexa.ViewModels
{
    public class WeekDays : ViewModelBase
    {
        private string _nameOfWeekDay;
        public string NameOfWeekDay
        {
            get => _nameOfWeekDay;
            set
            {
                _nameOfWeekDay = value;
                NotifyPropertyChanged(nameof(NameOfWeekDay));
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
            return NameOfWeekDay;
        }
        
    }
}