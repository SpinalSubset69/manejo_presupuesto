using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{

    public class TiposCuentasController : Controller
    {
        private readonly ITIposCuentasRepository _repo;
        private readonly IUsersService _service;
        
        public TiposCuentasController(ITIposCuentasRepository repo, IUsersService service)
        {
            _repo = repo;
            _service = service;
        }
        
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            else
            {
                //solo para testear
                tipoCuenta.UsuarioId = _service.GetUsuarioId();

                var exists = await _repo.Exists(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

                if (exists)
                {
                    ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El Nombre {tipoCuenta.Nombre} ya existe");
                    return View(tipoCuenta);
                }
                
                await _repo.Crear(tipoCuenta);
                return RedirectToAction("index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AccountTypeExists(string nombre)
        {
            var usuarioId = _service.GetUsuarioId();
            var accountExists = await _repo.Exists(nombre, usuarioId);
            return accountExists ? Json($"El nombre {nombre} ya existe") : Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuarioId = _service.GetUsuarioId();
            var tipoCuenta = await _repo.GetById(id, usuarioId);

            if (tipoCuenta is null) return RedirectToAction("NotFound", "Home");
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TipoCuenta tipoCuenta)
        {
            var usuarioId = _service.GetUsuarioId();
            var tipoCuentaExiste = await _repo.GetById(tipoCuenta.Id, usuarioId);

            if (tipoCuentaExiste is null) return RedirectToAction("NotFound", "Home");
            await _repo.Update(tipoCuenta);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> index()
        {
            var usuarioId = _service.GetUsuarioId();
            var tiposCuentas = await _repo.Get(usuarioId);

            return View(tiposCuentas);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = _service.GetUsuarioId();
            var tipoCuenta = await _repo.GetById(id, usuarioId);

            if (tipoCuenta is null) return RedirectToAction("NotFound", "Home");
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTipoCuenta(int id)
        {
            var usuarioId = _service.GetUsuarioId();
            var tipoCuenta = await _repo.GetById(id, usuarioId);

            if (tipoCuenta is null) return RedirectToAction("NotFound", "Home");
            await _repo.Delete(id);
            return RedirectToAction("index");
        }

    }
}