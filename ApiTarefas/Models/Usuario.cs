using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiTarefas.Models;

public class Usuario
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [JsonIgnore]
    public string? SenhaHash { get; set; }

    public ICollection<Tarefa>? Tarefas { get; set; }
}
