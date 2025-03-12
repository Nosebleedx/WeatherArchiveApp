using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WeatherArchiveApp.Services
{
    public class WeatherArchiveService
    {
        private readonly WeatherDataService _weatherDataService;

        // Конструктор с инжекцией зависимостей для WeatherDataService
        public WeatherArchiveService(WeatherDataService weatherDataService)
        {
            _weatherDataService = weatherDataService;
        }

        public async Task<(bool Success, string ErrorMessage)> ProcessFilesAsync(IEnumerable<IFormFile> files)
        {
            if (files == null || !files.Any())
            {
                return (false, "Файл не выбран.");
            }

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    return (false, "Один из файлов пуст.");
                }
                try
                {
                    using var stream = file.OpenReadStream();
                    IWorkbook workbook = new XSSFWorkbook(stream); 
                    ISheet sheet = workbook.GetSheetAt(0);
                    bool fileHasData = false;

                    for (int rowIndex = 5; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null) continue;

                        var tt = ParseTime(row.GetCell(1));

                        var data = new WeatherData
                        {
                            DateTime = ParseDate(row.GetCell(0)),
                            Time = ParseTime(row.GetCell(1)),
                            Temperature = ParseFloat(row.GetCell(2)),
                            Humidity = ParseInt(row.GetCell(3)),
                            DewPoint = ParseFloat(row.GetCell(4)),
                            Pressure = ParseInt(row.GetCell(5)),
                            WindDirection = row.GetCell(6)?.ToString()?.Trim(),
                            WindSpeed = ParseInt(row.GetCell(7)),
                            Cloudiness = ParseInt(row.GetCell(8)),
                            CloudBaseHeight = ParseInt(row.GetCell(9)),
                            Visibility = ParseInt(row.GetCell(10)),
                            WeatherPhen = row.GetCell(11)?.ToString()?.Trim()
                        };

                        await _weatherDataService.AddAsync(data);
                        fileHasData = true;
                    }

                    if (!fileHasData)
                    {
                        return (false, $"Файл {file.FileName} не содержит данных.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке файла {file.FileName}: {ex.Message}");
                    return (false, "Произошла ошибка при обработке файла.");
                }
            }

            return (true, "Все файлы успешно загружены.");
        }



        private DateOnly ParseDate(ICell cell)
        {
            if (cell == null) return new DateOnly(2025, 01, 01);

            string dateString = cell.ToString().Trim();

            if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return DateOnly.FromDateTime(date);
            }

            return new DateOnly(2025, 01, 01);
        }


        private TimeOnly ParseTime(ICell cell)
        {
            if (cell == null) return new TimeOnly(00, 00);
            string timeString = cell.ToString().Trim();

            if (DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                return TimeOnly.FromDateTime(time);

            return new TimeOnly(00, 00);
        }

        private float? ParseFloat(ICell cell)
        {
            if (cell == null) return null;
            float.TryParse(cell.ToString().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float result);
            return result;
        }

        private int? ParseInt(ICell cell)
        {
            if (cell == null) return null;
            int.TryParse(cell.ToString(), out int result);
            return result;
        }
    }
}
