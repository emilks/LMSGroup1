using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext db;

        public ActivityRepository(ApplicationDbContext context) {
            this.db = context;
        }

        public async Task<Activity?> GetActivity(int? id, bool includeModuleAndDocuments) {
            if (id == null || db.Activity == null) {
                return null;
            }

            if (includeModuleAndDocuments) {
                return db.Activity.Include(a => a.Module)
                                  .Include(a => a.Documents)
                                  .FirstOrDefault(a => a.Id == id);
            }
            return db.Activity.FirstOrDefault(a => a.Id == id);
        }

        public async Task AddDocument(Activity activity, Document document) {
            if (db.Activity == null) {
                return;
            }

            var target = await db.Activity.Where(a => a.Id == activity.Id)
                                          .Include(a => a.Documents)
                                          .FirstOrDefaultAsync();

            if (target == null) {
                return;
            }

            target.Documents.Add(document);
        }

    }
}
