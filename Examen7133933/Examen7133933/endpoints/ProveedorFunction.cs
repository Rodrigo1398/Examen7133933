using Examen7133933.models;
using Examen7133933.services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Examen7133933.endpoints
{
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly ProveedorService proveedorService;

        public ProveedorFunction(ILogger<ProveedorFunction> logger, ProveedorService _proveedorService)
        {
            _logger = logger;
            proveedorService = _proveedorService;
        }

        [Function("InsertarProveedor")]
        [OpenApiOperation("InsertarProveedor", Description = "Sirve para Insertar Proveedor")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Proveedor))]
        public async Task<HttpResponseData> InsertarProveedor([HttpTrigger(AuthorizationLevel.Function, "post", Route = "InsertarProveedor")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var proveedor = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar una Proveedor");
                proveedor.RowKey = Guid.NewGuid().ToString();
                proveedor.Timestamp = DateTime.UtcNow;
                bool seGuardo = await proveedorService.Create(proveedor);

                if (!seGuardo) return req.CreateResponse(HttpStatusCode.BadRequest);

                resp = req.CreateResponse(HttpStatusCode.OK);

                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("GetAllProveedor")]
        [OpenApiOperation("ListaProveedores", Description = "Sirve para Listar Proveedores")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Producto>),
            Description = "Muestra los proveedores de la base de datos")]
        public async Task<HttpResponseData> GetAllProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var proveedores = await proveedorService.GetAll();

                resp = req.CreateResponse(HttpStatusCode.OK);

                await resp.WriteAsJsonAsync(proveedores);

                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }

        [Function("GetProveedor")]
        [OpenApiOperation("Lista un Proveedor por Id", Description = "Sirve para listar un Proveedor")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "El ID del proveedor")]
        public async Task<HttpResponseData> GetProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var id = req.Query["id"];

                if (id == null) return req.CreateResponse(HttpStatusCode.BadRequest);

                var proveedor = await proveedorService.Get(id);

                resp = req.CreateResponse(HttpStatusCode.OK);

                await resp.WriteAsJsonAsync(proveedor);

                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("UpdateProveedor")]
        [OpenApiOperation("Actualiza un Proveedor", Description = "Sirve para Actualizar un Proveedor")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Proveedor))]
        public async Task<HttpResponseData> UpdateProveedor([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var proveedor = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar una Proveedor");

                bool sw = await proveedorService.Update(proveedor);

                if (!sw) return req.CreateResponse(HttpStatusCode.BadRequest);

                resp = req.CreateResponse(HttpStatusCode.OK);

                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
        [Function("DeleteProveedor")]
        [OpenApiOperation("Elimina un Proveedor", Description = "Sirve para Eliminar un Proveedor")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "el partitionkey del proveedor")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "el rowkey del proveedor")]
        public async Task<HttpResponseData> DeleteProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData resp;
            try
            {
                var partitionkey = req.Query["partitionkey"];
                var rowkey = req.Query["rowkey"];

                if (partitionkey == null || rowkey == null) return req.CreateResponse(HttpStatusCode.BadRequest);

                var Proveedor = await proveedorService.Delete(partitionkey, rowkey);

                resp = req.CreateResponse(HttpStatusCode.OK);

                await resp.WriteAsJsonAsync(Proveedor);

                return resp;
            }
            catch (Exception)
            {
                resp = req.CreateResponse(HttpStatusCode.InternalServerError);
                return resp;
            }
        }
    }
}
