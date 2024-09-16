namespace MinimalApiProject.Models
{
    public class PriestAvailability {
    public int ID { get; set; }
    public int UserID { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
}
}