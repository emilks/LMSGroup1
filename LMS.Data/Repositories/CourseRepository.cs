using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext context) {
            this.db = context;
        }

        /*
         * find course by id, maybe a course name is better?
         */
        public async Task<IEnumerable<TeacherUser>?> GetCourseTeachers(int? id) {
            if (db.Course == null || id == null) {
                return null;
            }

            var course = await db.Course.Include(c => c.Teachers).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) {
                return null;
            }

            return course.Teachers;
        }

        public async Task<IEnumerable<StudentUser>?> GetCourseStudents(int? id) {
            if (db.Course == null || id == null) {
                return null;
            }

            var course = await db.Course.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) {
                return null;
            }

            return course.Students;
        }

        public async Task<Course?> GetCourseWithContacts(int? id) {
            if (db.Course == null || id == null) {
                return null;
            }

            return await db.Course.Include(c => c.Students)
                                  .Include(c => c.Teachers)
                                  .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
