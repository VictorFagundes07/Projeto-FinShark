using BibliotecaMvc.Data;
using BibliotecaMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMvc.Controllers;

public class LivrosController : Controller
{
    private readonly BibliotecaContext _context;

    public LivrosController(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? busca, bool somenteDisponiveis = false)
    {
        var livros = _context.Livros.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            livros = livros.Where(l => l.Nome.Contains(busca) || l.Autor.Contains(busca) || l.Categoria.Contains(busca));
        }

        if (somenteDisponiveis)
        {
            livros = livros.Where(l => l.QuantidadeEstoque > 0);
        }

        ViewBag.Busca = busca;
        ViewBag.SomenteDisponiveis = somenteDisponiveis;
        return View(await livros.OrderBy(l => l.Nome).ToListAsync());
    }

    public IActionResult Create() => View(new Livro());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Livro livro)
    {
        if (!ModelState.IsValid)
        {
            return View(livro);
        }

        _context.Add(livro);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Livro cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        return livro is null ? NotFound() : View(livro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Livro livro)
    {
        if (id != livro.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(livro);
        }

        _context.Update(livro);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Livro atualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        return livro is null ? NotFound() : View(livro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro is not null)
        {
            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Livro excluído.";
        }

        return RedirectToAction(nameof(Index));
    }
}
