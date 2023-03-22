using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Exercise.Model;

namespace Exercise.WebApp.Pages
{
    public class Model1WithoutLogin : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ModelType1 Item { get; set; }

        public Model1WithoutLogin(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            Item = await InvokeEndPoint<ModelType1>("ExerciseWebApi", "Data/ModelType1");
        }

        private async Task<T> InvokeEndPoint<T>(string clientName, string url)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var mt1 = new ModelType1() { SomeOtherField = "ModelType1", DenyUnlessLoggedIn = true };
            var dataAsString = JsonConvert.SerializeObject(mt1);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode) 
            {
                // Note: To display the error from API
                mt1.TagFromServer = "401: Unauthorized";
                dataAsString = JsonConvert.SerializeObject(mt1);
                return JsonConvert.DeserializeObject<T>(dataAsString);
            }

            dataAsString = await response.Content.ReadAsStringAsync();
            T item = JsonConvert.DeserializeObject<T>(dataAsString);

            return item;
        }
    }
}
