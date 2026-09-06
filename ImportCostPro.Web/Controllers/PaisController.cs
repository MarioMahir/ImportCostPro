using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class PaisController : Controller
    {
        private readonly IPaisService _paisService;

        public PaisController(IPaisService paisService)
        {
            _paisService = paisService;
        }

        public async Task<IActionResult> Index()
        {
            var paises = await _paisService.ObtenerTodosAsync();

            return View(paises);
        }

        public IActionResult Create()
        {
            return View(new Pais());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return View(pais);
            }

            try
            {
                await _paisService.CrearAsync(pais);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(pais);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var pais = await _paisService.ObtenerPorIdAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            return View(pais);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return View(pais);
            }

            try
            {
                await _paisService.ActualizarAsync(pais);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(pais);
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            var pais = await _paisService.ObtenerPorIdAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            return View(pais);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _paisService.EliminarAsync(id);
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