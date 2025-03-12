using Microsoft.EntityFrameworkCore;
using WeatherArchiveApp.Data;

namespace WeatherArchiveApp.Services
{

    public class WeatherDataService
    {
        private readonly AppDbContext _context;
        private const int PageSize = 20;

        public WeatherDataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<WeatherData> Data, int TotalCount)> GetWeatherDataAsync(
        int page, int? year, int? month)
        {
            var query = _context.WeatherData.AsQueryable();

            if (year.HasValue)
                query = query.Where(w => w.DateTime.Year == year.Value);

            if (month.HasValue)
                query = query.Where(w => w.DateTime.Month == month.Value);

            int totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(w => w.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task AddAsync(WeatherData data)
        {
            _context.WeatherData.Add(data);
            await _context.SaveChangesAsync();
        }
    }
}
