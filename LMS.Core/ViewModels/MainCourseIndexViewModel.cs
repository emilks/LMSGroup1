#nullable disable
using LMS;

namespace LMS.Core.ViewModels
{
    public class MainCourseIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<ModuleViewModel> Modules { get; set; }

    }
}
