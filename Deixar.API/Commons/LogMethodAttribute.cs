using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Globalization;

namespace Deixar.API.Commons
{
    /// <summary>
    /// Custom attribute which log each HTTP request and response with datetime stamp
    /// </summary>
    public class LogMethodAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// Work that happen before action execution
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.Controller.GetType().Name;
            string actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            Log.Logger.Information("{controllerName} -> {actionName} -> Executing - {DT}", controllerName, actionName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Work that happen after action execution
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string controllerName = context.Controller.GetType().Name;
            string actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            Log.Logger.Information("{controllerName} -> {actionName} -> Executed - {DT}\n", controllerName, actionName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
        }
    }
}
