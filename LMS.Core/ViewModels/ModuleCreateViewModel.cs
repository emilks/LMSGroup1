using LMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class ModuleCreateViewModel
    {
        public MainCourseIndexViewModel Course { get; set; }
        public ModuleViewModel Module { get; set; }
    }
}
