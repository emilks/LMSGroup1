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
        private readonly static string testPassword = "a";
        private static Random random = new();

        public static async Task InitAsync(IServiceProvider services)
        {
            db = services.GetRequiredService<ApplicationDbContext>();

            if (db is null)
                throw new ArgumentNullException(nameof(db));

            if (await DbMissingTablesOrHasData())
                return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            ArgumentNullException.ThrowIfNull(roleManager);

            userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            ArgumentNullException.ThrowIfNull(userManager);

            faker = new Faker("sv");

            var roleNames = new[] { "Student", "Teacher" };

            await AddRolesAsync(roleNames);

            var teachers = await GetTeacherUsersAsync(10);
            await AddToRolesAsync(teachers, new[] { "Teacher" });

            var students = await GetStudentUsersAsync(80);
            await AddToRolesAsync(students, new[] { "Student" });

            var activityTypes = GetActivityTypes();
            await db.AddRangeAsync(activityTypes);

            var courses = GetCourses(
                nrCourses: 5,
                nrModulesPerCourse: 3,
                nrActivitiesPerModule: 6,
                activityTypes, teachers, students
                );

            await db.AddRangeAsync(courses);

            await db.SaveChangesAsync();
        }

        private async static Task<bool> DbMissingTablesOrHasData()
        {
            ArgumentNullException.ThrowIfNull(db);

            if (db.Course != null && await db.Course.AnyAsync())
                return true;

            if (db.Module != null && await db.Module.AnyAsync())
                return true;

            if (db.Activity != null && await db.Activity.AnyAsync())
                return true;

            if (db.Document != null && await db.Document.AnyAsync())
                return true;

            if (db.ActivityType != null && await db.ActivityType.AnyAsync())
                return true;

            if (db.Users != null && await db.Users.AnyAsync())
                return true;

            if (db.Roles != null && await db.Roles.AnyAsync())
                return true;

            return false;
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

        private static async Task<IdentityUser> GetTestTeacher()
        {
            var testTeacher = new TeacherUser()
            {
                UserName = "teacher@lms.se",
                Email = "teacher@lms.se",
                FirstName = faker!.Name.FirstName(),
                LastName = faker.Name.LastName()
            };

            var result = await userManager.CreateAsync(testTeacher, testPassword);

            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));

            return testTeacher;
        }

        private static async Task<IdentityUser> GetTestStudent()
        {
            var testStudent = new StudentUser
            {
                UserName = "student@lms.se",
                Email = "student@lms.se",
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName()
            };

            var result = await userManager.CreateAsync(testStudent, testPassword);

            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));

            return testStudent;
        }

        private static async Task<IEnumerable<IdentityUser>> GetTeacherUsersAsync(int nrOfUsers)
        {
            var users = new List<IdentityUser>();

            for (var i = 0; i < nrOfUsers; i++)
            {
                var person = new Person("sv");

                var email = person.Email;
                var found = await userManager.FindByEmailAsync(email);

                if (found != null) return null!;

                var teacher = new TeacherUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = person.FirstName,
                    LastName = person.LastName
                };

                users.Add(teacher);

                var result = await userManager.CreateAsync(teacher, testPassword);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }

            users.Add(await GetTestTeacher());

            return users;
        }

        private static async Task<IEnumerable<IdentityUser>> GetStudentUsersAsync(int nrOfUsers)
        {
            var users = new List<IdentityUser>();

            for (var i = 0; i < nrOfUsers; i++)
            {
                var person = new Person("sv");

                var email = person.Email;
                var found = await userManager.FindByEmailAsync(email);

                if (found != null) return null!;

                var student = new StudentUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = person.FirstName,
                    LastName = person.LastName
                };

                users.Add(student);

                var result = await userManager.CreateAsync(student, testPassword);

                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors));
            }

            users.Add(await GetTestStudent());

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

        private static IEnumerable<ActivityType> GetActivityTypes()
        {
            var activityTypes = new List<ActivityType>() {
                new ActivityType() { ActivityName = "E-Learning" },
                new ActivityType() { ActivityName = "Föreläsning" },
                new ActivityType() { ActivityName = "Övning" },
                new ActivityType() { ActivityName = "Inlämning" }
            };

            return activityTypes;
        }

        private static bool StudentIsFree(IEnumerable<Course> courses, IdentityUser u)
        {
            var result = true;

            foreach (var c in courses)
                if (c.Students.Contains(u))
                    result = false;

            return result;
        }

        private static bool TeacherIsFree(IEnumerable<Course> courses, IdentityUser u)
        {
            var result = true;

            foreach (var c in courses)
                if (c.Teachers.Contains(u))
                    result = false;

            return result;
        }

        private static IEnumerable<Course> GetCourses(
            int nrCourses, int nrModulesPerCourse, int nrActivitiesPerModule,
            IEnumerable<ActivityType> activityTypes,
            IEnumerable<IdentityUser> teachers,
            IEnumerable<IdentityUser> students)
        {
            var courses = new List<Course>();

            for (var i = 0; i < nrCourses; i++)
            {
                var course = new Course
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = DateTime.Now.AddDays(1 + random.Next(40)),
                    EndDate = DateTime.Now.AddDays(60 + random.Next(40))
                };

                course.Modules = GetModules(
                    nrModulesPerCourse,
                    nrActivitiesPerModule,
                    course.StartDate,
                    course.EndDate,
                    activityTypes, teachers, students);

                var nrOfDocuments = random.Next(8);

                for (var j = 0; j < nrOfDocuments; j++)
                {
                    IdentityUser docOwner = teachers.ElementAt(random.Next(teachers.Count()));

                    course.Documents.Add(new Document()
                    {
                        Name = faker!.Lorem.Word(),
                        Description = faker!.Lorem.Sentence(),
                        Timestamp = faker.Date.Recent(10),
                        FilePath = faker.Internet.UrlRootedPath(".pdf"),
                        Owner = docOwner
                    });
                }

                courses.Add(course);
            }

            foreach (var course in courses)
            {
                var nrOfTeachers = 1 + random.Next(3);

                for (var j = 0; j < nrOfTeachers; j++)
                {
                    foreach (var t in teachers)
                    {
                        if (TeacherIsFree(courses, t))
                        {
                            var castedTeacher = t as TeacherUser;
                            if (castedTeacher != null)
                            {
                                course.Teachers.Add(castedTeacher);
                                break;
                            }
                        }
                    }
                }

                var nrOfStudents = 15 + random.Next(15);

                for (var j = 0; j < nrOfStudents; j++)
                {
                    foreach (var student in students)
                    {
                        if (StudentIsFree(courses, student))
                        {
                            var castedStudent = student as StudentUser;

                            if (castedStudent != null)
                            {
                                course.Students.Add(castedStudent);
                                break;
                            }
                        }
                    }
                }
            }

            return courses;
        }

        private static ICollection<Module> GetModules(
            int nrModules, int nrActivitiesPerModule,
            DateTime courseStart, DateTime courseEnd,
            IEnumerable<ActivityType> activityTypes,
            IEnumerable<IdentityUser> teachers,
            IEnumerable<IdentityUser> students)
        {
            var modules = new List<Module>();

            var moduleStartDate = courseStart;

            for (var i = 0; i < nrModules; i++)
            {
                var maxModuleDaysLen = courseEnd.Subtract(new TimeSpan(10, 0, 0, 0)).Subtract(moduleStartDate).Days;

                if (maxModuleDaysLen <= 0)
                    break;

                var moduleEndDate = moduleStartDate.AddDays(5 + random.Next(maxModuleDaysLen));

                if (moduleEndDate > courseEnd)
                    break;

                var module = new Module
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = moduleStartDate,
                    EndDate = moduleEndDate
                };

                var nrOfDocuments = random.Next(8);

                for (var j = 0; j < nrOfDocuments; j++)
                {
                    IdentityUser docOwner = teachers.ElementAt(random.Next(teachers.Count()));

                    module.Documents.Add(new Document()
                    {
                        Name = faker!.Lorem.Word(),
                        Description = faker!.Lorem.Sentence(),
                        Timestamp = faker.Date.Recent(10),
                        FilePath = faker.Internet.UrlRootedPath(".pdf"),
                        Owner = docOwner
                    });
                }

                moduleStartDate = moduleEndDate;

                module.Activities = GetActivites(nrActivitiesPerModule, module.StartDate, module.EndDate,
                    activityTypes, teachers, students);

                modules.Add(module);
            }

            return modules;
        }

        private static ICollection<Activity> GetActivites(int nrActivities,
            DateTime moduleStart, DateTime moduleEnd,
            IEnumerable<ActivityType> activityTypes,
            IEnumerable<IdentityUser> teachers,
            IEnumerable<IdentityUser> students)
        {
            var activites = new List<Activity>();

            var activityStartDate = moduleStart;

            for (var i = 0; i < nrActivities; i++)
            {
                var maxActivityDaysLen = moduleEnd.Subtract(new TimeSpan(2, 0, 0, 0)).Subtract(activityStartDate).Days;

                if (maxActivityDaysLen <= 0)
                    break;

                var activityEndDate = activityStartDate.AddDays(1 + random.Next(maxActivityDaysLen));

                if (activityEndDate > moduleEnd)
                    break;

                var activity = new Activity
                {
                    Name = faker!.Company.CatchPhrase(),
                    Description = faker!.Lorem.Paragraph(),
                    StartDate = activityStartDate,
                    EndDate = activityEndDate,
                    ActivityType = activityTypes.ElementAt(random.Next(activityTypes.Count()))
                };

                var nrOfDocuments = random.Next(8);

                for (var j = 0; j < nrOfDocuments; j++)
                {
                    IdentityUser docOwner;

                    if (random.Next(2) == 0)
                        docOwner = teachers.ElementAt(random.Next(teachers.Count()));
                    else
                        docOwner = students.ElementAt(random.Next(students.Count()));

                    activity.Documents.Add(new Document()
                    {
                        Name = faker!.Lorem.Word(),
                        Description = faker!.Lorem.Sentence(),
                        Timestamp = faker.Date.Recent(10),
                        FilePath = faker.Internet.UrlRootedPath(".pdf"),
                        Owner = docOwner
                    });
                }

                activityStartDate = activityEndDate;

                activites.Add(activity);
            }

            return activites;
        }
    }
}
