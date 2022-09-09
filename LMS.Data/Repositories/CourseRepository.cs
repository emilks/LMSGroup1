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

        public async Task<IEnumerable<Course>?> GetCourses(bool includeModules = false) {
            if(db.Course == null) {
                return null;
            }
            if (includeModules) {
                return await db.Course.Include(c => c.Modules).ToListAsync();
            }
            return await db.Course.ToListAsync();
        }

        public async Task<Course?> GetCourseWithContacts(int? id) {
            if (db.Course == null || id == null) {
                return null;
            }

            return await db.Course.Include(c => c.Students)
                                  .Include(c => c.Teachers)
                                  .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetCourseFull(int? id) {
            if(db.Course == null || id == null) {
                return null;
            }

            return await db.Course.Include(c => c.Students)
                                  .Include(c => c.Teachers)
                                  .Include(c => c.Documents)
                                  .Include(c => c.Modules)//.Select(m => m.Documents))
                                  .ThenInclude(m => m.Documents)
                                  //.ThenInclude(m => m.Activities)
                                  .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
