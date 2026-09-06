using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.Services;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class ImportadorController : Controller
    {
        private readonly IImportadorService _importadorService;
        private readonly IPaisService _paisService;

        public ImportadorController(IImportadorService importadorService, IPaisService paisService)
        {
            _importadorService = importadorService;
            _paisService = paisService;
        }

        private async Task CargarPaises(ImportadorViewModel vm)
        {
            var paises =
                await _paisService.ObtenerActivosAsync();

            if (vm.PaisId > 0)
            {
                var paisActual =
                    await _paisService.ObtenerPorIdAsync(vm.PaisId);

                if (paisActual != null &&
                    !paises.Any(p => p.Id == paisActual.Id))
                {
                    paises.Add(paisActual);
                }
            }

            vm.Paises = paises
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                })
                .ToList();
        }

        public async Task<IActionResult> Index()
        {
            var importadores = await _importadorService.ObtenerTodosAsync();

            return View(importadores);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ImportadorViewModel();

            await CargarPaises(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportadorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarPaises(vm);

                return View(vm);
            }

            var importador = new Importador
            {
                Nombre = vm.Nombre,
                RNC = vm.RNC.Trim(),
                PaisId = vm.PaisId,
                Telefono = vm.Telefono,
                Email = vm.Email,
                Direccion = vm.Direccion,
                Estado = vm.Estado
            };

            await _importadorService.CrearAsync(importador);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var importador =
                await _importadorService.ObtenerPorIdAsync(id);

            if (importador == null)
            {
                return NotFound();
            }

            var vm = new ImportadorViewModel
            {
                Id = importador.Id,
                Nombre = importador.Nombre,
                RNC = importador.RNC,
                PaisId = importador.PaisId,
                Telefono = importador.Telefono,
                Email = importador.Email,
                Direccion = importador.Direccion,
                Estado = importador.Estado
            };

            await CargarPaises(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ImportadorViewModel vm)
        {
            var importador = new Importador
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                RNC = vm.RNC.Trim(),
                PaisId = vm.PaisId,
                Telefono = vm.Telefono,
                Email = vm.Email,
                Direccion = vm.Direccion,
                Estado = vm.Estado
            };

            if (!ModelState.IsValid)
            {
                await CargarPaises(vm);
                return View(vm);
            }

            await _importadorService.ActualizarAsync(importador);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var importador = await _importadorService.ObtenerPorIdAsync(id);

            if (importador == null)
            {
                return NotFound();
            }

            return View(importador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _importadorService.EliminarAsync(id);
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