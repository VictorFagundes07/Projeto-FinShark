using BibliotecaMvc.Data;
using BibliotecaMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMvc.Controllers;

public class EmprestimosController : Controller
{
    private readonly BibliotecaContext _context;

    public EmprestimosController(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? usuarioId)
    {
        var query = _context.Emprestimos.Include(e => e.Usuario).Include(e => e.Livro).AsQueryable();
        if (usuarioId.HasValue)
        {
            query = query.Where(e => e.UsuarioId == usuarioId.Value);
        }

        await CarregarUsuarios(usuarioId);
        return View(await query.OrderByDescending(e => e.DataEmprestimo).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        if (!UsuarioLogado())
        {
            TempData["Erro"] = "Faça login para registrar empréstimos.";
            return RedirectToAction("Login", "Account");
        }

        await CarregarCombos();
        return View(new Emprestimo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Emprestimo emprestimo)
    {
        if (!UsuarioLogado())
        {
            TempData["Erro"] = "Faça login para registrar empréstimos.";
            return RedirectToAction("Login", "Account");
        }

        var usuario = await _context.Usuarios.FindAsync(emprestimo.UsuarioId);
        var livro = await _context.Livros.FindAsync(emprestimo.LivroId);

        if (usuario is null)
        {
            ModelState.AddModelError(nameof(Emprestimo.UsuarioId), "Usuário não encontrado.");
        }

        if (livro is null)
        {
            ModelState.AddModelError(nameof(Emprestimo.LivroId), "Livro não encontrado.");
        }
        else
        {
            if (livro.QuantidadeEstoque <= 0)
            {
                ModelState.AddModelError(nameof(Emprestimo.LivroId), "Não há unidades disponíveis deste livro.");
            }

            if (usuario is not null && livro.FaixaEtariaPermitida >= 18 && usuario.CalcularIdade(DateTime.Today) < 18)
            {
                ModelState.AddModelError(nameof(Emprestimo.LivroId), "Livro 18+ só pode ser emprestado para usuário maior de idade.");
            }
        }

        if (emprestimo.DataPrevistaDevolucao.Date < emprestimo.DataEmprestimo.Date)
        {
            ModelState.AddModelError(nameof(Emprestimo.DataPrevistaDevolucao), "A devolução prevista deve ser depois do empréstimo.");
        }

        if (!ModelState.IsValid)
        {
            await CarregarCombos(emprestimo.UsuarioId, emprestimo.LivroId);
            return View(emprestimo);
        }

        emprestimo.Status = StatusEmprestimo.Emprestado;
        emprestimo.Multa = 0;
        livro!.QuantidadeEstoque -= 1;
        _context.Emprestimos.Add(emprestimo);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Empréstimo registrado e estoque atualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Devolver(int id)
    {
        var emprestimo = await _context.Emprestimos.Include(e => e.Usuario).Include(e => e.Livro).FirstOrDefaultAsync(e => e.Id == id);
        return emprestimo is null ? NotFound() : View(emprestimo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarDevolucao(int id)
    {
        var emprestimo = await _context.Emprestimos.Include(e => e.Livro).FirstOrDefaultAsync(e => e.Id == id);
        if (emprestimo is null)
        {
            return NotFound();
        }

        if (emprestimo.Status != StatusEmprestimo.Emprestado)
        {
            TempData["Erro"] = "Este empréstimo já foi finalizado.";
            return RedirectToAction(nameof(Index));
        }

        emprestimo.DataRealDevolucao = DateTime.Today;
        var diasAtraso = emprestimo.DiasAtraso(DateTime.Today);
        emprestimo.Multa = diasAtraso * 2m;
        emprestimo.Status = diasAtraso > 0 ? StatusEmprestimo.Atrasado : StatusEmprestimo.Devolvido;
        emprestimo.Livro!.QuantidadeEstoque += 1;

        await _context.SaveChangesAsync();
        TempData["Mensagem"] = $"Devolução registrada. Multa: {emprestimo.Multa:C}.";
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioLogado() => HttpContext.Session.GetInt32("UsuarioId").HasValue;

    private async Task CarregarCombos(int? usuarioId = null, int? livroId = null)
    {
        var usuarios = await _context.Usuarios.Where(u => u.Status == StatusUsuario.Ativo).OrderBy(u => u.NomeCompleto).ToListAsync();
        var livros = await _context.Livros.OrderBy(l => l.Nome).ToListAsync();
        ViewBag.UsuarioId = new SelectList(usuarios, "Id", "NomeCompleto", usuarioId);
        ViewBag.LivroId = new SelectList(livros, "Id", "Nome", livroId);
    }

    private async Task CarregarUsuarios(int? usuarioId = null)
    {
        var usuarios = await _context.Usuarios.OrderBy(u => u.NomeCompleto).ToListAsync();
        ViewBag.UsuarioId = new SelectList(usuarios, "Id", "NomeCompleto", usuarioId);
    }
}
