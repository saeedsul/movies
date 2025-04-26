using Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseController()
        { 
            _logger = Log.ForContext(this.GetType());
        }

        protected IActionResult Success<T>(T data, string message)
        {
            _logger.Information("Success: {Message}", message);  
            return Ok(new ApiResponse<T> { Success = true, Message = message, Data = data });
        }
         
        protected IActionResult Failure<T>(string message)
        {
            _logger.Error("Failure: {Message}", message);  
            return NotFound(new ApiResponse<T> { Success = false, Message = message, Data = default });
        }

    }
}
