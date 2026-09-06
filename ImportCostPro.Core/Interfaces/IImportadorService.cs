using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IImportadorService
    {
        Task<List<Importador>> ObtenerTodosAsync();

        Task<Importador?> ObtenerPorIdAsync(int id);

        Task CrearAsync(Importador importador);

        Task ActualizarAsync(Importador importador);

        Task EliminarAsync(int id);
    }
}