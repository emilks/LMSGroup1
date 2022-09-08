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

namespace LMS.Data
{
    public class SeedData
    {
        private static ApplicationDbContext db;
        private static Faker? faker;

        public static async Task InitAsync(ApplicationDbContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            db = context;

            if (db.Course != null)
                if (await db.Course.AnyAsync()) return;

            faker = new Faker("sv");

            var courses = GetCoursesAsync(5);
            await db.AddRangeAsync(courses);

            await db.SaveChangesAsync();
        }

        private static IEnumerable<Course> GetCoursesAsync(int amount)
        {
            var courses = new List<Course>();

            for (var i = 0; i < amount; i++)
            {
                var course = new Course
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(faker!.Random.Int(-5, -5)),
                    EndDate = DateTime.Now.AddDays(6 + faker!.Random.Int(-5, -5))
                };

                courses.Add(course);
            }

            return courses;
        }
    }
}
