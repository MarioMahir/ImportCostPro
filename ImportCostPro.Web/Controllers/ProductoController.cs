using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IPaisService _paisService;
        private readonly ICategoriaArancelariaService _categoriaService;

        public ProductoController(
            IProductoService productoService,
            IPaisService paisService,
            ICategoriaArancelariaService categoriaService)
        {
            _productoService = productoService;
            _paisService = paisService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodosAsync();

            return View(productos);
        }

        private async Task CargarCombos(ProductoViewModel vm)
        {
            vm.Paises = (await _paisService.ObtenerActivosAsync())
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                })
                .ToList();

            vm.Categorias = (await _categoriaService.ObtenerActivasAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Descripcion
                })
                .ToList();
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ProductoViewModel();

            await CargarCombos(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);

                return View(vm);
            }

            var producto = new Producto
            {
                Nombre = vm.Nombre,
                CodigoReferencia = vm.CodigoReferencia,
                PaisId = vm.PaisId,
                CategoriaArancelariaId = vm.CategoriaArancelariaId,
                PesoUnitario = vm.PesoUnitario,
                Largo = vm.Largo,
                Ancho = vm.Ancho,
                Alto = vm.Alto,
                UnidadMedida = vm.UnidadMedida,
                Descripcion = vm.Descripcion,
                Estado = vm.Estado
            };

            await _productoService.CrearAsync(producto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            var vm = new ProductoViewModel
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                CodigoReferencia = producto.CodigoReferencia,
                PaisId = producto.PaisId,
                CategoriaArancelariaId = producto.CategoriaArancelariaId,
                PesoUnitario = producto.PesoUnitario,
                Largo = producto.Largo,
                Ancho = producto.Ancho,
                Alto = producto.Alto,
                UnidadMedida = producto.UnidadMedida,
                Descripcion = producto.Descripcion,
                Estado = producto.Estado
            };

            await CargarCombos(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);

                return View(vm);
            }

            var producto = new Producto
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                CodigoReferencia = vm.CodigoReferencia,
                PaisId = vm.PaisId,
                CategoriaArancelariaId = vm.CategoriaArancelariaId,
                PesoUnitario = vm.PesoUnitario,
                Largo = vm.Largo,
                Ancho = vm.Ancho,
                Alto = vm.Alto,
                UnidadMedida = vm.UnidadMedida,
                Descripcion = vm.Descripcion,
                Estado = vm.Estado
            };

            await _productoService.ActualizarAsync(producto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productoService.EliminarAsync(id);
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