using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Presentation.Filters
{
    public class RequireLoginFilter : IActionFilter
    {
        private readonly ILogger<RequireLoginFilter> _logger;

        public RequireLoginFilter(ILogger<RequireLoginFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"];

            if (actionName == "Login" || actionName == "Register")
            {
                return;  
            }

            var username = context.HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("Unauthorized access attempt.");
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
