using Bogus;
using Bogus.DataSets;
using LMS.Core.Entities;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<IdentityUser> userManager = default!;

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            db = context;

            if (db.Course != null)
                if (await db.Course.AnyAsync()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            ArgumentNullException.ThrowIfNull(roleManager);

            userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            ArgumentNullException.ThrowIfNull(userManager);

            faker = new Faker("sv");

            var roleNames = new[] { "Student", "Teacher" };

            await AddRolesAsync(roleNames);

            var teachers = await GetTeacherUsersAsync(5);
            await AddToRolesAsync(teachers, new[] { "Teacher" });

            var students = await GetStudentUsersAsync(5);
            await AddToRolesAsync(students, new[] { "Student" });

            var activityTypes = GetActivityTypes();
            await db.AddRangeAsync(activityTypes);
            var courses = GetCourses(5, 3, 6, activityTypes);
            await db.AddRangeAsync(courses);

            // TODO: Lägg till seeddata för dokument

            await db.SaveChangesAsync();
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                    continue;

                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<IEnumerable<IdentityUser>> GetTeacherUsersAsync(int nrOfUsers)
        {
            var users = new List<IdentityUser>();

            var password = "a";

            var testTeacher = new TeacherUser()
            {
                UserName = "teacher@lms.se",
                Email = "teacher@lms.se",
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName()
            };

            var result = await userManager.CreateAsync(testTeacher, password);

            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));

            for (var i = 0; i < nrOfUsers; i++)
            {
                var email = faker.Internet.Email();
                var found = await userManager.FindByEmailAsync(email);

                if (found != null) return null!;

                var teacher = new TeacherUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName()
                    //TimeOfRegistration = DateTime.Now
                };

                result = await userManager.CreateAsync(teacher, password);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }

            return users;
        }

        private static async Task<IEnumerable<IdentityUser>> GetStudentUsersAsync(int nrOfUsers)
        {
            var users = new List<IdentityUser>();

            var password = "a";

            var testStudent = new StudentUser
            {
                UserName = "student@lms.se",
                Email = "student@lms.se",
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName()
                //TimeOfRegistration = DateTime.Now
            };

            var result = await userManager.CreateAsync(testStudent, password);

            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));

            for (var i = 0; i < nrOfUsers; i++)
            {
                var email = faker.Internet.Email();
                var found = await userManager.FindByEmailAsync(email);

                if (found != null) return null!;

                var teacher = new StudentUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName()
                    //TimeOfRegistration = DateTime.Now
                };

                result = await userManager.CreateAsync(teacher, password);

                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors));
            }

            return users;
        }

        private static async Task AddToRolesAsync(IEnumerable<IdentityUser> users, string[] roleNames)
        {
            foreach (var user in users)
            {
                foreach (var role in roleNames)
                {
                    if (await userManager.IsInRoleAsync(user, role))
                        continue;

                    var result = await userManager.AddToRoleAsync(user, role);

                    if (!result.Succeeded)
                        throw new Exception(string.Join("\n", result.Errors));
                }
            }
        }

        //private static async Task<TeacherUser> AddTeacherAsync(string teacherEmail, string teacherPW)
        //{
        //    var found = await userManager.FindByEmailAsync(teacherEmail);

        //    if (found != null) return null!;

        //    var teacher = new TeacherUser
        //    {
        //        UserName = teacherEmail,
        //        Email = teacherEmail,
        //        FirstName = "Admin",
        //        //TimeOfRegistration = DateTime.Now
        //    };

        //    var result = await userManager.CreateAsync(teacher, teacherPW);
        //    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

        //    return teacher;
        //}

        //private static async Task AddToRolesAsync(IdentityUser user, string[] roleNames)
        //{
        //    foreach (var role in roleNames)
        //    {
        //        if (await userManager.IsInRoleAsync(user, role)) continue;
        //        var result = await userManager.AddToRoleAsync(user, role);
        //        if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        //    }
        //}


        private static IEnumerable<ActivityType> GetActivityTypes()
        {
            var activityTypes = new List<ActivityType>();

            activityTypes.Add(new ActivityType() { ActivityName = "E-Learning" });
            activityTypes.Add(new ActivityType() { ActivityName = "Föreläsning" });
            activityTypes.Add(new ActivityType() { ActivityName = "Övning" });
            activityTypes.Add(new ActivityType() { ActivityName = "Inlämning" });

            return activityTypes;
        }

        private static IEnumerable<Course> GetCourses(int nrCourses, int nrModules, int nrActivities, IEnumerable<ActivityType> activityTypes)
        {
            var courses = new List<Course>();

            for (var i = 0; i < nrCourses; i++)
            {
                var course = new Course
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(
                        Random.Shared.Next(40) + Random.Shared.Next(40)),
                    EndDate = DateTime.Now.AddDays(40 + Random.Shared.Next(40))
                };

                course.Modules = GetModules(nrModules, nrActivities,
                    course.StartDate, course.EndDate, activityTypes);

                courses.Add(course);
            }

            return courses;
        }

        private static ICollection<Module> GetModules(int nrModules, int nrActivities, DateTime courseStart, DateTime courseEnd,
            IEnumerable<ActivityType> activityTypes)
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

                module.Activities = GetActivites(nrActivities, module.StartDate, module.EndDate, activityTypes);

                modules.Add(module);
            }

            return modules;
        }

        private static ICollection<Activity> GetActivites(int nrActivities, DateTime moduleStart, DateTime moduleEnd,
            IEnumerable<ActivityType> activityTypes)
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
                    ActivityType = activityTypes.ElementAt(Random.Shared.Next(activityTypes.Count()))
                };

                activityStartDate = activityEndDate;

                activites.Add(activity);
            }

            return activites;
        }
    }
}
