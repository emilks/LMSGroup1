using LMS.Core.Entities;

namespace LMS.Core.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity?> GetActivity(int? id, bool includeModuleAndDocuments);
        void AddDocument(Activity activity, Document document);
    }
}