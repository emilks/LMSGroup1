﻿using System.ComponentModel;

namespace LMS.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        [DisplayName ("Namn")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Beskrivning")]
        public string Description { get; set; } = string.Empty;
        [DisplayName("Start datum")]
        public DateTime StartDate { get; set; }
        [DisplayName("Slut datum")]
        public DateTime EndDate { get; set; }

        // Navigation props
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<StudentUser> Students { get; set; } = new List<StudentUser>();
        public ICollection<TeacherUser> Teachers { get; set; } = new List<TeacherUser>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}