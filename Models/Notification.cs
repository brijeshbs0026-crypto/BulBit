using System.ComponentModel.DataAnnotations;

namespace AuthMvcApp.Models;

public class Notification
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = "General"; // "Task", "Event", "General"

    [StringLength(150)]
    public string? TargetUserEmail { get; set; } // Null if for all users or role-based

    public int? TargetEmployeeId { get; set; }

    [StringLength(50)]
    public string? TargetRole { get; set; } // "All", "Admin", "Employee"

    [StringLength(255)]
    public string? Url { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
