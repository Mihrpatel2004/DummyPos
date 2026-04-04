using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DummyPos.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // 1. Capture the exact error so you (the developer) can see it in debugging
            Console.WriteLine($"[SYSTEM CRASH PREVENTED] Error: {context.Exception.Message}");

            // 2. Show a friendly message to the staff instead of a scary code screen
            context.Result = new ContentResult
            {
                Content = "Oops! Something went wrong with the system. Please refresh the page and try again.",
                ContentType = "text/plain",
                StatusCode = 500
            };

            // 3. Tell ASP.NET Core: "I handled the crash safely, do not break the website!"
            context.ExceptionHandled = true;
        }
    }
}