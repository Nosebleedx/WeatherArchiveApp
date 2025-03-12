namespace WeatherArchiveApp.Models
{
    public class WeatherArchiveViewModel
    {
        public List<WeatherData> WeatherDataList { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
    }


}
