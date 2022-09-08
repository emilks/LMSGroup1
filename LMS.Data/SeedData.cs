using Bogus;
using Bogus.DataSets;
using LMS.Core.Entities;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Activity = LMS.Core.Entities.Activity;

namespace LMS.Data
{
    public class SeedData
    {
        private static ApplicationDbContext? db;
        private static Faker? faker;

        public static async Task InitAsync(ApplicationDbContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            
            db = context;

            if (db.Course != null)
                if (await db.Course.AnyAsync()) return;

            faker = new Faker("sv");

            var courses = GetCourses(5, 3, 6);

            await db.AddRangeAsync(courses);

            await db.SaveChangesAsync();
        }

        private static IEnumerable<Course> GetCourses(int nrCourses, int nrModulesPerCourse, int nrActivitiesPerModule)
        {
            var courses = new List<Course>();

            for (var i = 0; i < nrCourses; i++)
            {
                var course = new Course
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(faker!.Random.Int(-5, -5)),
                    EndDate = DateTime.Now.AddDays(6 + faker!.Random.Int(-5, -5)),
                    Modules = GetModules(nrModulesPerCourse, nrActivitiesPerModule)
                };

                courses.Add(course);
            }

            return courses;
        }

        private static ICollection<Module> GetModules(int nrModulesPerCourse, int nrActivitiesPerModule)
        {
            var modules = new List<Module>();

            for (var i = 0; i < nrModulesPerCourse; i++)
            {
                modules.Add(new Module()
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(faker!.Random.Int(-5, -5)),
                    EndDate = DateTime.Now.AddDays(6 + faker!.Random.Int(-5, -5)),
                    Activities = GetActivites(nrActivitiesPerModule)
                });
            }

            return modules;
        }

        private static ICollection<Activity> GetActivites(int nrActivitiesPerModule)
        {
            var activites = new List<Activity>();

            for (var i = 0; i < nrActivitiesPerModule; i++)
            {
                activites.Add(new Activity()
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(faker!.Random.Int(-5, -5)),
                    EndDate = DateTime.Now.AddDays(6 + faker!.Random.Int(-5, -5)),
                });
            }

            return activites;
        }
    }
}
