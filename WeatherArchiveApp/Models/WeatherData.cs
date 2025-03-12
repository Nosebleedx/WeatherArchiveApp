using System.ComponentModel.DataAnnotations.Schema;

[Table("WeatherDatas")]
public class WeatherData
{
    public int Id { get; set; }
    public DateOnly DateTime { get; set; }
    public TimeOnly Time { get; set; } 
    public float? Temperature { get; set; }
    public int? Humidity { get; set; }
    public float? DewPoint { get; set; }
    public int? Pressure { get; set; }
    public string WindDirection { get; set; }
    public int? WindSpeed { get; set; }
    public int? Cloudiness { get; set; }
    public int? CloudBaseHeight { get; set; }
    public int? Visibility { get; set; }
    public string? WeatherPhen { get; set; }
}
