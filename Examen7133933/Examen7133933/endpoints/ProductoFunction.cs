using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Examen7133933.endpoints
{
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;

        public ProductoFunction(ILogger<ProductoFunction> logger)
        {
            _logger = logger;
        }

        [Function("ProductoFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
