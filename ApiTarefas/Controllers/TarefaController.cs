namespace ApiTarefas.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ApiTarefas.Contexto;
using ApiTarefas.DTO;
using ApiTarefas.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TarefaController : ControllerBase
{
    private readonly BancoDados _db;

    public TarefaController(BancoDados db)
    {
        _db = db;
    }

    private int ObterUsuarioId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet]
    public IActionResult Listar()
    {
        var id = ObterUsuarioId();
        var tarefas = _db.Tarefas.Where(t => t.UsuarioId == id).ToList();
        return Ok(tarefas);
    }

    [HttpPost]
    public IActionResult Criar(TarefaCreateDto dto)
    {
        var id = ObterUsuarioId();
        var tarefa = new Tarefa { Descricao = dto.Descricao, UsuarioId = id };
        _db.Tarefas.Add(tarefa);
        _db.SaveChanges();
        return Created("", tarefa);
    }

      [HttpPut("{id}")]
public IActionResult Atualizar(int id, TarefaCreateDto dto)
{
    var usuarioId = ObterUsuarioId();
    var tarefa = _db.Tarefas.FirstOrDefault(t => t.Id == id && t.UsuarioId == usuarioId);
    if (tarefa == null) return NotFound();

    tarefa.Descricao = dto.Descricao;
    _db.SaveChanges();
    return Ok(tarefa);
}

[HttpPatch("{id}/concluir")]
public IActionResult Concluir(int id)
{
    var usuarioId = ObterUsuarioId();
    var tarefa = _db.Tarefas.FirstOrDefault(t => t.Id == id && t.UsuarioId == usuarioId);
    if (tarefa == null) return NotFound();

    tarefa.Status = 1; // CONCLUÍDO
    _db.SaveChanges();
    return Ok(tarefa);
}

[HttpDelete("{id}")]
public IActionResult Excluir(int id)
{
    var usuarioId = ObterUsuarioId();
    var tarefa = _db.Tarefas.FirstOrDefault(t => t.Id == id && t.UsuarioId == usuarioId);
    if (tarefa == null) return NotFound();

    _db.Tarefas.Remove(tarefa);
    _db.SaveChanges();
    return NoContent();
}
    
}
