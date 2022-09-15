using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LMS.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
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
