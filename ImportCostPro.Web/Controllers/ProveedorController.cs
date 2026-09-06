using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;
        private readonly IPaisService _paisService;
        private readonly IMonedaService _monedaService;

        public ProveedorController(
            IProveedorService proveedorService,
            IPaisService paisService,
            IMonedaService monedaService)
        {
            _proveedorService = proveedorService;
            _paisService = paisService;
            _monedaService = monedaService;
        }

        private async Task CargarCombos(ProveedorViewModel vm)
        {
            vm.Paises =
                (await _paisService.ObtenerActivosAsync())
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                })
                .ToList();

            vm.Monedas =
                (await _monedaService.ObtenerActivasAsync())
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.CodigoISO} - {m.Descripcion}"
                })
                .ToList();
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.ObtenerTodosAsync();

            return View(proveedores);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ProveedorViewModel();

            await CargarCombos(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    ProveedorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var proveedor = new Proveedor
            {
                Nombre = vm.Nombre,
                PaisId = vm.PaisId,
                Email = vm.Email,
                Telefono = vm.Telefono,
                MonedaId = vm.MonedaId,
                Estado = vm.Estado
            };

            await _proveedorService.CrearAsync(proveedor);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var proveedor =
                await _proveedorService.ObtenerPorIdAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            var vm = new ProveedorViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                PaisId = proveedor.PaisId,
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                MonedaId = proveedor.MonedaId,
                Estado = proveedor.Estado
            };

            await CargarCombos(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    ProveedorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var proveedor = new Proveedor
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                PaisId = vm.PaisId,
                Email = vm.Email,
                Telefono = vm.Telefono,
                MonedaId = vm.MonedaId,
                Estado = vm.Estado
            };

            await _proveedorService.ActualizarAsync(proveedor);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorIdAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _proveedorService.EliminarAsync(id);
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