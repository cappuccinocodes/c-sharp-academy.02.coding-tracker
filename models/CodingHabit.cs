namespace Coding_Tracker.models
{
    internal class CodingHabit
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TimeSpan TimeSpan
        {
            get
            {
                return EndDate - StartDate;
            }
        }
    }
}