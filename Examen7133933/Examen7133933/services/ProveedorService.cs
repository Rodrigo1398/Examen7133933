using Azure.Data.Tables;
using Examen7133933.interfaces;
using Examen7133933.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen7133933.services
{
    public class ProveedorService : IProveedor
    {
        public readonly string cadenaConexion;
        public readonly string tablaNombre;
        public readonly IConfiguration configuration;
        public ProveedorService(IConfiguration conf)
        {
            this.configuration = conf;
            this.cadenaConexion = configuration.GetSection("cadenaConexion").Value ?? "";
            this.tablaNombre = "Proveedor";
        }
        public async Task<bool> Create(Proveedor proveedor)
        {
            try
            {
                var tablaClient = new TableClient(cadenaConexion, tablaNombre);
                await tablaClient.UpsertEntityAsync(proveedor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string partitionkey, string rowkey)
        {
            try
            {
                var tablaClient = new TableClient(cadenaConexion, tablaNombre);
                await tablaClient.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Proveedor> Get(string id)
        {
            try
            {
                var tablaClient = new TableClient(cadenaConexion, tablaNombre);
                var filtro = $"PartitionKey eq 'Proveedor' and RowKey eq '{id}'";
                await foreach (Proveedor proveedor in tablaClient.QueryAsync<Proveedor>(filter: filtro))
                {
                    return proveedor;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Proveedor>> GetAll()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            var tablaClient = new TableClient(cadenaConexion, tablaNombre);
            await foreach (Proveedor proveedor in tablaClient.QueryAsync<Proveedor>(filter: $"PartitionKey eq 'Proveedor'"))
            {
                proveedores.Add(proveedor);
            }
            return proveedores;
        }

        public async Task<bool> Update(Proveedor proveedor)
        {
            try
            {
                var tablaClient = new TableClient(cadenaConexion, tablaNombre);
                await tablaClient.UpdateEntityAsync(proveedor, proveedor.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
