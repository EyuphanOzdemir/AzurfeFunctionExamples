using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pelicanaf.models;
using System.Net;

namespace SqlOutput
{
    public static class SqlOutput
    {
        [FunctionName("SqlOutput")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] 
                                                     HttpRequest req, 
                                                     ILogger log, 
                                                     [Sql("Products", "SqlConnectionString")] 
                                                     IAsyncCollector<Product> products)
        {
            log.LogInformation("SqlOutput HTTP trigger function processed a request.");
            var requestBody =await new StreamReader(req.Body).ReadToEndAsync(); 
            string error=null;
            Product newProduct = new();
            try
            {
                newProduct = JsonConvert.DeserializeObject<Product>(requestBody);
                await products.AddAsync(newProduct);
            }
            catch (Exception ex)
            {
                error = ex.Message;     
            }
            if(!string.IsNullOrEmpty(error))
               return new ObjectResult(error){StatusCode=StatusCodes.Status304NotModified};
            
            return new OkObjectResult(newProduct);
            //return new OkResult();
        }
    }
}
