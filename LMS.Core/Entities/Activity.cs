using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LMS.Core.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation props
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();

        public int ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
