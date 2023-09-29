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
        private readonly IConfiguration _configRoot;
        private readonly string _tenantId;
        private readonly string _resource;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _authority;
        private readonly string _entityName;
        public HomeController(ILogger<HomeController> logger, IDataverseHelper dataverseHelper, IConfiguration configRoot)
        {
            _logger = logger;
            _dataverseHelper = dataverseHelper;
            _configRoot = configRoot;
            _tenantId = _configRoot.GetSection("DataverseConfig:TenantId").Value;
            _resource = _configRoot.GetSection("DataverseConfig:Resource").Value;
            _clientId = _configRoot.GetSection("DataverseConfig:ClientId").Value;
            _clientSecret = _configRoot.GetSection("DataverseConfig:ClientSecret").Value;
            _authority = _configRoot.GetSection("DataverseConfig:Authority").Value;
            _entityName = _configRoot.GetSection("DataverseConfig:EntityName").Value;
        }

        public async Task<IActionResult> Index([FromQuery] string id, [FromQuery] string gl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error");
            }
            try
            {
                FeedbackViewModel feedbackViewModel = new()
                {
                    Rating = _configRoot.GetSection($"{gl}:Rating").Value,
                    Comment = _configRoot.GetSection($"{gl}:Comment").Value,
                    Title = _configRoot.GetSection($"{gl}:Title").Value,
                    Msg = _configRoot.GetSection($"{gl}:Msg").Value,
                    Header = _configRoot.GetSection($"{gl}:Header").Value,
                    Submit = _configRoot.GetSection($"{gl}:Submit").Value,
                    SubmitConfirm = _configRoot.GetSection($"{gl}:SubmitConfirm").Value,
                };
                if (string.IsNullOrEmpty(feedbackViewModel.Rating))
                {
                    feedbackViewModel.Rating = _configRoot.GetSection($"EN:Rating").Value;
                }
                if (string.IsNullOrEmpty(feedbackViewModel.Comment))
                {
                    feedbackViewModel.Comment = _configRoot.GetSection($"EN:Comment").Value;
                }
                if (string.IsNullOrEmpty(feedbackViewModel.Title))
                {
                    feedbackViewModel.Title = _configRoot.GetSection($"EN:Title").Value;
                }
                if (string.IsNullOrEmpty(feedbackViewModel.Msg))
                {
                    feedbackViewModel.Msg = _configRoot.GetSection($"EN:Msg").Value;
                }
                if (string.IsNullOrEmpty(feedbackViewModel.Header))
                {
                    feedbackViewModel.Header = _configRoot.GetSection($"EN:Header").Value;
                }
                if (string.IsNullOrEmpty(feedbackViewModel.Submit))
                {
                    feedbackViewModel.Submit = _configRoot.GetSection($"EN:Submit").Value;
                }
                if (string.IsNullOrEmpty(feedbackViewModel.SubmitConfirm))
                {
                    feedbackViewModel.SubmitConfirm = _configRoot.GetSection($"EN:SubmitConfirm").Value;
                }

                return View(feedbackViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveFeedback([FromQuery] string id, [FromBody] FeedbackViewModel feedbackViewModel)
        {
            string accessToken = await _dataverseHelper.GetAccessToken(_resource, _clientId, _clientSecret, _authority);
            bool status = await _dataverseHelper.UpdateField(_resource, accessToken, _entityName, id, feedbackViewModel);
            return Ok(status);
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