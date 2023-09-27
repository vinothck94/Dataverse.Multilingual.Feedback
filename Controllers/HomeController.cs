using Dataverse.Multilingual.Feedback.Helper;
using Dataverse.Multilingual.Feedback.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dataverse.Multilingual.Feedback.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataverseHelper _dataverseHelper;
        private readonly ISettings _settings;
        private readonly IConfiguration _configRoot;
        public HomeController(ILogger<HomeController> logger, IDataverseHelper dataverseHelper, ISettings settings, IConfiguration configRoot)
        {
            _logger = logger;
            _dataverseHelper = dataverseHelper;
            _settings = settings;
            _configRoot = configRoot;
        }

        public async Task<IActionResult> Index([FromQuery] string id, [FromQuery] string gl)
        {
            string accessToken = await _dataverseHelper.GetAccessToken("", _settings.DataverseConfig.ClientId, _settings.DataverseConfig.ClientSecret, _settings.DataverseConfig.Authority);

            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(gl))
            {
                return RedirectToAction("Error");
            }
            try
            {
                List<QuestionViewModel> questionViewModel = new()
                {
                    new QuestionViewModel{Question=_configRoot.GetSection($"{gl}:Question1").Value},
                    new QuestionViewModel{Question=_configRoot.GetSection($"{gl}:Question2").Value},
                    new QuestionViewModel{Question=_configRoot.GetSection($"{gl}:Question3").Value},
                    new QuestionViewModel{Question=_configRoot.GetSection($"{gl}:Question4").Value}
                };
                return View(questionViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public void SaveFeedback([FromBody] List<QuestionViewModel> questionViewModel)
        {

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}