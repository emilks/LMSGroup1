namespace LMS.Web.ViewModels
{
    public class MainClassIndexViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<string> moduleNames { get; set; }
        public IEnumerable<(string Name, string Description)> Modules { get; set; }

        /*public List<string> moduleNames { get; set; }
        public List<string> moduleDescriptions { get; set; }
        public List<DateTime> ModuleStartDates { get; set; }
        public List<DateTime> ModuleEndDates { get; set; }*/

    }
}
