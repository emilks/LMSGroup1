using LMS.Core.Repositories;
using LMS.Data.Data;

namespace LMS.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public ICourseRepository CourseRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public IActivityRepository ActivityRepository { get; }

        public UnitOfWork(ApplicationDbContext context) {
            db = context;
            CourseRepository = new CourseRepository(context);
            ModuleRepository = new ModuleRepository(context);
            ActivityRepository = new ActivityRepository(context);
        }

        public async Task CompleteAsync() {
            await db.SaveChangesAsync();
        }
    }
}
