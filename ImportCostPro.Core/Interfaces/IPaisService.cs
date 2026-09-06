using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IPaisService
    {
        Task<List<Pais>> ObtenerTodosAsync();

        Task<List<Pais>> ObtenerActivosAsync();

        Task<Pais?> ObtenerPorIdAsync(int id);

        Task CrearAsync(Pais pais);

        Task ActualizarAsync(Pais pais);

        Task EliminarAsync(int id);
    }
}