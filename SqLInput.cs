using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SqlInput
{
    public static class SqlInput
    {
        [FunctionName("SqlInput")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, 
                                        ILogger log,
                                        [Sql(commandText:"select * from products where productid= @productid", 
                                             commandType:System.Data.CommandType.Text,
                                             connectionStringSetting: "SqlConnectionString",
                                             parameters:"@productid={Query.productid}"
                                             )] IEnumerable<Object> result)
        {
            log.LogInformation("SqlInput HTTP trigger function processed a request.");
            return new OkObjectResult(result);
        }
    }
}
