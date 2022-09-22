using Microsoft.AspNetCore.Identity;

namespace LMS.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string FilePath { get; set; } = string.Empty;

        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public int? ModuleId { get; set; }
        public Module? Module { get; set; }

        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }

        public string? IdentityUserId { get; set; }
        public IdentityUser? Owner { get; set; }

        public Document() {
            Timestamp = DateTime.Now;
        }
    }
}
