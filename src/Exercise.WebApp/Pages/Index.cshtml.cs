using Exercise.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Exercise.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        [BindProperty] public IEnumerable<WeatherForecast> WeatherForecastModel { get; set; } = new List<WeatherForecast>();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnGet()
        {
            _logger.LogDebug("IndexModel.OnGet()");

            var reservationList = new List<WeatherForecast>();
            using (var httpClient = new HttpClient())
            {
                var url = _configuration["ApiBaseUrl"] + "WeatherForecast";
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var items = JsonConvert.DeserializeObject<List<WeatherForecast>>(apiResponse);
                    if (items != null) { reservationList.AddRange(items); }
                }
            }

            WeatherForecastModel = reservationList;
        }
    }
}