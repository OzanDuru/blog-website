
using System.ComponentModel.DataAnnotations;

namespace WebSitem.Models
{
    public class FileResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? StoredFileName { get; set; }
        public string OriginalFileName { get; set; }

        [Url]
        public string? ExternalLink { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public string UploadedByUserId { get; set; }


    }
}