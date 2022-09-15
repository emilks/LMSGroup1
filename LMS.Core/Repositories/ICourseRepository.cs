﻿using LMS.Core.Entities;

namespace LMS.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>?> GetCourses(bool includeModules = false);
        Task<Course?> GetCourseWithContacts(int? id);
        Task<Course?> GetCourseFull(int? id);
        Task<IEnumerable<TeacherUser?>> GetTeacherContacts();
    }
}