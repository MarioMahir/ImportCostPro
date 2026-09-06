using ImportCostPro.Core.Entities;

public interface IGastoImportacionService
{
    Task<List<GastoImportacion>> ObtenerTodosAsync();

    Task<GastoImportacion?> ObtenerPorIdAsync(int id);

    Task CrearAsync(GastoImportacion gasto);

    Task ActualizarAsync(GastoImportacion gasto);

    Task EliminarAsync(int id);
}