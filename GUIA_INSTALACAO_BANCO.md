# Guia de Instalação do Banco de Dados - PIM IV

## Sistema de Chamados com IA e LGPD

Este guia contém todas as instruções necessárias para configurar o banco de dados PostgreSQL do projeto PIM IV.

---

## Índice

1. [Pré-requisitos](#pré-requisitos)
2. [Criação do Banco de Dados](#criação-do-banco-de-dados)
3. [Execução do Script](#execução-do-script)
4. [Verificação da Instalação](#verificação-da-instalação)
5. [Usuários de Teste](#usuários-de-teste)
6. [Estrutura do Banco](#estrutura-do-banco)
7. [Requisitos do PIM IV Atendidos](#requisitos-do-pim-iv-atendidos)
8. [Solução de Problemas](#solução-de-problemas)

---

## Pré-requisitos

### Software Necessário

- **PostgreSQL 12 ou superior** instalado e rodando
- **pgAdmin 4** (ou outra ferramenta de administração PostgreSQL)
- **Acesso administrativo** ao PostgreSQL (usuário `postgres`)

### Verificar se o PostgreSQL está Rodando

**Windows:**
1. Pressione `Win + R`
2. Digite `services.msc` e pressione Enter
3. Procure por `postgresql` na lista
4. Verifique se o status está "Em execução"
5. Se não estiver, clique com botão direito → "Iniciar"

**Linux/Mac:**
```bash
sudo systemctl status postgresql
# ou
pg_isready
```

---

## Criação do Banco de Dados

### Opção 1: Usando pgAdmin

1. Abra o **pgAdmin 4**
2. Conecte-se ao servidor **PostgreSQL** (localhost)
3. Digite a senha do usuário `postgres`
4. No painel esquerdo, clique com botão direito em **"Databases"**
5. Selecione **"Create"** → **"Database..."**
6. Preencha:
   - **Database:** `pim`
   - **Owner:** `postgres`
   - **Encoding:** `UTF8`
7. Clique em **"Save"**

### Opção 2: Usando linha de comando (psql)

```bash
# Conectar ao PostgreSQL
psql -U postgres

# Criar o banco de dados
CREATE DATABASE pim WITH ENCODING 'UTF8';

# Sair
\q
```

---

## Execução do Script

### Passo 1: Localizar o Script

O arquivo do script está em:
```
pimAtualizado/SCRIPT_COMPLETO_PIM_IV.sql
```

### Passo 2: Executar no pgAdmin

1. No pgAdmin, expanda o servidor e localize o banco **pim**
2. Clique com botão direito no banco **pim**
3. Selecione **"Query Tool"** (ou pressione `Alt+Shift+Q`)
4. Na Query Tool, clique em **"File"** → **"Open"** (ou `Ctrl+O`)
5. Navegue até a pasta do projeto e selecione **`SCRIPT_COMPLETO_PIM_IV.sql`**
6. Clique no botão **"Execute"** (ícone de play ▶) ou pressione **F5**
7. Aguarde a execução (pode levar alguns segundos)

### Passo 3: Verificar Erros

- Verifique a aba **"Messages"** na parte inferior
- Se aparecer **"Query returned successfully"**, tudo está correto
- Se houver erros, leia a mensagem e corrija conforme necessário

### Opção Alternativa: Linha de Comando

```bash
# Executar o script via psql
psql -U postgres -d pim -f SCRIPT_COMPLETO_PIM_IV.sql
```

---

## Verificação da Instalação

### 1. Verificar Tabelas Criadas

Execute no pgAdmin (Query Tool):

```sql
SELECT tablename 
FROM pg_tables 
WHERE schemaname = 'public'
ORDER BY tablename;
```

**Resultado esperado (9 tabelas):**
- categorias
- chamados
- consentimentos
- estatisticas_diarias
- faqs
- interacoes
- logs_lgpd
- sugestoes_ia
- usuarios

### 2. Verificar Usuários Criados

```sql
SELECT email, nome, tipo 
FROM usuarios 
ORDER BY tipo, nome;
```

**Resultado esperado (3 usuários):**
| email | nome | tipo |
|-------|------|------|
| admin@sistema.com | Administrador | admin |
| tecnico@sistema.com | Técnico Suporte | tecnico |
| user@sistema.com | Usuário Teste | comum |

### 3. Verificar Categorias

```sql
SELECT nome, descricao 
FROM categorias 
ORDER BY ordem;
```

**Resultado esperado (8 categorias):**
- Hardware
- Software
- Rede
- E-mail
- Acesso
- Impressora
- Telefonia
- Outros

### 4. Verificar FAQs

```sql
SELECT COUNT(*) as total_faqs FROM faqs;
```

**Resultado esperado:** 6 FAQs

### 5. Verificar Views

```sql
SELECT viewname 
FROM pg_views 
WHERE schemaname = 'public'
ORDER BY viewname;
```

**Resultado esperado (7 views):**
- vw_chamados_por_categoria
- vw_chamados_por_mes
- vw_chamados_por_status
- vw_dashboard_resumo
- vw_desempenho_tecnicos
- vw_faqs_populares
- vw_relatorio_lgpd

---

## Usuários de Teste

Use estes usuários para testar o sistema:

### Administrador
- **Email:** admin@sistema.com
- **Senha:** admin123
- **Permissões:** Acesso completo ao sistema

### Técnico de Suporte
- **Email:** tecnico@sistema.com
- **Senha:** tecnico123
- **Permissões:** Atender e resolver chamados

### Usuário Comum
- **Email:** user@sistema.com
- **Senha:** user123
- **Permissões:** Abrir e acompanhar chamados

---

## Estrutura do Banco

### Tabelas Base (PIM III)

1. **usuarios** - Armazena usuários do sistema
   - Campos: email (PK), senha, nome, tipo, ativo, criado_em

2. **chamados** - Tickets de suporte técnico
   - Campos: id (PK), titulo, descricao, categoria, prioridade, data_abertura, data_fechamento, solicitante (FK), tecnico (FK), status, solucao

3. **interacoes** - Comentários nos chamados
   - Campos: id (PK), chamado_id (FK), data_hora, descricao, usuario (FK)

### Tabelas do PIM IV (IA e LGPD)

4. **faqs** - Perguntas frequentes dinâmicas (IA)
   - Campos: id (PK), pergunta, resposta, categoria, relevancia, palavras_chave, criado_em, atualizado_em

5. **sugestoes_ia** - Sugestões automáticas da IA
   - Campos: id (PK), chamado_id (FK), sugestao, confianca, aceita, feedback, criado_em

6. **logs_lgpd** - Logs de auditoria (LGPD)
   - Campos: id (PK), usuario_email (FK), acao, tabela, registro_id, dados_acessados, ip_address, user_agent, timestamp

7. **consentimentos** - Consentimentos dos usuários (LGPD)
   - Campos: id (PK), usuario_email (FK), tipo_consentimento, consentido, data_consentimento, ip_address, revogado, data_revogacao

### Tabelas Auxiliares

8. **categorias** - Categorias padronizadas
   - Campos: id (PK), nome, descricao, ativo, ordem

9. **estatisticas_diarias** - Cache de estatísticas
   - Campos: id (PK), data, total_chamados, chamados_abertos, chamados_em_analise, chamados_finalizados, tempo_medio_resolucao, atualizado_em

### Funções

- `atualizar_estatisticas_diarias()` - Atualiza cache de estatísticas
- `incrementar_relevancia_faq(faq_id)` - Incrementa contador de FAQ útil

### Triggers

- `trigger_atualizar_faq` - Atualiza timestamp ao modificar FAQ

### Views (Relatórios e Gráficos)

1. **vw_dashboard_resumo** - Resumo geral de chamados
2. **vw_chamados_por_status** - Distribuição por status (gráfico pizza)
3. **vw_chamados_por_mes** - Chamados por mês (gráfico barras)
4. **vw_faqs_populares** - Top 10 FAQs mais úteis
5. **vw_relatorio_lgpd** - Relatório de auditoria LGPD
6. **vw_chamados_por_categoria** - Distribuição por categoria
7. **vw_desempenho_tecnicos** - Desempenho dos técnicos

---

## Requisitos do PIM IV Atendidos

### ✅ Disciplinas Contempladas

| Requisito | Implementação no Banco |
|-----------|------------------------|
| **Orientação a Objetos** | Estrutura normalizada, relacionamentos FK, encapsulamento |
| **IA (Inteligência Artificial)** | Tabelas `faqs` e `sugestoes_ia` para sugestões automáticas |
| **LGPD** | Tabelas `logs_lgpd` e `consentimentos` para auditoria completa |
| **Relatórios e Gráficos** | 7 views para diferentes análises e visualizações |
| **PostgreSQL** | Banco de dados conforme especificação do PIM |
| **Gestão da Qualidade** | Índices, triggers, funções para integridade e performance |

### ✅ Funcionalidades Implementadas

- ✓ Gestão de usuários (comum, técnico, admin)
- ✓ Gestão de chamados (abertura, atribuição, resolução)
- ✓ FAQs dinâmicas com busca por palavras-chave
- ✓ Sugestões automáticas da IA
- ✓ Auditoria completa (LGPD)
- ✓ Consentimentos dos usuários
- ✓ Relatórios gerenciais
- ✓ Gráficos (status, categoria, mês, desempenho)
- ✓ Estatísticas agregadas para performance

---

## Solução de Problemas

### Erro: "database 'pim' does not exist"

**Solução:** Crie o banco de dados primeiro (veja seção [Criação do Banco de Dados](#criação-do-banco-de-dados))

### Erro: "permission denied"

**Solução:** Certifique-se de estar conectado como usuário `postgres` (superusuário)

### Erro: "relation already exists"

**Solução:** O script já tem comandos `DROP TABLE IF EXISTS`, então execute o script completo novamente. Ele vai limpar tudo e recriar.

### Erro: "syntax error near..."

**Solução:** Certifique-se de que o arquivo foi carregado completamente. Use "File → Open" no pgAdmin em vez de copiar e colar.

### PostgreSQL não está rodando

**Windows:**
1. `Win + R` → `services.msc`
2. Procure `postgresql`
3. Clique com botão direito → "Iniciar"

**Linux:**
```bash
sudo systemctl start postgresql
```

### Senha do PostgreSQL incorreta

Se você não lembra a senha do usuário `postgres`:

**Windows:**
1. Reinstale o PostgreSQL (vai pedir para definir nova senha)

**Linux:**
```bash
sudo -u postgres psql
ALTER USER postgres PASSWORD 'nova_senha';
```

---

## Próximos Passos

Após executar o script com sucesso:

1. ✅ Banco de dados configurado
2. ✅ Tabelas criadas
3. ✅ Dados de teste inseridos
4. ⏭️ Configurar string de conexão no código C#
5. ⏭️ Executar o projeto (Desktop/Web/Mobile)
6. ⏭️ Fazer login com um dos usuários de teste
7. ⏭️ Testar as funcionalidades

---

## String de Conexão

Use esta string de conexão no código C#:

```csharp
Server=localhost;Port=5432;Database=pim;User Id=postgres;Password=SUA_SENHA;
```

**Substitua `SUA_SENHA` pela senha do seu PostgreSQL!**

---

## Comandos Úteis

### Verificar tamanho do banco

```sql
SELECT pg_size_pretty(pg_database_size('pim'));
```

### Listar todas as tabelas com tamanho

```sql
SELECT 
    tablename,
    pg_size_pretty(pg_total_relation_size('public.'||tablename)) AS tamanho
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY pg_total_relation_size('public.'||tablename) DESC;
```

### Backup do banco

```bash
pg_dump -U postgres pim > backup_pim.sql
```

### Restaurar backup

```bash
psql -U postgres pim < backup_pim.sql
```

---

## Suporte

Se você encontrar problemas:

1. Verifique a seção [Solução de Problemas](#solução-de-problemas)
2. Leia as mensagens de erro com atenção
3. Verifique se o PostgreSQL está rodando
4. Confirme que está usando o usuário `postgres`
5. Verifique se a senha está correta

---

**Banco de dados pronto para uso!** 🎉

Desenvolvido para o PIM IV - UNIP  
Análise e Desenvolvimento de Sistemas  
2025/2
