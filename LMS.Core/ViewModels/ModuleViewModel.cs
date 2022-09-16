#nullable disable
using LMS;
using LMS.Core.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.ViewModels
{
    public class ModuleViewModel
    {
        [DisplayName("Modulnamn")]
        public string Name { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
        [DisplayName("Startdatum")]
        [Remote(action: "VerifyStartDate", controller: "Modules")]
        [DataType(DataType.Date)]
        // Avvaktivera backend-validering tills svidare
        //[ValidateModuleStartDate]
        public DateTime StartDate { get; set; }
        // Avvaktivera backend-validering tills svidare
        [DisplayName("Slutdatum")]
        //[ValidateModuleEndDate]
        [DataType(DataType.Date)]
        [Remote(action: "VerifyEndDate", controller: "Modules", AdditionalFields = "StartDate,CourseId")]
        public DateTime EndDate { get; set; }

        [DisplayName("Längd (antal dagar)")]
        [Remote(action: "VerifyDuration", controller:"Modules", AdditionalFields = "StartDate,CourseId")]
        public int Duration
        {
            get
            {
                return (EndDate - StartDate).Days;
            }
            set
            {
                EndDate = StartDate + TimeSpan.FromDays(value);
            }
        }

        public IEnumerable<ActivityViewModel> Activities { get; set; }
    }
}