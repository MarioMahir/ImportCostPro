using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Enums;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenImportacionController : Controller
    {
        private readonly IOrdenImportacionService _ordenService;
        private readonly IImportadorService _importadorService;
        private readonly IProveedorService _proveedorService;
        private readonly IMonedaService _monedaService;

        public OrdenImportacionController(
            IOrdenImportacionService ordenService,
            IImportadorService importadorService,
            IProveedorService proveedorService,
            IMonedaService monedaService)
        {
            _ordenService = ordenService;
            _importadorService = importadorService;
            _proveedorService = proveedorService;
            _monedaService = monedaService;
        }

        // Solo registros activos en los combos; los inactivos se conservan en el historico.
        private async Task CargarCombos(OrdenImportacionViewModel vm)
        {
            vm.Importadores = (await _importadorService.ObtenerTodosAsync())
                .Where(i => i.Estado || i.Id == vm.ImportadorId)
                .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Nombre })
                .ToList();

            vm.Proveedores = (await _proveedorService.ObtenerTodosAsync())
                .Where(p => p.Estado || p.Id == vm.ProveedorId)
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })
                .ToList();

            vm.Monedas = (await _monedaService.ObtenerActivasAsync())
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = $"{m.CodigoISO} - {m.Descripcion}" })
                .ToList();
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _ordenService.ObtenerTodasAsync();
            return View(ordenes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orden = await _ordenService.ObtenerCompletaAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new OrdenImportacionViewModel { Fecha = DateTime.Today };
            await CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenImportacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            try
            {
                var orden = new OrdenImportacion
                {
                    Fecha = vm.Fecha,
                    ImportadorId = vm.ImportadorId,
                    ProveedorId = vm.ProveedorId,
                    MonedaId = vm.MonedaId,
                    Estado = EstadoOrden.Abierta
                };

                await _ordenService.CrearAsync(orden);
                TempData["Exito"] = $"La orden #{orden.Id} se registró correctamente.";
                return RedirectToAction(nameof(Details), new { id = orden.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombos(vm);
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var orden = await _ordenService.ObtenerPorIdAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            if (orden.Estado != EstadoOrden.Abierta)
            {
                TempData["Error"] = $"No se puede editar la orden #{orden.Id} porque está en estado {orden.Estado}.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var vm = new OrdenImportacionViewModel
            {
                Id = orden.Id,
                Fecha = orden.Fecha,
                ImportadorId = orden.ImportadorId,
                ProveedorId = orden.ProveedorId,
                MonedaId = orden.MonedaId,
                Estado = orden.Estado
            };

            await CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrdenImportacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            try
            {
                var orden = new OrdenImportacion
                {
                    Id = vm.Id,
                    Fecha = vm.Fecha,
                    ImportadorId = vm.ImportadorId,
                    ProveedorId = vm.ProveedorId,
                    MonedaId = vm.MonedaId
                };

                await _ordenService.ActualizarAsync(orden);
                TempData["Exito"] = $"La orden #{orden.Id} se actualizó correctamente.";
                return RedirectToAction(nameof(Details), new { id = vm.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombos(vm);
                return View(vm);
            }
        }

        public async Task<IActionResult> Cerrar(int id)
        {
            var orden = await _ordenService.ObtenerCompletaAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarConfirmed(int id)
        {
            try
            {
                await _ordenService.CerrarOrdenAsync(id);
                TempData["Exito"] = $"La orden #{id} se cerró correctamente. A partir de ahora solo puede consultarse.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Cancelar(int id)
        {
            var orden = await _ordenService.ObtenerCompletaAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmed(int id)
        {
            try
            {
                await _ordenService.CancelarAsync(id);
                TempData["Exito"] = $"La orden #{id} fue cancelada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var orden = await _ordenService.ObtenerCompletaAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _ordenService.EliminarAsync(id);
                TempData["Exito"] = $"La orden #{id} se eliminó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}
