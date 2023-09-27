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
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(gl))
            {
                return RedirectToAction("Error");
            }
            try
            {
                FeedbackViewModel feedbackViewModel = new()
                {
                    Rating = _configRoot.GetSection($"{gl}:Rating").Value,
                    Comment = _configRoot.GetSection($"{gl}:Comment").Value
                };
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