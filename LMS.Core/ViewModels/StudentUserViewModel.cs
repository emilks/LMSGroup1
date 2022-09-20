using LMS.Core.Entities;

namespace LMS.Core.ViewModels
{
    public class StudentUserViewModel
    {
        public int StudentUser_CourseId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

    }
}