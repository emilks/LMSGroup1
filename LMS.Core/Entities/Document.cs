using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

#nullable disable

namespace LMS.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        [DisplayName("Namn")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Beskrivning")]
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string FilePath { get; set; } = string.Empty;

        public Course? Course { get; set; }
        public Module? Module { get; set; }
        public Activity? Activity { get; set; }
        public IdentityUser Owner { get; set; }

        public Document() {
            Timestamp = DateTime.Now;
        }
    }
}
