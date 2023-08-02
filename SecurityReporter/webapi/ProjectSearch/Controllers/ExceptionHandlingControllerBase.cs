using Microsoft.AspNetCore.Mvc;
using System.Net;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Controllers
{
    public class ExceptionHandlingControllerBase : ControllerBase
    {
        protected ILogger Logger;

        public ExceptionHandlingControllerBase(ILogger<ExceptionHandlingControllerBase> logger)
        {
            Logger = logger;
        }

        protected async Task<IActionResult> HandleExceptionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (CustomException ex)
            {
                Logger.LogError(ex, "An exception occurred while processing the request.");
                return StatusCode(ex.StatusCode, new ErrorResponse(ex.Message, ex.Details));
            }
            catch (NotImplementedException ex)
            {
                Logger.LogError(ex, "Method not implemented.");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse("Method not implemented.", null));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unhandled exception occurred.");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse("An unexpected error occurred.", null));
            }
        }
    }
}
