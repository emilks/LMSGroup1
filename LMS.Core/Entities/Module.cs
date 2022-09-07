using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation props
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Course Course { get; set; }
    }
}
