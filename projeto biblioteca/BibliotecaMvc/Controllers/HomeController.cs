using BibliotecaMvc.Data;
using BibliotecaMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMvc.Controllers;

public class HomeController : Controller
{
    private readonly BibliotecaContext _context;

    public HomeController(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalUsuarios = await _context.Usuarios.CountAsync();
        ViewBag.TotalLivros = await _context.Livros.CountAsync();
        ViewBag.Disponiveis = await _context.Livros.SumAsync(l => l.QuantidadeEstoque);
        ViewBag.Emprestados = await _context.Emprestimos.CountAsync(e => e.Status == StatusEmprestimo.Emprestado);
        ViewBag.Atrasados = await _context.Emprestimos.CountAsync(e => e.Status == StatusEmprestimo.Emprestado && e.DataPrevistaDevolucao.Date < DateTime.Today);
        return View();
    }
}
