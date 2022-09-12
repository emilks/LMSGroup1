using LMS.Core.Entities;

namespace LMS.Core.ViewModels
{
    public class CourseContactsViewModel
    {
        public int Id;
        public string Name { get; set; } = string.Empty;
        public ICollection<StudentUser> Students { get; set; } = new List<StudentUser>();
        public ICollection<TeacherUser> Teachers { get; set; } = new List<TeacherUser>();
    }
}
