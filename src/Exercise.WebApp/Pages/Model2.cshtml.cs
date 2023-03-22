using Exercise.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Exercise.WebApp.Pages
{
    public class Model2 : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ModelType2 Item { get; set; }

        public Model2(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            Item = await InvokeEndPoint<ModelType2>("ExerciseWebApi", "Data/ModelType2");
        }

        private async Task<T> InvokeEndPoint<T>(string clientName, string url)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var dataAsString = JsonConvert.SerializeObject(new ModelType2() { SomeOtherField = "ModelType2" });
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(url, content);
            dataAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dataAsString);
        }
    }
}
