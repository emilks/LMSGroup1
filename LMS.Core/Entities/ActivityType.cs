using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities
{
    public class ActivityType
    {
        public int Id { get; set; }
        [DisplayName("Aktivitetsnamn")]
        public string ActivityName { get; set; } = string.Empty;
    }
}
