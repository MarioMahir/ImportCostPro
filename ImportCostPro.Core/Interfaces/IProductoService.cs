using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodosAsync();

        Task<List<Producto>> ObtenerActivosAsync();

        Task<Producto?> ObtenerPorIdAsync(int id);

        Task CrearAsync(Producto producto);

        Task ActualizarAsync(Producto producto);

        Task EliminarAsync(int id);
    }
}