using System.ComponentModel.DataAnnotations;

namespace BibliotecaMvc.Models;

public class Emprestimo
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Data do empréstimo")]
    public DateTime DataEmprestimo { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Selecione um usuário.")]
    [Display(Name = "Usuário")]
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    [Required(ErrorMessage = "Selecione um livro.")]
    [Display(Name = "Livro")]
    public int LivroId { get; set; }
    public Livro? Livro { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Data prevista de devolução")]
    public DateTime DataPrevistaDevolucao { get; set; } = DateTime.Today.AddDays(7);

    [DataType(DataType.Date)]
    [Display(Name = "Data real de devolução")]
    public DateTime? DataRealDevolucao { get; set; }

    public decimal Multa { get; set; }

    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Emprestado;

    public int DiasAtraso(DateTime dataReferencia)
    {
        var dataBase = DataRealDevolucao?.Date ?? dataReferencia.Date;
        return Math.Max(0, (dataBase - DataPrevistaDevolucao.Date).Days);
    }
}
