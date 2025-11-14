// Programa utilitário para gerar hashes BCrypt
// Compile: csc gerar_hashes.cs /r:BCrypt.Net-Next.dll
// Execute: mono gerar_hashes.exe

using System;
using BCrypt.Net;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Gerador de Hashes BCrypt ===\n");

        string[] senhas = { "admin123", "tecnico123", "user123" };

        foreach (var senha in senhas)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(senha);
            Console.WriteLine($"Senha: {senha}");
            Console.WriteLine($"Hash:  {hash}");
            Console.WriteLine();
        }

        Console.WriteLine("=== Script SQL para atualizar ===\n");
        
        foreach (var senha in senhas)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(senha);
            string email = senha.Replace("123", "@sistema.com");
            Console.WriteLine($"UPDATE usuarios SET senha = '{hash}' WHERE email LIKE '%{email.Split('@')[0]}%';");
        }
    }
}
