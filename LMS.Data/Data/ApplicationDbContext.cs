using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;

namespace LMS.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Course>? Course { get; set; }
        public DbSet<Module>? Module { get; set; }
        public DbSet<Activities>? Activity { get; set; }
        public DbSet<ActivityType>? ActivityType { get; set; }
        public DbSet<Document>? Document { get; set; }
        public DbSet<StudentUser>? StudentUser { get; set; }
        public DbSet<TeacherUser>? TeacherUser { get; set; }
    }
}