using System.ComponentModel.DataAnnotations;

namespace BibliotecaMvc.Models;

public class Livro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do livro.")]
    [Display(Name = "Nome do livro")]
    [StringLength(160)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o autor.")]
    [StringLength(120)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    [Display(Name = "Quantidade em estoque")]
    public int QuantidadeEstoque { get; set; }

    [Required]
    [Range(0, 18, ErrorMessage = "Use 0, 10, 12, 14, 16 ou 18.")]
    [Display(Name = "Faixa etária permitida")]
    public int FaixaEtariaPermitida { get; set; }

    [Required(ErrorMessage = "Informe a categoria.")]
    [StringLength(80)]
    public string Categoria { get; set; } = string.Empty;

    [Required]
    [Range(1000, 2100, ErrorMessage = "Informe um ano válido.")]
    [Display(Name = "Ano de publicação")]
    public int AnoPublicacao { get; set; } = DateTime.Today.Year;

    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}
