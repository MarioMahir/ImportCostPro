using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface ICategoriaArancelariaService
    {
        Task<List<CategoriaArancelaria>> ObtenerTodasAsync();

        Task<List<CategoriaArancelaria>> ObtenerActivasAsync();

        Task<bool> ExisteCodigoAsync( string codigo, int? excluirId = null);

        Task<bool> TieneProductosAsync(int categoriaId);

        Task<CategoriaArancelaria?> ObtenerPorIdAsync(int id);

        Task CrearAsync(CategoriaArancelaria categoria);

        Task ActualizarAsync(CategoriaArancelaria categoria);

        Task EliminarAsync(int id);
    }
}