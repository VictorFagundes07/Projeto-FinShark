using System.ComponentModel.DataAnnotations;

namespace BibliotecaMvc.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome completo.")]
    [Display(Name = "Nome completo")]
    [StringLength(120)]
    public string NomeCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a data de nascimento.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de nascimento")]
    public DateTime DataNascimento { get; set; } = DateTime.Today.AddYears(-18);

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    [DataType(DataType.Password)]
    [StringLength(80, MinimumLength = 4, ErrorMessage = "A senha deve ter pelo menos 4 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [Required]
    public StatusUsuario Status { get; set; } = StatusUsuario.Ativo;

    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

    public int Idade => CalcularIdade(DateTime.Today);

    public int CalcularIdade(DateTime dataReferencia)
    {
        var idade = dataReferencia.Year - DataNascimento.Year;
        if (DataNascimento.Date > dataReferencia.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }
}
