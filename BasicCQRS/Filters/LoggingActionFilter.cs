using Microsoft.AspNetCore.Mvc.Filters;

namespace BasicCQRS.Filters
{
    public class LoggingActionFilter : IActionFilter 
    {
        // Implementing IActionFilter interface. This is a filter that runs before and after an action method.
        // action method is the method in the controller that is called when a request is made to the API.
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
