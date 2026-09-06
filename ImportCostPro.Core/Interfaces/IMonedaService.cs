using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IMonedaService
    {
        Task<List<Moneda>> ObtenerTodasAsync();

        Task<List<Moneda>> ObtenerActivasAsync();

        Task<Moneda?> ObtenerPorIdAsync(int id);

        Task CrearAsync(Moneda moneda);

        Task ActualizarAsync(Moneda moneda);

        Task EliminarAsync(int id);

        Task<bool> TieneRelacionesAsync(int id);
    }
}