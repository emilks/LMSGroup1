using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.ViewModels
{
    public class DocumentViewModel
    {
        public int ParentId { get; set; }

        [DisplayName("Beskrivning")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IFormFile? FileBuffer { get; set; }
    }
}
