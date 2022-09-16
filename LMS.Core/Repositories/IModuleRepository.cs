using LMS.Core.Entities;

namespace LMS.Core.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>?> GetModules(bool includeActivities = false);
        Task<Module?> GetModuleFull(int? id);
        void RemoveModule(Module module);
    }
}
