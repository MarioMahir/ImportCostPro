using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Enums;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class GastoImportacionController : Controller
    {
        private readonly IGastoImportacionService _gastoService;
        private readonly IOrdenImportacionService _ordenService;
        private readonly IMonedaService _monedaService;

        public GastoImportacionController(
            IGastoImportacionService gastoService,
            IOrdenImportacionService ordenService,
            IMonedaService monedaService)
        {
            _gastoService = gastoService;
            _ordenService = ordenService;
            _monedaService = monedaService;
        }

        public async Task<IActionResult> Index()
        {
            var gastos = await _gastoService.ObtenerTodosAsync();
            return View(gastos);
        }

        private async Task CargarCombos(GastoImportacionViewModel vm)
        {
            vm.Ordenes = (await _ordenService.ObtenerAbiertasAsync())
                .Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"Orden #{o.Id} - {o.Fecha:dd/MM/yyyy} - {o.Importador?.Nombre}"
                })
                .ToList();

            vm.Monedas = (await _monedaService.ObtenerActivasAsync())
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.CodigoISO} - {m.Descripcion}"
                })
                .ToList();
        }

        public async Task<IActionResult> Create(int? ordenId)
        {
            var vm = new GastoImportacionViewModel { OrdenImportacionId = ordenId ?? 0 };
            await CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GastoImportacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var gastos = await _gastoService.ObtenerTodosAsync();

            if (vm.TipoGasto == TipoGasto.FleteInternacional &&
                gastos.Any(g => g.OrdenImportacionId == vm.OrdenImportacionId && g.TipoGasto == TipoGasto.FleteInternacional))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un gasto de flete internacional para esta orden.");
                await CargarCombos(vm);
                return View(vm);
            }

            if (vm.TipoGasto == TipoGasto.SeguroInternacional &&
                gastos.Any(g => g.OrdenImportacionId == vm.OrdenImportacionId && g.TipoGasto == TipoGasto.SeguroInternacional))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un gasto de seguro internacional para esta orden.");
                await CargarCombos(vm);
                return View(vm);
            }

            var gasto = new GastoImportacion
            {
                OrdenImportacionId = vm.OrdenImportacionId,
                TipoGasto = vm.TipoGasto,
                Monto = vm.Monto,
                MonedaId = vm.MonedaId,
                MetodoDistribucion = vm.MetodoDistribucion,
                Fecha = vm.Fecha
            };

            try
            {
                await _gastoService.CrearAsync(gasto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombos(vm);
                return View(vm);
            }

            TempData["Exito"] = "Gasto registrado en la orden.";
            return RedirectToAction("Details", "OrdenImportacion", new { id = vm.OrdenImportacionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var gasto = await _gastoService.ObtenerPorIdAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }

            var vm = new GastoImportacionViewModel
            {
                Id = gasto.Id,
                OrdenImportacionId = gasto.OrdenImportacionId,
                TipoGasto = gasto.TipoGasto,
                Monto = gasto.Monto,
                MonedaId = gasto.MonedaId,
                MetodoDistribucion = gasto.MetodoDistribucion,
                Fecha = gasto.Fecha
            };

            await CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GastoImportacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var gasto = await _gastoService.ObtenerPorIdAsync(vm.Id);
            if (gasto == null)
            {
                return NotFound();
            }

            gasto.TipoGasto = vm.TipoGasto;
            gasto.Monto = vm.Monto;
            gasto.MonedaId = vm.MonedaId;
            gasto.MetodoDistribucion = vm.MetodoDistribucion;
            gasto.Fecha = vm.Fecha;

            try
            {
                await _gastoService.ActualizarAsync(gasto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombos(vm);
                return View(vm);
            }

            TempData["Exito"] = "Gasto actualizado.";
            return RedirectToAction("Details", "OrdenImportacion", new { id = gasto.OrdenImportacionId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _gastoService.ObtenerPorIdAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _gastoService.ObtenerPorIdAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }

            try
            {
                await _gastoService.EliminarAsync(id);
                TempData["Exito"] = "Gasto eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", "OrdenImportacion", new { id = gasto.OrdenImportacionId });
        }
    }
}
