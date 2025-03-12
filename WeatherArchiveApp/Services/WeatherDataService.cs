using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WeatherArchiveApp.Data;
using WeatherArchiveApp.Models;

namespace WeatherArchiveApp.Services
{

    public class WeatherDataService
    {
        private readonly AppDbContext _context;
        private Random _random;
        private const int PageSize = 20;

        public WeatherDataService(AppDbContext context)
        {
            _context = context;
            _random = new Random();
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


        public async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                var testData = new WeatherData
                {
                    DateTime = new DateOnly(2025, 01, 01),
                    Time = new TimeOnly(00, 00),
                    Temperature = -5.5f,
                    Humidity = 89,
                    DewPoint = -6.9f,
                    Pressure = 737,
                    WindDirection = "З,ЮЗ",
                    WindSpeed = 1,
                    Cloudiness = 100,
                    CloudBaseHeight = 800,
                    Visibility = 10,
                    WeatherPhen = "Дымка"
                };

                await _context.WeatherData.AddAsync(testData);
                await _context.SaveChangesAsync();

                Console.WriteLine("Данные успешно добавлены.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении данных в БД: {ex.Message}");
                return false;
            }
        }
    }    
}
