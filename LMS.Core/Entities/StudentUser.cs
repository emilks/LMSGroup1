using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities
{
    public class StudentUser : IdentityUser
    {
        [DisplayName("Förnamn")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Efternamn")]
        public string LastName { get; set; } = string.Empty;

        // Foreign keys
        public int? CourseId { get; set; }
    }
}
