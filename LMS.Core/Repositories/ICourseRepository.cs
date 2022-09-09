using LMS.Core.Entities;

namespace LMS.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<StudentUser>?> GetCourseStudents(int? id);
        Task<IEnumerable<TeacherUser>?> GetCourseTeachers(int? id);
        Task<Course?> GetCourseWithContacts(int? id);
    }
}