namespace AuthMvcApp.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string? Description { get; set; }

    public int EmployeeId { get; set; }

    public string Priority { get; set; } = "Medium";

    public string Status { get; set; } = "Pending";

    public int Progress { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public Employee? Employee { get; set; }
}