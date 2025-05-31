using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiTarefas.Models;

public class Tarefa
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ?Descricao { get; set; }

    public int Status { get; set; } = 0; // 0 = PENDENTE, 1 = CONCLUIDO

    public int UsuarioId { get; set; }

    [JsonIgnore]
    public Usuario? Usuario { get; set; }


}
