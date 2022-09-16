using LMS.Core.Entities;

namespace LMS.Core.ViewModels
{
    public class ContactsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

    }
}