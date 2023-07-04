using ASP121.Models.Translator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;
using System.Text;
using ASP121.Models.Orm.Translator;

namespace ASP121.Controllers
{
    public class TranslatorController : Controller
    {
        private readonly IConfiguration _configuration;

        public TranslatorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ViewResult> Index(TranslatorFormModel? formModel)
        {
            TranslatorViewModel viewModel = new();
            viewModel.FormModel = formModel;

            if(formModel != null && (formModel.TranslateButton != null || formModel.InverseButton != null))
            {
                var section = _configuration.GetSection("Azure").GetSection("Translator");
                String key = section.GetValue<String>("Key");
                String endpoint = section.GetValue<String>("Endpoint");
                String region = section.GetValue<String>("Region");

                string route = "/translate?api-version=3.0&";
                string textToTranslate = "";

                if (formModel.TranslateButton != null) // звичайний переклад
                {
                    if (String.IsNullOrEmpty(formModel.OriginalText))
                    {
                        viewModel.ErrorMessage = "Відсутній текст для перекладу";
                    }
                    else
                    {
                        route += $"from={formModel.LangFrom}&to={formModel.LangTo}";
                        textToTranslate = formModel.OriginalText;
                                  
                    }
                }
                else if (formModel.InverseButton != null) // зворотний переклад
                {
                    if (String.IsNullOrEmpty(formModel.TranslatedText))
                    {
                        viewModel.ErrorMessage = "Відсутній текст для перекладу";
                    }
                    else
                    {
                        route += $"from={formModel.LangTo}&to={formModel.LangFrom}";
                        string tempLang = viewModel.FormModel.LangFrom;
                        viewModel.FormModel.LangFrom = viewModel.FormModel.LangTo;
                        viewModel.FormModel.LangTo = tempLang;

                        string tempText = viewModel.FormModel.OriginalText;
                        viewModel.FormModel.OriginalText = viewModel.FormModel.TranslatedText;
                        viewModel.FormModel.TranslatedText = tempText;
                        textToTranslate = formModel.TranslatedText;                       
                    }
                }

                if (viewModel.ErrorMessage == null)
                {
                    object[] body = new object[] { new { Text = textToTranslate } };
                    var requestBody = JsonSerializer.Serialize(body);

                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage())
                    {                       
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(endpoint + route);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                      
                        request.Headers.Add("Ocp-Apim-Subscription-Region", region);

                        
                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);                        
                        string result = await response.Content.ReadAsStringAsync();
                       

                        var translation = JsonSerializer.Deserialize<List<TranslationResult>>(result);

                        viewModel.FormModel.TranslatedText = translation[0].translations[0].text;

                        //viewModel.ErrorMessage = result;
                        //Console.WriteLine(result);
                       
                    }
                }
            }
            return View(viewModel);
        }
    }
}
