using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Exercise.Model;

namespace Exercise.WebApp.Pages
{
    //[Authorize(Policy = "LoginRequired")]
    public class Model1 : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ModelType1 Item { get; set; }

        public Model1(IHttpClientFactory httpClientFactory)
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
            var dataAsString = JsonConvert.SerializeObject(new ModelType1() { 
                SomeOtherField = "ModelType1", DenyUnlessLoggedIn = false 
            });
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var res = await httpClient.PostAsync(url, content);
            dataAsString = await res.Content.ReadAsStringAsync();
            T item = JsonConvert.DeserializeObject<T>(dataAsString);

            return item;
        }
    }
}
