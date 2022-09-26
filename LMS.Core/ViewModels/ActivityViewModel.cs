#nullable disable
using LMS;
using LMS.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.ViewModels
{
    public class ActivityViewModel
    {
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
        [DataType(DataType.Date)]
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

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}