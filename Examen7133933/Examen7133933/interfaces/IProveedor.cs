using Examen7133933.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen7133933.interfaces
{
    public interface IProveedor
    {
        public Task<bool> Create(Proveedor proveedor);
        public Task<bool> Update(Proveedor proveedor);
        public Task<bool> Delete(string partitionkey, string rowkey);
        public Task<Proveedor> Get(string id);
        public Task<List<Proveedor>> GetAll();
    }
}
