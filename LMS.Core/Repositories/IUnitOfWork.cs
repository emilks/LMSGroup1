namespace LMS.Core.Repositories
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IActivityRepository ActivityRepository { get; }

        Task CompleteAsync();
    }
}