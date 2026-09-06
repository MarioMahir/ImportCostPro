using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Interfaces
{
    public interface ILandedCostService
    {
        // Ordenes en estado Abierta, de la mas reciente a la mas antigua.
        Task<List<OrdenImportacion>> ObtenerOrdenesCalculablesAsync();

        // Valida la orden y calcula el landed cost sin guardar nada.
        Task<LandedCostCalculoDto> CalcularAsync(int ordenId);

        // Recalcula, guarda el resultado oficial con los valores usados y pasa la orden a Calculada.
        Task<ResultadoLandedCost> GuardarCalculoOficialAsync(int ordenId);

        Task<ResultadoLandedCost?> ObtenerResultadoOficialAsync(int ordenId);
    }
}
