#nullable disable
using LMS;
using LMS.Core.Validations;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Core.ViewModels
{
    public class ModuleViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Remote(action: "VerifyStartDate", controller: "Modules")]
        // Avvaktivera backend-validering tillfälligt
        //[ValidateModuleStartDate]
        public DateTime StartDate { get; set; }
        // Avvaktivera backend-validering tillfälligt
        //[ValidateModuleEndDate]
        [Remote(action: "VerifyEndDate", controller: "Modules", AdditionalFields = "StartDate,CourseId")]
        public DateTime EndDate { get; set; }
        public IEnumerable<ActivityViewModel> Activities { get; set; }
    }
}