using System.ComponentModel.DataAnnotations;

namespace AuthMvcApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [StringLength(30)]
        public string Status { get; set; } = "Active";

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    }
}