using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherArchiveApp.Data;
using WeatherArchiveApp.Models;
using WeatherArchiveApp.Services;

public class WeatherController : Controller
{
    private readonly AppDbContext _context;
    private readonly WeatherArchiveService _weatherArchiveService;
    private readonly WeatherDataService _weatherDataService;
    private const int PageSize = 20;

    public WeatherController(AppDbContext context, WeatherArchiveService weatherArchiveService, WeatherDataService weatherDataService)
    {
        _context = context;
        _weatherArchiveService = weatherArchiveService;
        _weatherDataService = weatherDataService;
    }

    public async Task<IActionResult> Index()
    {
        var weatherList = await _context.WeatherData.OrderByDescending(w => w.DateTime).ToListAsync();
        return View(weatherList); 
    }

    [HttpGet]
    public async Task<IActionResult> ViewArchives(int page = 1, int SelectedYear = 0, int SelectedMonth = 0)
    {
        var (data, totalCount) = await _weatherDataService.GetWeatherDataAsync(
            page,
            SelectedYear > 0 ? SelectedYear : null,
            SelectedMonth > 0 ? SelectedMonth : null
        );

        var viewModel = new WeatherArchiveViewModel
        {
            WeatherDataList = data,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize),
            SelectedYear = SelectedYear,
            SelectedMonth = SelectedMonth
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult UploadArchives()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> UploadArchives(IEnumerable<IFormFile> files)
    {
        var (success, errorMessage) = await _weatherArchiveService.ProcessFilesAsync(files);

        if (!success)
        {
            TempData["ErrorMessage"] = errorMessage;  
            return RedirectToAction("UploadArchives");  
        }

        TempData["SuccessMessage"] = errorMessage;
        return RedirectToAction("UploadArchives");
    }

}

