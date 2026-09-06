using ImportCostPro.Core.Entities;
using Microsoft.AspNetCore.Mvc;

public class ConfiguracionImpuestosController
    : Controller
{
    private readonly
        IConfiguracionImpuestosService _service;

    public ConfiguracionImpuestosController(
        IConfiguracionImpuestosService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var config =
            await _service.ObtenerAsync()
            ?? new ConfiguracionImpuestos();

        return View(config);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        ConfiguracionImpuestos config)
    {
        if (!ModelState.IsValid)
        {
            return View(config);
        }

        await _service.GuardarAsync(config);

        TempData["Exito"] =
            "Configuración guardada correctamente.";

        return RedirectToAction(nameof(Index));
    }
}