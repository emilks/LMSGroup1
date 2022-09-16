using LMS.Core.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    internal class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;

        public ModuleRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Module>?> GetModules(bool includeActivities = false)
        {
            throw new NotImplementedException();
            if (db.Module == null)
            {
                return null;
            }
            if (includeActivities)
            {
                return await db.Module.Include(c => c.Activities.OrderBy(m => m.StartDate)).OrderBy(c => c.StartDate).ToListAsync();
            }
            return await db.Module.OrderBy(c => c.StartDate).ToListAsync();
        }

        public async Task<Module?> GetModuleFull(int? id)
        {
            if (db.Module == null || id == null)
            {
                return null;
            }

            return await db.Module.Include(m => m.Documents)
                                  .Include(m => m.Activities.OrderBy(a => a.StartDate))
                                  .ThenInclude(a => a.Documents)
                                  .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void RemoveModule(Module module)
        {
            db.Module.Remove(module);
        }
    }
}
