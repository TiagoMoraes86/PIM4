#!/bin/bash

# Script para gerar hashes BCrypt das senhas
# PIM IV - Sistema de Chamados

echo "=== Gerador de Hashes BCrypt ==="
echo ""

cd /home/ubuntu/pim/SistemaChamados.Web

# Criar programa temporário
cat > /tmp/HashGenerator.cs << 'EOF'
using System;

class Program
{
    static void Main()
    {
        string[] senhas = { "admin123", "tecnico123", "user123" };
        string[] emails = { "admin@sistema.com", "tecnico@sistema.com", "user@sistema.com" };

        Console.WriteLine("-- Hashes BCrypt gerados:");
        Console.WriteLine();

        for (int i = 0; i < senhas.Length; i++)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(senhas[i]);
            Console.WriteLine($"-- Senha: {senhas[i]}");
            Console.WriteLine($"UPDATE usuarios SET senha = '{hash}' WHERE email = '{emails[i]}';");
            Console.WriteLine();
        }
    }
}
EOF

# Compilar e executar
export PATH="$PATH:/home/ubuntu/.dotnet"
dotnet script /tmp/HashGenerator.cs

# Limpar
rm /tmp/HashGenerator.cs

echo ""
echo "✅ Hashes gerados! Copie os comandos UPDATE acima e execute no PostgreSQL."
