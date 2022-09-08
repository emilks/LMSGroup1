namespace LMS.Core.Repositories
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }

        Task CompleteAsync();
    }
}