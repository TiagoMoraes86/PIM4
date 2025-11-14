# 🚀 Início Rápido - Sistema de Chamados

## Para Começar em 5 Minutos

### 1. Instalar Tudo Automaticamente

```bash
cd /home/ubuntu/pim
bash instalar_projeto.sh
```

O script irá:
- ✅ Verificar .NET e PostgreSQL
- ✅ Criar banco de dados
- ✅ Executar migrations
- ✅ Popular dados iniciais
- ✅ Gerar senhas
- ✅ Compilar projeto

### 2. Iniciar o Sistema

```bash
bash /home/ubuntu/pim/iniciar_sistema.sh
```

Ou manualmente:

```bash
cd /home/ubuntu/pim/SistemaChamados.Web
export PATH="$PATH:/home/ubuntu/.dotnet"
dotnet run
```

### 3. Acessar

Abra o navegador em:
- http://localhost:5000
- https://localhost:5001

### 4. Fazer Login

Use uma dessas credenciais:

| Email | Senha | Tipo |
|-------|-------|------|
| admin@sistema.com | admin123 | Admin |
| tecnico@sistema.com | tecnico123 | Técnico |
| user@sistema.com | user123 | Usuário |

---

## 📚 Documentação Completa

- **README.md** - Visão geral (10 páginas)
- **README_WEB.md** - Documentação técnica (30 páginas)
- **GUIA_TESTES.md** - 15 casos de teste (25 páginas)
- **ENTREGA_PROJETO.md** - Documento oficial (40 páginas)
- **RESUMO_EXECUTIVO.md** - Para gestores (10 páginas)
- **CHECKLIST_ENTREGA.md** - 184 itens verificados

**Total**: 115 páginas de documentação

---

## 🎯 Principais Funcionalidades

1. **Login Seguro** - BCrypt + Sessões
2. **Dashboard** - Gráficos interativos (Chart.js)
3. **Chamados** - Criar, listar, comentar
4. **FAQ Dinâmica** - IA com algoritmo próprio
5. **LGPD** - Logs automáticos de auditoria

---

## 🛠️ Comandos Úteis

```bash
# Iniciar sistema
bash /home/ubuntu/pim/iniciar_sistema.sh

# Parar sistema
bash /home/ubuntu/pim/parar_sistema.sh

# Gerar senhas BCrypt
bash /home/ubuntu/pim/gerar_hashes.sh

# Ver logs do banco
sudo -u postgres psql -d pim -c "SELECT * FROM logs_lgpd ORDER BY timestamp DESC LIMIT 10;"

# Ver estatísticas
sudo -u postgres psql -d pim -c "SELECT * FROM vw_dashboard_resumo;"
```

---

## ❓ Problemas?

### PostgreSQL não conecta
```bash
sudo systemctl start postgresql
```

### Erro de senha
```bash
# Edite appsettings.json e ajuste a senha
nano /home/ubuntu/pim/SistemaChamados.Web/appsettings.json
```

### Tabelas não existem
```bash
cd /home/ubuntu/pim/SistemaChamados.Web
psql -U postgres -d pim -f Data/Scripts/migrations.sql
```

---

## 📞 Mais Informações

Leia a documentação completa em:
- `/home/ubuntu/pim/README_WEB.md`
- `/home/ubuntu/pim/ENTREGA_PROJETO.md`

---

**Desenvolvido para PIM IV - UNIP 2025**
