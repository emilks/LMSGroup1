using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LMS.Core.Entities
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Start date")]
        [Remote(action: "VerifyStartDate", controller: "Activities", AdditionalFields = "ModuleId")]
        public DateTime StartDate { get; set; }

        [DisplayName("End date")]
        [Remote(action: "VerifyEndDate", controller: "Activities", AdditionalFields = "ModuleId")]
        public DateTime EndDate { get; set; }

        // Foreign keys
        public int ModuleId { get; set; }

        // Navigation props
        public Module Module { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ActivityType ActivityType { get; set; }
    }
}
