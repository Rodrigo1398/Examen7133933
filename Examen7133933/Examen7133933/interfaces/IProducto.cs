using Examen7133933.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen7133933.interfaces
{
    public interface IProducto
    {
        public Task<bool> Create(Producto producto);
        public Task<bool> Update(Producto producto);
        public Task<bool> Delete(string partitionkey, string rowkey);
        public Task<List<Producto>> GetAll();
        public Task<Producto> Get(string id);
    }
}
