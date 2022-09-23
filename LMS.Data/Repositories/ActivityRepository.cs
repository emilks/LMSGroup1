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

        public async Task<Activity?> GetActivity(int? id, bool includeModule) {
            if (id == null || db.Activity == null) {
                return null;
            }

            if (includeModule) {
                return db.Activity.Include(a => a.Module).FirstOrDefault(a => a.Id == id);
            }
            return db.Activity.FirstOrDefault(a => a.Id == id);
        }
    }
}
