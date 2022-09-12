#nullable disable
using LMS;

namespace LMS.Core.ViewModels
{
    public class ModuleViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}