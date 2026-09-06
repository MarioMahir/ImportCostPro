using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class CategoriaArancelariaController : Controller
    {
        private readonly ICategoriaArancelariaService _categoriaService;

        public CategoriaArancelariaController(
            ICategoriaArancelariaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();

            return View(categorias);
        }

        public IActionResult Create()
        {
            return View(new CategoriaArancelaria());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    CategoriaArancelaria categoria)
        {
            categoria.CodigoArancelario =
                categoria.CodigoArancelario.Trim();

            if (await _categoriaService
                .ExisteCodigoAsync(categoria.CodigoArancelario))
            {
                ModelState.AddModelError(
                    "CodigoArancelario",
                    "Ya existe una categoría arancelaria registrada con este código.");
            }

            if (categoria.AplicaImpuestoSelectivo &&
                categoria.PorcentajeImpuestoSelectivo <= 0)
            {
                ModelState.AddModelError(
                    "PorcentajeImpuestoSelectivo",
                    "Debe ser mayor que 0.");
            }

            if (!categoria.AplicaImpuestoSelectivo)
            {
                categoria.PorcentajeImpuestoSelectivo = 0;
            }

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            await _categoriaService.CrearAsync(categoria);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoria =
                await _categoriaService.ObtenerPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    CategoriaArancelaria categoria)
        {
            categoria.CodigoArancelario =
                categoria.CodigoArancelario.Trim();

            if (await _categoriaService
                .ExisteCodigoAsync(
                    categoria.CodigoArancelario,
                    categoria.Id))
            {
                ModelState.AddModelError(
                    "CodigoArancelario",
                    "Ya existe una categoría arancelaria registrada con este código.");
            }

            if (categoria.AplicaImpuestoSelectivo &&
                categoria.PorcentajeImpuestoSelectivo <= 0)
            {
                ModelState.AddModelError(
                    "PorcentajeImpuestoSelectivo",
                    "Debe ser mayor que 0.");
            }

            if (!categoria.AplicaImpuestoSelectivo)
            {
                categoria.PorcentajeImpuestoSelectivo = 0;
            }

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var original =
                await _categoriaService.ObtenerPorIdAsync(categoria.Id);

            if (original == null)
            {
                return NotFound();
            }

            bool tieneProductos =
                await _categoriaService
                .TieneProductosAsync(categoria.Id);

            if (tieneProductos)
            {
                categoria.CodigoArancelario =
                    original.CodigoArancelario;

                categoria.PorcentajeArancel =
                    original.PorcentajeArancel;

                categoria.AplicaITBIS =
                    original.AplicaITBIS;

                categoria.AplicaImpuestoSelectivo =
                    original.AplicaImpuestoSelectivo;

                categoria.PorcentajeImpuestoSelectivo =
                    original.PorcentajeImpuestoSelectivo;
            }

            await _categoriaService.ActualizarAsync(categoria);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoria =
                await _categoriaService.ObtenerPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool tieneProductos =
                await _categoriaService
                .TieneProductosAsync(id);

            if (tieneProductos)
            {
                TempData["Error"] =
                    "No se puede eliminar esta categoría arancelaria porque está asociada a productos registrados.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _categoriaService.EliminarAsync(id);
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