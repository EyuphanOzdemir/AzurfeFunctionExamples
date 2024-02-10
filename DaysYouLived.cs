using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DaysYouLived
{
    public static class DaysYouLived
    {
        [FunctionName("DaysCalculator")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dayscalculator/{birthDate}")] HttpRequest req,
        string birthDate,
        ILogger log)
        {
            log.LogInformation("AgeCalculator HTTP trigger function processed a request to calculate age.");

            // Verify birthDate format
            if (!DateTime.TryParse(birthDate, out DateTime parsedDate))
            {
                return new BadRequestObjectResult("Invalid birthDate format. Please use YYYY-MM-DD.");
            }

            // Calculate days passed since birthDate
            var today = DateTime.UtcNow;
            var daysPassed = (today - parsedDate).TotalDays;

            // Return response
            return new OkObjectResult(new { birthDate, daysPassed });
        }
    }
}
