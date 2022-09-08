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
        public DbSet<Activity>? Activity { get; set; }
        public DbSet<ActivityType>? ActivityType { get; set; }
        public DbSet<Document>? Document { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityType>().HasData(
                new ActivityType { Id = 1, ActivityName = "Föreläsning"}
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Kurs 1", Description = "En kurs", StartDate = DateTime.Now, EndDate = DateTime.Now },
                new Course { Id = 2, Name = "Kurs 2", Description = "En annan kurs", StartDate = DateTime.Now, EndDate = DateTime.Now }
            );

            modelBuilder.Entity<Module>().HasData(
                new Module { Id = 1, Name = "Modul 1", Description = "Första modulen", 
                    StartDate = DateTime.Now, EndDate = DateTime.Now, CourseId = 1 
                },
                new Module
                {
                    Id = 2,
                    Name = "Modul 2",
                    Description = "Andra modulen",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    CourseId = 1
                },
                new Module
                {
                    Id = 3,
                    Name = "Modul 3",
                    Description = "Tredje modulen",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    CourseId = 2
                }
            );

            modelBuilder.Entity<Activity>().HasData(
                new Activity { Id = 1, Name = "Aktivitet 1", Description = "Första aktiviteten i första modulen", 
                    StartDate = DateTime.Now, EndDate = DateTime.Now, ActivityTypeId = 1, ModuleId = 1
                },
                new Activity
                {
                    Id = 2,
                    Name = "Aktivitet 2",
                    Description = "Andra aktiviteten i först modulen",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    ActivityTypeId = 1,
                    ModuleId = 1
                }, 
                new Activity
                {
                    Id = 3,
                    Name = "Aktivitet 3",
                    Description = "Första aktiviteten i andra modulen",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    ActivityTypeId = 1,
                    ModuleId = 2
                },
                new Activity
                {
                    Id = 4,
                    Name = "Aktivitet 4",
                    Description = "Andra aktiviteten i andra modulen",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    ActivityTypeId = 1,
                    ModuleId = 2
                }
            );



            //modelBuilder.Entity<Member>().HasData(
            //    new Member { Id = 1, FirstName = "John", LastName = "Doe", PerNr = "123456" },
            //    new Member { Id = 2, FirstName = "Jane", LastName = "Doe", PerNr = "123" }
            //);
            //modelBuilder.Entity<VehicleTypeEntity>().HasData(
            //    new VehicleTypeEntity { Id = 1, Category = "Car", Size = 1 }
            //);

            //modelBuilder.Entity<Vehicle>().HasData(
            //    new Vehicle { Id = 1, RegNr = "AAA111", Color = "Red", Brand = "Volvo", Model = "V20", NrOfWheels = 4, MemberId = 1, VehicleTypeEntityId = 1 },
            //    new Vehicle { Id = 2, RegNr = "BBB222", Color = "Black", Brand = "Mercedes", Model = "X100", NrOfWheels = 4, MemberId = 1, VehicleTypeEntityId = 1 },
            //    new Vehicle { Id = 3, RegNr = "CCC333", Color = "White", Brand = "Ferrari", Model = "E-Type", NrOfWheels = 4, MemberId = 1, VehicleTypeEntityId = 1 },
            //    new Vehicle { Id = 4, RegNr = "DDD444", Color = "Blue", Brand = "Volvo", Model = "V20", NrOfWheels = 4, MemberId = 2, VehicleTypeEntityId = 1 }
            //);

            //modelBuilder.Entity<ParkingSpace>().HasData(
            //    new ParkingSpace { Id = 1, NumberSpot = "A1" },
            //    new ParkingSpace { Id = 2, NumberSpot = "A2" },
            //    new ParkingSpace { Id = 3, NumberSpot = "A3" },
            //    new ParkingSpace { Id = 4, NumberSpot = "A4" }
            //);
        }
    }
}