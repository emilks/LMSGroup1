#nullable disable
using LMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.ViewModels
{
    public class ActivityViewModel
    {
        [DisplayName("Modulnamn")]
        public string Name { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
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
    }
}