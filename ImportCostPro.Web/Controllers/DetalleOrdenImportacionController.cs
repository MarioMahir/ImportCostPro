using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class DetalleOrdenImportacionController : Controller
    {
        private readonly IDetalleOrdenImportacionService _detalleService;
        private readonly IOrdenImportacionService _ordenService;
        private readonly IProductoService _productoService;

        public DetalleOrdenImportacionController(
            IDetalleOrdenImportacionService detalleService,
            IOrdenImportacionService ordenService,
            IProductoService productoService)
        {
            _detalleService = detalleService;
            _ordenService = ordenService;
            _productoService = productoService;
        }

        // Solo ordenes abiertas y productos activos; los inactivos permanecen en el historico.
        private async Task CargarCombos(DetalleOrdenImportacionViewModel vm)
        {
            vm.Ordenes = (await _ordenService.ObtenerAbiertasAsync())
                .Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"Orden #{o.Id} - {o.Fecha:dd/MM/yyyy} - {o.Importador?.Nombre}"
                })
                .ToList();

            vm.Productos = (await _productoService.ObtenerTodosAsync())
                .Where(p => p.Estado || p.Id == vm.ProductoId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.CodigoReferencia} - {p.Nombre}"
                })
                .ToList();
        }

        public async Task<IActionResult> Index()
        {
            var detalles = await _detalleService.ObtenerTodosAsync();
            return View(detalles);
        }

        public async Task<IActionResult> Create(int? ordenId)
        {
            var vm = new DetalleOrdenImportacionViewModel { OrdenImportacionId = ordenId ?? 0 };
            await CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetalleOrdenImportacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var detalle = new DetalleOrdenImportacion
            {
                OrdenImportacionId = vm.OrdenImportacionId,
                ProductoId = vm.ProductoId,
                Cantidad = vm.Cantidad,
                PrecioUnitario = vm.PrecioUnitario,
                MargenDeseado = vm.MargenDeseado
            };

            try
            {
                await _detalleService.CrearAsync(detalle);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombos(vm);
                return View(vm);
            }

            TempData["Exito"] = "Producto agregado a la orden.";
            return RedirectToAction("Details", "OrdenImportacion", new { id = vm.OrdenImportacionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var detalle = await _detalleService.ObtenerPorIdAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }

            var vm = new DetalleOrdenImportacionViewModel
            {
                Id = detalle.Id,
                OrdenImportacionId = detalle.OrdenImportacionId,
                ProductoId = detalle.ProductoId,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                MargenDeseado = detalle.MargenDeseado
            };

            await CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DetalleOrdenImportacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var detalle = new DetalleOrdenImportacion
            {
                Id = vm.Id,
                OrdenImportacionId = vm.OrdenImportacionId,
                ProductoId = vm.ProductoId,
                Cantidad = vm.Cantidad,
                PrecioUnitario = vm.PrecioUnitario,
                MargenDeseado = vm.MargenDeseado
            };

            try
            {
                await _detalleService.ActualizarAsync(detalle);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarCombos(vm);
                return View(vm);
            }

            TempData["Exito"] = "Producto de la orden actualizado.";
            return RedirectToAction("Details", "OrdenImportacion", new { id = vm.OrdenImportacionId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var detalle = await _detalleService.ObtenerPorIdAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }

            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle = await _detalleService.ObtenerPorIdAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }

            try
            {
                await _detalleService.EliminarAsync(id);
                TempData["Exito"] = "Producto eliminado de la orden.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", "OrdenImportacion", new { id = detalle.OrdenImportacionId });
        }
    }
}
