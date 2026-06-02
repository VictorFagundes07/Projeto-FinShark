using BibliotecaMvc.Data;
using BibliotecaMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMvc.Controllers;

public class UsuariosController : Controller
{
    private readonly BibliotecaContext _context;

    public UsuariosController(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Usuarios.OrderBy(u => u.NomeCompleto).ToListAsync());
    }

    public IActionResult Create() => View(new Usuario());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        await ValidarEmailUnico(usuario);
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        _context.Add(usuario);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Usuário cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        return usuario is null ? NotFound() : View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return NotFound();
        }

        await ValidarEmailUnico(usuario);
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        _context.Update(usuario);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Usuário atualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        return usuario is null ? NotFound() : View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is not null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Usuário excluído.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarEmailUnico(Usuario usuario)
    {
        var existe = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.Id != usuario.Id);
        if (existe)
        {
            ModelState.AddModelError(nameof(Usuario.Email), "Já existe um usuário com este e-mail.");
        }
    }
}
