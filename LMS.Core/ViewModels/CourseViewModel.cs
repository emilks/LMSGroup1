using LMS.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // document properties
        public int? documentParentId { get; set; }
        public IFormFile? FileBuffer { get; set; }
        [Required]
        [DisplayName("Beskrivning")]
        public string DocumentDescription { get; set; } = string.Empty;

        // Navigation props
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<StudentUser> Students { get; set; } = new List<StudentUser>();
        public ICollection<TeacherUser> Teachers { get; set; } = new List<TeacherUser>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
