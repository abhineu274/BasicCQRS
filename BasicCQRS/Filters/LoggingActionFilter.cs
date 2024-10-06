using Microsoft.AspNetCore.Mvc.Filters;

namespace BasicCQRS.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Logic before action executes
            Console.WriteLine("Action is executing...");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Logic after action executes
            Console.WriteLine("Action has executed.");
        }
    }
}
