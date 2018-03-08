namespace Nexa.ViewModels
{
    public class WeekDays
    {
        public string WeekDayName { get; set; }
        public int WeekDayId { get; set; }

        
        public override string ToString()
        {
            return WeekDayName;
        }
        
    }
}