namespace ApiTarefas.Controllers;
using Microsoft.AspNetCore.Mvc;
using ApiTarefas.Models;
using ApiTarefas.DTO;
using ApiTarefas.Contexto;
using ApiTarefas.Autenticacao;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly BancoDados _db;
    private readonly TokenService _tokenService;

    public UsuarioController(BancoDados db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    [HttpPost("registrar")]
    public IActionResult Registrar(UsuarioRegisterDto dto)
    {
        if (_db.Usuarios.Any(u => u.Email == dto.Email))
            return BadRequest("E-mail já cadastrado.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = PasswordHasher.Hash(dto.Senha)
        };

        _db.Usuarios.Add(usuario);
        _db.SaveChanges();

        return Ok("Usuário registrado com sucesso.");
    }

    [HttpPost("login")]
    public IActionResult Login(UsuarioLoginDTO dto)
    {
        var usuario = _db.Usuarios.FirstOrDefault(u => u.Email == dto.Email);
        if (usuario == null || !PasswordHasher.Verify(dto.Senha, usuario.SenhaHash))
            return Unauthorized("Credenciais inválidas.");

        var token = _tokenService.GerarToken(usuario);
        return Ok(new { token });
    }
}
