public class PriestAvailabilityInput
{
    public int ID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Days { get; set; } // e.g., "Monday, Wednesday"
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; }
}
