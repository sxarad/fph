using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Domain;
using Exercise.Model;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Exercise.WebApp.Pages
{
    [Authorize(Policy = "LoginRequired")]
    public class Model1WithLogin : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ModelType1 Item { get; set; }

        public Model1WithLogin(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            Item = await InvokeEndPoint<ModelType1>("ExerciseWebApi", "Data/ModelType1");
        }

        private async Task<T> InvokeEndPoint<T>(string clientName, string url)
        {
            JwtToken token;

            var strTokenObj = HttpContext.Session.GetString("access_token");
            if (string.IsNullOrWhiteSpace(strTokenObj))
                token = await Authenticate();
            else
                token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj);

            if (token == null ||
                string.IsNullOrWhiteSpace(token.AccessToken) ||
                token.ExpiresAt <= DateTime.UtcNow)
                token = await Authenticate();

            var httpClient = _httpClientFactory.CreateClient(clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var dataAsString = JsonConvert.SerializeObject(new ModelType1() { SomeOtherField = "ModelType1", DenyUnlessLoggedIn = true });
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var res = await httpClient.PostAsync(url, content);
            dataAsString = await res.Content.ReadAsStringAsync();
            T item = JsonConvert.DeserializeObject<T>(dataAsString);

            return item;
        }

        private async Task<JwtToken> Authenticate()
        {            
            var httpClient = _httpClientFactory.CreateClient("ExerciseWebApi");

            string userName = string.Empty;
            string password = string.Empty;
            foreach (var c in User.Claims)
            {
                if (c.Type == "UserName")
                    userName = c.Value;
                if (c.Type == "Password")
                    password = c.Value;
            }

            var response = await httpClient.PostAsJsonAsync("auth", new Credential { UserName = userName, Password = password });
            response.EnsureSuccessStatusCode();
            string jwt = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", jwt);

            return JsonConvert.DeserializeObject<JwtToken>(jwt);
        }
    }
}
