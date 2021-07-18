using Conciliator.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conciliator.App.Interfaces
{
    public interface IExtractRepository : IRepository<Extract>
    {
        Task<Extract> GetLastExtract();
        Task<List<Extract>> SearchByDates(DateTime? minDate, DateTime? maxDate);
    }
}
