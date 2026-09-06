using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface IOrdenImportacionService
    {
        Task<List<OrdenImportacion>> ObtenerTodasAsync();
        Task<List<OrdenImportacion>> ObtenerAbiertasAsync();
        Task<OrdenImportacion?> ObtenerPorIdAsync(int id);

        // Orden con importador, proveedor, moneda, productos, gastos y resultado oficial.
        Task<OrdenImportacion?> ObtenerCompletaAsync(int id);

        Task CrearAsync(OrdenImportacion orden);
        Task ActualizarAsync(OrdenImportacion orden);
        Task CerrarOrdenAsync(int id);
        Task CancelarAsync(int id);
        Task EliminarAsync(int id);
    }
}
