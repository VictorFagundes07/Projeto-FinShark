using BibliotecaMvc.Data;
using BibliotecaMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMvc.Controllers;

public class AccountController : Controller
{
    private readonly BibliotecaContext _context;

    public AccountController(BibliotecaContext context)
    {
        _context = context;
    }

    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha && u.Status == StatusUsuario.Ativo);

        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "E-mail, senha ou status inválido.");
            return View(model);
        }

        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
        HttpContext.Session.SetString("UsuarioNome", usuario.NomeCompleto);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
