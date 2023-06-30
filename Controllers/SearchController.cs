using ASP121.Models.Orm.AzureSearchWeb;
using ASP121.Models.Orm.AzureSearchVideo;
using ASP121.Models.Search;
using Microsoft.AspNetCore.Mvc;
using ASP121.Models.Orm.AzureSearchNews;
using ASP121.Models.Orm.AzureSearchImage;

namespace ASP121.Controllers
{
    public class SearchController : Controller
    {
        private readonly IConfiguration _configuration;

        public SearchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index([FromQuery]String? search)
        {
            SearchWebModel model = new();
            if(! String.IsNullOrEmpty(search))   //  є пошуковий запит
            {
                var searchSection = _configuration.GetSection("Azure").GetSection("Search");
                String key      = searchSection.GetValue<String>("Key");
                String endpoint = searchSection.GetValue<String>("Endpoint");
                String path     = searchSection.GetValue<String>("PathWeb");

                String query    = $"{endpoint}{path}?q={Uri.EscapeDataString(search)}";

               HttpClient httpClient = new();

                httpClient.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", key);

                String response = httpClient
                    .GetAsync(query).Result.Content
                    .ReadAsStringAsync().Result;

                var searchResponse = System.Text.Json.JsonSerializer
                    .Deserialize<AzureSearchWebResponse>(response);

                model.SearchWebResponse = searchResponse;
            }
            return View(model);
        }

        public ViewResult Video([FromQuery] String? search)        
        {
            SearchVideoModel model = new();
            if (! String.IsNullOrEmpty(search))   //  є пошуковий запит
            {
                var searchSection = _configuration.GetSection("Azure").GetSection("Search");
                String key = searchSection.GetValue<String>("Key");
                String endpoint = searchSection.GetValue<String>("Endpoint");
                String path = searchSection.GetValue<String>("PathVideo");

                String query = $"{endpoint}{path}?q={Uri.EscapeDataString(search)}";

                HttpClient httpClient = new();

                httpClient.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", key);

                String response = httpClient
                    .GetAsync(query).Result.Content
                    .ReadAsStringAsync().Result;

                var searchResponse = System.Text.Json.JsonSerializer
                    .Deserialize<AzureSearchVideoResponse>(response);

               // ViewData["searchResponse"] = response;

                model.SearchVideoResponse = searchResponse;               
            }
            return View(model);  
        }

        public ViewResult News([FromQuery] String? search)
        {
            SearchNewsModel model = new();
            if (!String.IsNullOrEmpty(search))   //  є пошуковий запит
            {
                var searchSection = _configuration.GetSection("Azure").GetSection("Search");
                String key = searchSection.GetValue<String>("Key");
                String endpoint = searchSection.GetValue<String>("Endpoint");
                String path = searchSection.GetValue<String>("PathNews");

                String query = $"{endpoint}{path}?q={Uri.EscapeDataString(search)}";

                HttpClient httpClient = new();

                httpClient.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", key);

                String response = httpClient
                    .GetAsync(query).Result.Content
                    .ReadAsStringAsync().Result;

                var searchResponse = System.Text.Json.JsonSerializer
                    .Deserialize<AzureSearchNewsResponse>(response);

                // ViewData["searchResponse"] = response;
               
                model.SearchNewsResponse = searchResponse;
            }
            return View(model);
        }

        public ViewResult Image([FromQuery] String? search)
        {
            SearchImageModel model = new();
            if (!String.IsNullOrEmpty(search))   //  є пошуковий запит
            {
                var searchSection = _configuration.GetSection("Azure").GetSection("Search");
                String key = searchSection.GetValue<String>("Key");
                String endpoint = searchSection.GetValue<String>("Endpoint");
                String path = searchSection.GetValue<String>("PathImage");

                String query = $"{endpoint}{path}?q={Uri.EscapeDataString(search)}";

                HttpClient httpClient = new();

                httpClient.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", key);

                String response = httpClient
                    .GetAsync(query).Result.Content
                    .ReadAsStringAsync().Result;

                var searchResponse = System.Text.Json.JsonSerializer
                    .Deserialize<AzureSearchImageResponse>(response);

                //ViewData["searchResponse"] = response;                

                model.SearchImageResponse = searchResponse;
            }
            return View(model);
        }
    }
}
