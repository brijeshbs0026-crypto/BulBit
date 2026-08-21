
using System.ComponentModel.DataAnnotations;

namespace AuthMvcApp.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string ClientName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public int ReelsPerMonth { get; set; }

        public int PostsPerMonth { get; set; }

        public int StoriesPerMonth { get; set; }

        public int VideosPerMonth { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

