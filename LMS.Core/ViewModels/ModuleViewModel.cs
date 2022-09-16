#nullable disable
using LMS;
using LMS.Core.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.ViewModels
{
    public class ModuleViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Remote(action: "VerifyStartDate", controller: "Modules")]
        [DataType(DataType.Date)]
        // Avvaktivera backend-validering tills svidare
        //[ValidateModuleStartDate]
        public DateTime StartDate { get; set; }
        // Avvaktivera backend-validering tills svidare
        //[ValidateModuleEndDate]
        [DataType(DataType.Date)]
        [Remote(action: "VerifyEndDate", controller: "Modules", AdditionalFields = "StartDate,CourseId")]
        public DateTime EndDate { get; set; }
        public IEnumerable<ActivityViewModel> Activities { get; set; }
    }
}