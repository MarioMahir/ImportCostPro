using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface ITasaCambioService
    {
        Task<List<TasaCambio>> ObtenerTodasAsync();

        Task<bool> ExisteDuplicadaAsync( int origenId, int destinoId, DateTime fecha, 
            int? excluirId = null);
        Task<TasaCambio?> ObtenerPorIdAsync(int id);

        Task CrearAsync(TasaCambio tasa);

        Task ActualizarAsync(TasaCambio tasa);

        Task EliminarAsync(int id);
    }
}