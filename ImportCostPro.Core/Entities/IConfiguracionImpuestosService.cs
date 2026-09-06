using ImportCostPro.Core.Entities;

public interface IConfiguracionImpuestosService
{
    Task<ConfiguracionImpuestos?> ObtenerAsync();

    Task GuardarAsync(
        ConfiguracionImpuestos configuracion);
}