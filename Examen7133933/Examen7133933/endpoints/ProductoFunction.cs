using Examen7133933.models;
using Examen7133933.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Examen7133933.endpoints
{
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly ProductoService productoService;

        public ProductoFunction(ILogger<ProductoFunction> logger, ProductoService _productoService)
        {
            _logger = logger;
            productoService = _productoService;
        }

        [Function("InsertarProducto")]
        [OpenApiOperation("InsertarProducto", Description = "Sirve para Insertar Producto")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Producto))]
        public async Task<HttpResponseData> InsertarProducto([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var producto = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar un Producto");
                producto.RowKey = Guid.NewGuid().ToString();
                producto.Timestamp = DateTime.UtcNow;
                bool sw = await productoService.Create(producto);
                if (sw)
                {
                    resp = req.CreateResponse(HttpStatusCode.OK);
                    return resp;
                }
                else
                {
                    resp = req.CreateResponse(HttpStatusCode.BadRequest);
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("GetAllProducto")]
        [OpenApiOperation("ListarProductos", Description = "Sirve para Listar Productos")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Producto>),
            Description = "Muestra los productos de la base de datos")]
        public async Task<HttpResponseData> GetAllProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var productos = productoService.GetAll();
                resp = req.CreateResponse(HttpStatusCode.OK);
                await resp.WriteAsJsonAsync(productos.Result);
                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("DeleteProducto")]
        [OpenApiOperation("Elimina un Producto", Description = "Sirve para Eliminar un Producto")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(void))]
        public async Task<HttpResponseData> DeleteProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData resp;
            var partitionkey = req.Query["partitionkey"];
            var rowkey = req.Query["rowkey"];

            try
            {
                bool validate = await productoService.Delete(partitionkey, rowkey);
                if (validate)
                {
                    resp = req.CreateResponse(HttpStatusCode.OK);
                    return resp;
                }
                else
                {
                    resp = req.CreateResponse(HttpStatusCode.BadRequest);
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("GetProducto")] 
        [OpenApiOperation("Lista un Producto por Id", Description = "Sirve para listar un Producto")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(void))]
        public async Task<HttpResponseData> ObtenerProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData resp;
            var id = req.Query["Id"];
            if (id == null) return req.CreateResponse(HttpStatusCode.BadRequest);
            try
            {
                var producto = productoService.Get(id);
                resp = req.CreateResponse(HttpStatusCode.OK);
                await resp.WriteAsJsonAsync(producto.Result);
                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("UpdateProducto")]
        [OpenApiOperation("Actualiza un Producto", Description = "Sirve para Actualizar un Producto")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Producto))]
        public async Task<HttpResponseData> UpdateProducto([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var producto = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar un Producto");

                bool sw = await productoService.Update(producto);
                if (sw)
                {
                    resp = req.CreateResponse(HttpStatusCode.OK);
                    return resp;
                }
                else
                {
                    resp = req.CreateResponse(HttpStatusCode.BadRequest);
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
    }
}
