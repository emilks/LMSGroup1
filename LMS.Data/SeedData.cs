using Bogus;
using Bogus.DataSets;
using LMS.Core.Entities;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Activity = LMS.Core.Entities.Activity;
using Module = LMS.Core.Entities.Module;

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

        private static IEnumerable<Course> GetCourses(int nrCourses, int nrModules, int nrActivities)
        {
            var courses = new List<Course>();

            for (var i = 0; i < nrCourses; i++)
            {
                var course = new Course
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(Random.Shared.Next(40) + Random.Shared.Next(40)),
                    EndDate = DateTime.Now.AddDays(40 + Random.Shared.Next(40))
                };

                course.Modules = GetModules(nrModules, nrActivities, course.StartDate, course.EndDate);

                courses.Add(course);
            }

            return courses;
        }

        private static ICollection<Module> GetModules(int nrModules, int nrActivities, DateTime courseStart, DateTime courseEnd)
        {
            var modules = new List<Module>();

            var moduleStartDate = courseStart;

            for (var i = 0; i < nrModules; i++)
            {
                var maxModuleDaysLen = courseEnd.Subtract(moduleStartDate).Days;

                if (maxModuleDaysLen <= 0)
                    break;

                var moduleEndDate = courseStart.AddDays(5 + Random.Shared.Next(maxModuleDaysLen));

                var module = new Module
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = moduleStartDate,
                    EndDate = moduleEndDate
                };

                moduleStartDate = moduleEndDate;

                module.Activities = GetActivites(nrActivities, module.StartDate, module.EndDate);

                modules.Add(module);
            }

            return modules;
        }

        private static ICollection<Activity> GetActivites(int nrActivities, DateTime moduleStart, DateTime moduleEnd)
        {
            var activites = new List<Activity>();

            var activityStartDate = moduleStart;

            for (var i = 0; i < nrActivities; i++)
            {
                var maxActivityDaysLen = moduleEnd.Subtract(activityStartDate).Days;

                if (maxActivityDaysLen <= 0)
                    break;

                var activityEndDate = moduleStart.AddDays(1 + Random.Shared.Next(maxActivityDaysLen));

                var activity = new Activity
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = activityStartDate,
                    EndDate = activityEndDate,
                };

                activityStartDate = activityEndDate;

                activites.Add(activity);
            }

            return activites;
        }
    }
}
