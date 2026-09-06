using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> ObtenerTodosAsync();

        Task<Proveedor?> ObtenerPorIdAsync(int id);

        Task CrearAsync(Proveedor proveedor);

        Task ActualizarAsync(Proveedor proveedor);

        Task EliminarAsync(int id);
    }
}