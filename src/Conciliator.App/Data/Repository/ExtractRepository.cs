using Conciliator.App.Data.Context;
using Conciliator.App.Interfaces;
using Conciliator.App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conciliator.App.Data.Repository
{
    public class ExtractRepository : Repository<Extract>, IExtractRepository
    {
        public ExtractRepository(ConciliatorDbContext context): base(context)
        {
        }

        public async Task<List<Extract>> SearchByDates(DateTime? startDate, DateTime? endDate)
        {
            var result = await Db.Extracts.AsNoTracking()
                .Where(e => e.DatePosted >= startDate && e.DatePosted <= endDate)
                .OrderBy(e => e.DatePosted)
                .ToListAsync();
            return result; 
        }

        public async Task<List<Extract>> GetAllExtractsOrdered()
        {
            return await Db.Extracts.AsNoTracking().OrderBy(e => e.DatePosted).ToListAsync();
        }

        public async Task<Extract> GetLastExtract()
        {
            return await Db.Extracts.AsNoTracking().OrderBy(e => e.DatePosted).FirstOrDefaultAsync();
        }
    }
}
