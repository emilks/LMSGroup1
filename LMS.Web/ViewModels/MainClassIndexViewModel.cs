namespace LMS.Web.ViewModels
{
    public class MainClassIndexViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<ModuleViewModel> Modules { get; set; }

    }
}
