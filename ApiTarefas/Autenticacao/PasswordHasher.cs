using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ApiTarefas.Autenticacao;

public static class PasswordHasher
{
    public static string Hash(string senha)
    {
        // Gera um salt aleatório
        byte[] salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        // Deriva a chave (hash) da senha usando PBKDF2
        var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: senha,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Retorna salt + hash separados por ponto
        return $"{Convert.ToBase64String(salt)}.{hash}";
    }

    public static bool Verify(string senha, string hashComSalt)
    {
        var partes = hashComSalt.Split('.');
        if (partes.Length != 2) return false;

        var salt = Convert.FromBase64String(partes[0]);
        var hashEsperado = partes[1];

        // Deriva o hash da senha fornecida com o salt armazenado
        var hashAtual = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: senha,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Compara o hash gerado com o hash esperado
        return hashAtual == hashEsperado;
    }
}
