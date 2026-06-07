using Microsoft.AspNetCore.Mvc;

namespace SerilogFileLog.Controllers;
[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(ILogger<WeatherController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Weather endpoint called");
        
        return Ok("Hello Serilog");
    }
    
    [HttpGet("GetDivision")]
    public IActionResult GetDivision(int number)
    {
        try
        {
            _logger.LogInformation("GetDivision endpoint called with number: {Number}", number);

            int result = 10 / number;

            _logger.LogInformation(
                "Division completed successfully. Result: {Result}",
                result);

            return Ok(new
            {
                Success = true,
                Result = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An error occurred in GetDivision. Input Number: {Number}",
                number);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}