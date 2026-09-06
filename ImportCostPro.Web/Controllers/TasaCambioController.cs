using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class TasaCambioController : Controller
    {
        private readonly ITasaCambioService _tasaService;
        private readonly IMonedaService _monedaService;

        public TasaCambioController(
            ITasaCambioService tasaService,
            IMonedaService monedaService)
        {
            _tasaService = tasaService;
            _monedaService = monedaService;
        }

        private async Task CargarCombos(
            TasaCambioViewModel vm)
        {
            vm.Monedas =
                (await _monedaService.ObtenerActivasAsync())
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Descripcion
                })
                .ToList();
        }

        public async Task<IActionResult> Index()
        {
            var tasas =
                await _tasaService.ObtenerTodasAsync();

            return View(tasas);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new TasaCambioViewModel
            {
                FechaVigencia = DateTime.Today
            };

            await CargarCombos(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TasaCambioViewModel vm)
        {
            if (vm.MonedaOrigenId == vm.MonedaDestinoId)
            {
                ModelState.AddModelError(
                    "",
                    "La moneda origen no puede ser igual a la moneda destino.");
            }

            if (await _tasaService.ExisteDuplicadaAsync(
                vm.MonedaOrigenId,
                vm.MonedaDestinoId,
                vm.FechaVigencia))
            {
                ModelState.AddModelError(
                    "",
                    "Ya existe una tasa de cambio activa para esta combinación.");
            }

            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var tasa = new TasaCambio
            {
                MonedaOrigenId = vm.MonedaOrigenId,
                MonedaDestinoId = vm.MonedaDestinoId,
                ValorTasa = vm.ValorTasa,
                FechaVigencia = vm.FechaVigencia,
                Estado = vm.Estado
            };

            await _tasaService.CrearAsync(tasa);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tasa = await _tasaService.ObtenerPorIdAsync(id);

            if (tasa == null)
            {
                return NotFound();
            }

            var vm = new TasaCambioViewModel
            {
                Id = tasa.Id,
                MonedaOrigenId = tasa.MonedaOrigenId,
                MonedaDestinoId = tasa.MonedaDestinoId,
                ValorTasa = tasa.ValorTasa,
                FechaVigencia = tasa.FechaVigencia,
                Estado = tasa.Estado
            };

            await CargarCombos(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    TasaCambioViewModel vm)
        {
            if (vm.MonedaOrigenId == vm.MonedaDestinoId)
            {
                ModelState.AddModelError(
                    "",
                    "La moneda origen no puede ser igual a la moneda destino.");
            }

            if (await _tasaService.ExisteDuplicadaAsync(
                vm.MonedaOrigenId,
                vm.MonedaDestinoId,
                vm.FechaVigencia,
                vm.Id))
            {
                ModelState.AddModelError(
                    "",
                    "Ya existe una tasa de cambio activa para esta combinación.");
            }

            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var tasa = new TasaCambio
            {
                Id = vm.Id,
                MonedaOrigenId = vm.MonedaOrigenId,
                MonedaDestinoId = vm.MonedaDestinoId,
                ValorTasa = vm.ValorTasa,
                FechaVigencia = vm.FechaVigencia,
                Estado = vm.Estado
            };

            await _tasaService.ActualizarAsync(tasa);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tasa =
                await _tasaService.ObtenerPorIdAsync(id);

            if (tasa == null)
            {
                return NotFound();
            }

            return View(tasa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            try
            {
                await _tasaService.EliminarAsync(id);
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