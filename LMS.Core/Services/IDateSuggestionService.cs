using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Services
{
    public interface IDateSuggestionService
    {
        Task<DateTime> GetSuggestedModuleStartDate(int courseId);
    }
}
