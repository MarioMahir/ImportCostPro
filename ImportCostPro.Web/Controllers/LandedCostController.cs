using ImportCostPro.Core.Interfaces;
using ImportCostPro.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class LandedCostController : Controller
    {
        private readonly ILandedCostService _landedCostService;
        private readonly IOrdenImportacionService _ordenService;

        public LandedCostController(
            ILandedCostService landedCostService,
            IOrdenImportacionService ordenService)
        {
            _landedCostService = landedCostService;
            _ordenService = ordenService;
        }

        // Select con las ordenes abiertas, la mas reciente seleccionada por defecto.
        public async Task<IActionResult> Index()
        {
            var vm = await CrearFormularioAsync(null);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calcular(LandedCostViewModel vm)
        {
            try
            {
                var calculo = await _landedCostService.CalcularAsync(vm.OrdenId);
                return View("Resultado", calculo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var formulario = await CrearFormularioAsync(vm.OrdenId);
                return View("Index", formulario);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(int ordenId)
        {
            try
            {
                await _landedCostService.GuardarCalculoOficialAsync(ordenId);
                TempData["Exito"] = $"El cálculo oficial de la orden #{ordenId} se guardó correctamente. La orden pasó a estado Calculada.";
                return RedirectToAction(nameof(Oficial), new { id = ordenId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Resultado oficial guardado de una orden calculada o cerrada.
        public async Task<IActionResult> Oficial(int id)
        {
            var resultado = await _landedCostService.ObtenerResultadoOficialAsync(id);
            if (resultado == null)
            {
                TempData["Error"] = $"La orden #{id} no tiene un cálculo oficial de landed cost guardado.";
                return RedirectToAction("Details", "OrdenImportacion", new { id });
            }

            return View(resultado);
        }

        private async Task<LandedCostViewModel> CrearFormularioAsync(int? ordenSeleccionada)
        {
            var ordenes = await _landedCostService.ObtenerOrdenesCalculablesAsync();
            var vm = new LandedCostViewModel
            {
                OrdenId = ordenSeleccionada ?? ordenes.FirstOrDefault()?.Id ?? 0,
                Ordenes = ordenes.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"Orden #{o.Id} · {o.Fecha:dd/MM/yyyy} · {o.Importador?.Nombre} · {o.Proveedor?.Nombre} · {o.Moneda?.CodigoISO}"
                }).ToList()
            };
            return vm;
        }
    }
}
