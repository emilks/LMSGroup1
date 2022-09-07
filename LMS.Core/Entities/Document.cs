using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string FilePath { get; set; }

        public Course? Course { get; set; }
        public Module? Module { get; set; }
        public Activity? Activity { get; set; }
        public IdentityUser Owner { get; set; }
    }
}
