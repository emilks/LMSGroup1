using LMS.Core.Validations;
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
    public class Module
    {
        public int Id { get; set; }
        [DisplayName("Namn")]
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Beskrivning")]
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Start datum")]
        [Remote(action: "VerifyStartDate", controller: "Modules", AdditionalFields ="CourseId")]
        [ValidateModuleStartDate]
        public DateTime StartDate { get; set; }

        [DisplayName("Slut datum")]
        [ValidateModuleEndDate]
        [Remote(action: "VerifyEndDate", controller: "Modules", AdditionalFields = "StartDate,CourseId")]
        public DateTime EndDate { get; set; }

        // Foreign keys
        public int CourseId { get; set; }

        // Navigation props
        public ICollection<Activities> Activities { get; set; } = new List<Activities>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

        public Course Course { get; set; }
    }
}
