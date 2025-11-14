# Guia de Instalação - Projeto Corrigido

## Pré-requisitos

- PostgreSQL 12 ou superior
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code (para desenvolvimento)

## Passo 1: Configurar o Banco de Dados

### 1.1 Criar o Banco de Dados

Abra o terminal do PostgreSQL (psql) ou use uma ferramenta como pgAdmin:

```sql
CREATE DATABASE pim;
```

### 1.2 Executar o Script de Migrations

Execute o script de migrations para criar todas as tabelas e inserir dados iniciais:

```bash
psql -U postgres -d pim -f SistemaChamados.Web/Data/Scripts/migrations.sql
```

**Ou**, se você já tem um banco criado e precisa apenas atualizar as senhas:

```bash
psql -U postgres -d pim -f atualizar_senhas.sql
```

### 1.3 Verificar a Conexão

As aplicações estão configuradas para conectar com:
- **Host:** localhost
- **Porta:** 5432
- **Banco:** pim
- **Usuário:** postgres
- **Senha:** 2004

Se suas credenciais forem diferentes, atualize:
- **WinForms:** `pimproj/DatabaseConnection.cs`
- **Web:** `SistemaChamados.Web/appsettings.json`

## Passo 2: Configurar e Executar a Aplicação Web

### 2.1 Restaurar Pacotes

```bash
cd SistemaChamados.Web
dotnet restore
```

### 2.2 Executar a Aplicação

```bash
dotnet run
```

A aplicação estará disponível em: `https://localhost:5001` ou `http://localhost:5000`

### 2.3 Testar o Login Web

Acesse a página de login e use uma das credenciais:

| Email | Senha | Tipo |
|-------|-------|------|
| admin@sistema.com | admin123 | Administrador |
| tecnico@sistema.com | tecnico123 | Técnico |
| user@sistema.com | user123 | Usuário Comum |

### 2.4 Testar Criação de Chamados

1. Faça login com qualquer usuário
2. Acesse "Novo Chamado"
3. Preencha os campos:
   - **Título:** Teste de chamado
   - **Descrição:** Descrição do problema
   - **Categoria:** Hardware, Software, Rede, etc.
   - **Prioridade:** Baixa, Média, Alta ou Urgente
4. Clique em "Criar Chamado"
5. Verifique se o chamado foi criado com sucesso

## Passo 3: Configurar e Executar a Aplicação WinForms

### 3.1 Abrir o Projeto

Abra o arquivo `pimproj.sln` no Visual Studio 2022.

### 3.2 Restaurar Pacotes NuGet

O Visual Studio deve restaurar automaticamente. Se não, clique com o botão direito na solução e selecione "Restore NuGet Packages".

### 3.3 Compilar o Projeto

Pressione `Ctrl+Shift+B` ou vá em "Build > Build Solution".

### 3.4 Executar a Aplicação

Pressione `F5` ou clique em "Start" para executar.

### 3.5 Testar o Login WinForms

Use as mesmas credenciais da aplicação Web:

- **Email:** admin@sistema.com
- **Senha:** admin123

Ou qualquer outra combinação válida.

## Passo 4: Verificar as Correções

### ✅ Verificação 1: Login WinForms

- [ ] O login com `admin@sistema.com` e senha `admin123` funciona
- [ ] O login com credenciais inválidas mostra mensagem de erro
- [ ] Após login bem-sucedido, a tela principal é exibida

### ✅ Verificação 2: Login Web

- [ ] O login na aplicação Web funciona
- [ ] Após login, o usuário é redirecionado para o Dashboard
- [ ] A sessão do usuário é mantida ao navegar pelas páginas

### ✅ Verificação 3: Criação de Chamados Web

- [ ] É possível criar um novo chamado
- [ ] O chamado aparece na lista "Meus Chamados"
- [ ] Os detalhes do chamado podem ser visualizados
- [ ] A data de abertura é exibida corretamente

## Solução de Problemas

### Problema: "Erro ao autenticar usuário"

**Causa:** As senhas no banco não estão com hash BCrypt correto.

**Solução:**
```bash
psql -U postgres -d pim -f atualizar_senhas.sql
```

### Problema: "Column 'data_criacao' does not exist"

**Causa:** Código desatualizado tentando usar coluna antiga.

**Solução:** Certifique-se de que você fez o pull das últimas alterações:
```bash
git pull origin master
```

### Problema: "Connection refused" ao conectar no banco

**Causa:** PostgreSQL não está rodando ou as credenciais estão incorretas.

**Solução:**
1. Verifique se o PostgreSQL está rodando:
   ```bash
   sudo systemctl status postgresql
   ```
2. Verifique as credenciais nos arquivos de configuração

### Problema: Pacote BCrypt.Net não encontrado no WinForms

**Causa:** Pacotes NuGet não foram restaurados.

**Solução:**
```bash
cd pimproj
dotnet restore
```

## Estrutura de Senhas

Todas as senhas agora usam **BCrypt** para segurança. Os hashes são gerados automaticamente:

- Ao criar um usuário no WinForms: `BCrypt.Net.BCrypt.HashPassword(senha)`
- Ao criar um usuário no Web: `BCrypt.Net.BCrypt.HashPassword(senha)`
- Ao autenticar: `BCrypt.Net.BCrypt.Verify(senhaDigitada, hashArmazenado)`

## Arquivos de Referência

- **RELATORIO_CORRECOES.md:** Documentação completa das correções
- **ALTERACOES_RESUMO.md:** Resumo das alterações realizadas
- **atualizar_senhas.sql:** Script para atualizar senhas em banco existente
- **gerar_hashes_bcrypt.py:** Script Python para gerar novos hashes

## Suporte

Se encontrar problemas adicionais, verifique:
1. Os logs da aplicação
2. Os logs do PostgreSQL
3. As mensagens de erro no console

Para gerar novos hashes de senha, use o script Python:
```bash
python3 gerar_hashes_bcrypt.py
```
