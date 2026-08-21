using System.ComponentModel.DataAnnotations;

namespace AuthMvcApp.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, 999999999)]
    public decimal Price { get; set; }
}
