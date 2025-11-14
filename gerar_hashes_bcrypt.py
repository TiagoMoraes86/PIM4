import subprocess
import sys

# Instalar bcrypt se necessário
subprocess.run([sys.executable, "-m", "pip", "install", "bcrypt", "-q"])

import bcrypt

senhas = {
    "admin123": "",
    "tecnico123": "",
    "user123": ""
}

print("Gerando hashes BCrypt para as senhas padrão:\n")

for senha, _ in senhas.items():
    hash_senha = bcrypt.hashpw(senha.encode('utf-8'), bcrypt.gensalt()).decode('utf-8')
    senhas[senha] = hash_senha
    print(f"Senha: {senha}")
    print(f"Hash:  {hash_senha}\n")

print("\n--- SQL para atualizar usuários ---\n")
print(f"UPDATE usuarios SET senha = '{senhas['admin123']}' WHERE email = 'admin@sistema.com';")
print(f"UPDATE usuarios SET senha = '{senhas['tecnico123']}' WHERE email = 'tecnico@sistema.com';")
print(f"UPDATE usuarios SET senha = '{senhas['user123']}' WHERE email = 'user@sistema.com';")
