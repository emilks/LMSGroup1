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
    public class Activity
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
        [Remote(action: "VerifyStartDate", controller: "Activities", AdditionalFields = "ModuleId")]
        [ValidateActivityStartDate]
        public DateTime StartDate { get; set; }

        [DisplayName("Slut datum")]
        [Remote(action: "VerifyEndDate", controller: "Activities", AdditionalFields = "StartDate,ModuleId")]
        [ValidateActivityEndDate]
        public DateTime EndDate { get; set; }

        // Foreign keys
        public int ModuleId { get; set; }

        // Navigation props
        public Module Module { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ActivityType ActivityType { get; set; }
    }
}
