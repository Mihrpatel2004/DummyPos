using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System;

namespace DummyPos.Filters
{
    public class LogActivityFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // This runs the exact millisecond BEFORE the Controller method starts
            string actionName = context.ActionDescriptor.DisplayName;
            Debug.WriteLine($"[ACTIVITY LOG] Started '{actionName}' at {DateTime.Now}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This runs the exact millisecond AFTER the Controller method finishes
            string actionName = context.ActionDescriptor.DisplayName;
            Debug.WriteLine($"[ACTIVITY LOG] Successfully finished '{actionName}' at {DateTime.Now}");
        }
    }
}