using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IDetalleOrdenImportacionService
    {
        Task<List<DetalleOrdenImportacion>>
            ObtenerTodosAsync();

        Task<List<DetalleOrdenImportacion>>
            ObtenerPorOrdenAsync(int ordenId);

        Task<DetalleOrdenImportacion?>
            ObtenerPorIdAsync(int id);

        Task CrearAsync(
            DetalleOrdenImportacion detalle);

        Task ActualizarAsync(
            DetalleOrdenImportacion detalle);

        Task EliminarAsync(int id);
    }
}