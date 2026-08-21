namespace AuthMvcApp.Models;

public class CalendarEvent
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public string? EventType { get; set; }

    public DateTime CreatedAt { get; set; }
}