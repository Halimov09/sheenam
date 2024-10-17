//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class HomeController : RESTFulController
    {
        private readonly ILogger<HomeController> logger;

        private HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            logger.LogTrace("I am Logger Trace");
            logger.LogDebug("I am Debugging");
            logger.LogInformation("I am Logger Info");
            logger.LogError("I am Logger Error");
            logger.LogWarning("I am Logger Warning");
            logger.LogCritical("I am Logger Critical");

            return Ok("hello world");
        }
    }
}
