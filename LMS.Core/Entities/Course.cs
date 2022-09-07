namespace LMS.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation props
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<StudentUser> Students { get; set; } = new List<StudentUser>();
        public ICollection<TeacherUser> Teachers { get; set; } = new List<TeacherUser>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}