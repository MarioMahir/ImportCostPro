using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class MonedaController : Controller
    {
        private readonly IMonedaService _monedaService;

        public MonedaController(IMonedaService monedaService)
        {
            _monedaService = monedaService;
        }

        public async Task<IActionResult> Index()
        {
            var monedas = await _monedaService.ObtenerTodasAsync();

            return View(monedas);
        }

        public IActionResult Create()
        {
            return View(new Moneda());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Moneda moneda)
        {
            if (!ModelState.IsValid)
            {
                return View(moneda);
            }

            await _monedaService.CrearAsync(moneda);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var moneda = await _monedaService.ObtenerPorIdAsync(id);

            if (moneda == null)
            {
                return NotFound();
            }

            return View(moneda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Moneda moneda)
        {
            if (!ModelState.IsValid)
            {
                return View(moneda);
            }

            await _monedaService.ActualizarAsync(moneda);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var moneda = await _monedaService.ObtenerPorIdAsync(id);

            if (moneda == null)
            {
                return NotFound();
            }

            return View(moneda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moneda =
                await _monedaService.ObtenerPorIdAsync(id);

            if (moneda == null)
            {
                return NotFound();
            }

            if (moneda.EsMonedaLocal)
            {
                TempData["Error"] =
                    "No se puede eliminar la moneda local.";

                return RedirectToAction(nameof(Index));
            }

            if (await _monedaService.TieneRelacionesAsync(id))
            {
                TempData["Error"] =
                    "No se puede eliminar esta moneda porque está asociada a otros registros del sistema.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _monedaService.EliminarAsync(id);
                TempData["Exito"] = "Registro eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}