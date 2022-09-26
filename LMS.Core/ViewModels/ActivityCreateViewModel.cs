using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    internal class ActivityCreateViewModel
    {
        public int CourseId { get; set; }
        public int ModuleId { get; set; }


        [DisplayName("Aktivitetsnamn")]
        public string Name { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
        [DisplayName("Aktivitetstyp")]
        public int ActivityTypeId { get; set; }
        [DisplayName("Startdatum")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [DisplayName("Längd (antal dagar)")]
        public int Duration
        {
            get
            {
                return (EndDate - StartDate).Days;
            }
            set
            {
                EndDate = StartDate + TimeSpan.FromDays(value);
            }
        }

        public void ActivityViewModel(int courseId, int moduleId)
        {
            CourseId = courseId;
            ModuleId = moduleId;
        }
    }
}
