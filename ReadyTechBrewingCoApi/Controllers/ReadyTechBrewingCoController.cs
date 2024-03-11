using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReadyTechBrewingCoApi.Interfaces;
using System;
using System.Diagnostics;
using System.Net;

[ApiController]
[Route("[controller]")]
public class ReadyTechBrewingCoController : ControllerBase
{
    private readonly IMemoryCacheWrapper _cache;
    private readonly ILogger<ReadyTechBrewingCoController> _logger;   
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReadyTechBrewingCoController(IMemoryCacheWrapper cache, ILogger<ReadyTechBrewingCoController> logger, IDateTimeProvider dateTimeProvider)
    {
        _cache = cache;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpGet("brew-coffee")]
    public IActionResult BrewCoffee()
    {       
        try
        {
            int brewCount;
            if(!_cache.TryGetValue("brewCount", out brewCount))
            {
                brewCount = 0;
            }

            brewCount++;

            _cache.Set("brewCount", brewCount, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration time as needed
            });

            LogCoffeeEvent(brewCount);

            DateTime now = _dateTimeProvider.Now;
            string formattedDateTime = now.ToString("yyyy-MM-ddTHH:mm:sszzz");

            if (IsAprilFoolsDay(now))
            {
                _logger.LogInformation("April Fools! The coffee machine is a teapot today.");
                return new ContentResult{
                    StatusCode = 418,
                    Content = string.Empty,
                    ContentType = "application/json"
                };
            }

            if (brewCount % 5 == 0)
            {
                _logger.LogWarning("Coffee machine is out of coffee!");
                return new ContentResult {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable,
                    Content = string.Empty,
                    ContentType = "application/json"
                };
            }            

            var response = new
            {
                message = "Your piping hot coffee is ready",
                prepared = formattedDateTime
            };

            _logger.LogInformation("Coffee brewed successfully!");
            return Ok(response);           
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while brewing coffee: {ex.Message}");
            return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    private void LogCoffeeEvent(int brewCount)
    {
        _logger.LogInformation($"Brewing coffee attempt #{brewCount}");
    }

    private bool IsAprilFoolsDay(DateTime dateTime)
    {
        return dateTime.Month == 4 && dateTime.Day == 1;
    }
}