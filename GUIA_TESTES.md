# Guia de Testes - Sistema de Chamados PIM IV

## Preparação do Ambiente

### 1. Instalar PostgreSQL (se ainda não tiver)

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install postgresql postgresql-contrib

# Iniciar serviço
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

### 2. Criar Banco de Dados

```bash
# Conectar como postgres
sudo -u postgres psql

# Criar banco
CREATE DATABASE pim;

# Sair
\q
```

### 3. Executar Migrations

```bash
cd /home/ubuntu/pim/SistemaChamados.Web

# Executar script de criação de tabelas
psql -U postgres -d pim -f Data/Scripts/migrations.sql

# Executar script de dados iniciais
psql -U postgres -d pim -f Data/Scripts/seed_data.sql
```

### 4. Gerar Hashes de Senha

As senhas no seed_data.sql são exemplos. Você precisa gerar hashes reais:

```bash
cd /home/ubuntu/pim/SistemaChamados.Web

# Criar programa para gerar hashes
cat > /tmp/gerador.cs << 'EOF'
using System;

Console.WriteLine("Gerando hashes BCrypt...\n");

var senhas = new[] { 
    ("admin@sistema.com", "admin123"),
    ("tecnico@sistema.com", "tecnico123"),
    ("user@sistema.com", "user123")
};

foreach (var (email, senha) in senhas)
{
    var hash = BCrypt.Net.BCrypt.HashPassword(senha);
    Console.WriteLine($"UPDATE usuarios SET senha = '{hash}' WHERE email = '{email}';");
}
EOF

# Executar
dotnet script /tmp/gerador.cs

# Copiar os comandos UPDATE gerados e executar no PostgreSQL
```

### 5. Atualizar Senhas no Banco

```bash
# Conectar ao banco
psql -U postgres -d pim

# Colar os comandos UPDATE gerados acima
# Exemplo:
# UPDATE usuarios SET senha = '$2a$11$...' WHERE email = 'admin@sistema.com';
```

### 6. Compilar e Executar

```bash
cd /home/ubuntu/pim/SistemaChamados.Web

# Restaurar pacotes
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run
```

A aplicação estará disponível em:
- **HTTPS**: https://localhost:5001
- **HTTP**: http://localhost:5000

---

## Casos de Teste

### Teste 1: Login

**Objetivo**: Verificar autenticação de usuários

**Passos**:
1. Acesse http://localhost:5000
2. Será redirecionado para /Login
3. Tente fazer login com credenciais inválidas
   - Email: `teste@teste.com`
   - Senha: `errada`
   - **Esperado**: Mensagem de erro "Usuário ou senha inválidos"

4. Faça login com usuário válido:
   - Email: `user@sistema.com`
   - Senha: `user123`
   - **Esperado**: Redirecionamento para Dashboard

**Resultado Esperado**: ✅ Login bem-sucedido, sessão criada

---

### Teste 2: Dashboard

**Objetivo**: Verificar exibição de estatísticas e gráficos

**Pré-requisito**: Estar logado

**Passos**:
1. Após login, você deve estar no Dashboard
2. Verifique os cards de estatísticas:
   - Chamados Abertos
   - Em Análise
   - Finalizados (Mês)
   - Tempo Médio

3. Verifique os gráficos:
   - Gráfico de pizza: Chamados por Status
   - Gráfico de barras: Chamados por Mês

**Resultado Esperado**: 
- ✅ Cards exibem números corretos
- ✅ Gráficos são renderizados
- ✅ Dados correspondem aos do banco

---

### Teste 3: Criar Novo Chamado

**Objetivo**: Testar criação de chamado e FAQ dinâmica

**Pré-requisito**: Estar logado

**Passos**:
1. Clique em "Abrir Novo Chamado" no menu lateral
2. Preencha o formulário:
   - **Título**: "Computador não liga"
   - **Descrição**: "Meu computador não está ligando. Já verifiquei o cabo de energia."
   - **Categoria**: Hardware
   - **Prioridade**: Alta

3. **Teste FAQ Dinâmica**:
   - Ao digitar a descrição, observe o painel à direita
   - **Esperado**: Aparecem sugestões de FAQs relacionadas
   - Deve mostrar a FAQ "Meu computador não liga, o que fazer?"

4. Clique em "Registrar Chamado"

**Resultado Esperado**:
- ✅ Chamado criado com sucesso
- ✅ Redirecionado para página de detalhes
- ✅ FAQ dinâmica funcionou
- ✅ Mensagem de sucesso exibida

---

### Teste 4: Listar Meus Chamados

**Objetivo**: Verificar listagem de chamados do usuário

**Pré-requisito**: Ter criado pelo menos um chamado

**Passos**:
1. Clique em "Meus Chamados" no menu lateral
2. Verifique a tabela de chamados
3. Observe as colunas:
   - ID
   - Título
   - Categoria
   - Status (com badge colorido)
   - Prioridade (com badge colorido)
   - Data de Abertura
   - Botão "Ver Detalhes"

**Resultado Esperado**:
- ✅ Todos os chamados do usuário são exibidos
- ✅ Badges de status e prioridade com cores corretas
- ✅ Datas formatadas corretamente

---

### Teste 5: Ver Detalhes do Chamado

**Objetivo**: Verificar visualização completa e timeline

**Pré-requisito**: Ter pelo menos um chamado

**Passos**:
1. Em "Meus Chamados", clique em "Ver Detalhes" de um chamado
2. Verifique as informações exibidas:
   - Título e descrição
   - Status atual
   - Categoria e prioridade
   - Solicitante e técnico
   - Datas de abertura/fechamento

3. Verifique a **Timeline de Interações**:
   - Deve mostrar histórico cronológico
   - Interações do sistema (cinza)
   - Comentários de usuários (azul)

**Resultado Esperado**:
- ✅ Todas as informações corretas
- ✅ Timeline ordenada cronologicamente
- ✅ Diferenciação visual entre sistema e usuário

---

### Teste 6: Adicionar Comentário

**Objetivo**: Testar adição de interações

**Pré-requisito**: Estar na página de detalhes de um chamado não finalizado

**Passos**:
1. Na página de detalhes, role até o final
2. No campo "Adicionar Comentário", digite:
   - "Já tentei reiniciar o computador mas o problema persiste."
3. Clique em "Adicionar Comentário"

**Resultado Esperado**:
- ✅ Comentário adicionado à timeline
- ✅ Página recarregada com novo comentário
- ✅ Mensagem de sucesso
- ✅ Log LGPD registrado

---

### Teste 7: Busca de FAQs via API

**Objetivo**: Testar endpoint da API de FAQs

**Passos**:
1. Abra um terminal ou Postman
2. Faça uma requisição GET:

```bash
curl "http://localhost:5000/api/faqs/buscar?descricao=computador%20nao%20liga"
```

3. Verifique a resposta JSON

**Resultado Esperado**:
```json
[
  {
    "id": 2,
    "pergunta": "Meu computador não liga, o que fazer?",
    "resposta": "Verifique os seguintes itens...",
    "categoria": "Hardware",
    "relevancia": 0,
    "palavrasChave": ["computador", "ligar", "energia", ...],
    "scoreSimilaridade": 0.75
  }
]
```

- ✅ Retorna FAQs relevantes
- ✅ Score de similaridade calculado
- ✅ Ordenado por relevância

---

### Teste 8: Conformidade LGPD

**Objetivo**: Verificar registro de logs

**Passos**:
1. Faça login no sistema
2. Navegue por diferentes páginas
3. Crie um chamado
4. Adicione um comentário
5. Conecte ao banco de dados:

```bash
psql -U postgres -d pim
```

6. Consulte os logs:

```sql
SELECT * FROM logs_lgpd ORDER BY timestamp DESC LIMIT 10;
```

**Resultado Esperado**:
```
 id | usuario_email      | acao        | tabela    | registro_id | timestamp
----+-------------------+-------------+-----------+-------------+-------------------
  5 | user@sistema.com  | modificacao | interacoes|      4      | 2025-01-15 10:30
  4 | user@sistema.com  | criacao     | chamados  |      4      | 2025-01-15 10:25
  3 | user@sistema.com  | acesso      | chamados  |      1      | 2025-01-15 10:20
  2 | user@sistema.com  | listagem    | chamados  |    NULL     | 2025-01-15 10:15
  1 | user@sistema.com  | login       | usuarios  |    NULL     | 2025-01-15 10:10
```

- ✅ Todos os acessos registrados
- ✅ IP e User Agent capturados
- ✅ Ações identificadas corretamente

---

### Teste 9: Gráficos Interativos

**Objetivo**: Verificar funcionamento dos gráficos Chart.js

**Pré-requisito**: Estar no Dashboard

**Passos**:
1. No Dashboard, observe o gráfico de pizza
2. Passe o mouse sobre as fatias
   - **Esperado**: Tooltip com detalhes (quantidade e percentual)

3. Observe o gráfico de barras
4. Passe o mouse sobre as barras
   - **Esperado**: Tooltip com valores

**Resultado Esperado**:
- ✅ Gráficos renderizados corretamente
- ✅ Cores diferenciadas
- ✅ Tooltips funcionando
- ✅ Responsivos (teste redimensionar janela)

---

### Teste 10: Logout

**Objetivo**: Verificar encerramento de sessão

**Passos**:
1. Estando logado, clique em "Sair" no menu lateral
2. Verifique se foi redirecionado para /Login
3. Tente acessar diretamente /Dashboard
   - Digite: http://localhost:5000/Dashboard

**Resultado Esperado**:
- ✅ Redirecionado para login
- ✅ Sessão limpa
- ✅ Não consegue acessar páginas protegidas

---

### Teste 11: Algoritmo de Similaridade

**Objetivo**: Testar busca inteligente de FAQs

**Passos**:
1. Vá para "Abrir Novo Chamado"
2. Teste diferentes descrições:

**Teste A**: "impressora não funciona"
- **Esperado**: FAQ sobre impressora

**Teste B**: "esqueci senha sistema"
- **Esperado**: FAQ sobre reset de senha

**Teste C**: "internet lenta conexão ruim"
- **Esperado**: FAQ sobre problemas de rede

**Teste D**: "xyz abc 123" (texto sem sentido)
- **Esperado**: FAQs populares ou mensagem de nenhuma solução

**Resultado Esperado**:
- ✅ Busca retorna FAQs relevantes
- ✅ Ignora stop words
- ✅ Calcula similaridade corretamente
- ✅ Trata casos sem resultado

---

### Teste 12: Responsividade

**Objetivo**: Verificar layout em diferentes tamanhos

**Passos**:
1. Abra o sistema no navegador
2. Pressione F12 (DevTools)
3. Ative o modo responsivo
4. Teste em diferentes resoluções:
   - Desktop: 1920x1080
   - Tablet: 768x1024
   - Mobile: 375x667

5. Verifique:
   - Menu lateral (deve colapsar em mobile)
   - Gráficos (devem ajustar tamanho)
   - Tabelas (devem ter scroll horizontal)
   - Cards (devem empilhar)

**Resultado Esperado**:
- ✅ Layout adaptável
- ✅ Botão de toggle do menu funciona
- ✅ Conteúdo legível em todas as resoluções

---

### Teste 13: Validação de Formulários

**Objetivo**: Verificar validações client-side e server-side

**Passos**:
1. Vá para "Abrir Novo Chamado"
2. Tente submeter formulário vazio
   - **Esperado**: Mensagens de campo obrigatório

3. Preencha apenas o título e submeta
   - **Esperado**: Validação de descrição

4. Preencha todos os campos e submeta
   - **Esperado**: Chamado criado

**Resultado Esperado**:
- ✅ Validações HTML5 funcionando
- ✅ Mensagens claras
- ✅ Não permite submissão inválida

---

### Teste 14: Performance de Consultas

**Objetivo**: Verificar uso de views e índices

**Passos**:
1. Conecte ao banco:

```bash
psql -U postgres -d pim
```

2. Execute consultas e verifique tempo:

```sql
-- Teste 1: Dashboard (deve usar view)
EXPLAIN ANALYZE SELECT * FROM vw_dashboard_resumo;

-- Teste 2: Chamados por status (deve usar view)
EXPLAIN ANALYZE SELECT * FROM vw_chamados_por_status;

-- Teste 3: FAQs populares (deve usar view e índice)
EXPLAIN ANALYZE SELECT * FROM vw_faqs_populares LIMIT 10;
```

**Resultado Esperado**:
- ✅ Consultas executam em < 50ms
- ✅ Views são utilizadas
- ✅ Índices são aplicados (Seq Scan evitado)

---

### Teste 15: Segurança

**Objetivo**: Verificar proteções básicas

**Passos**:
1. **Teste SQL Injection**:
   - No login, tente: `admin@sistema.com' OR '1'='1`
   - **Esperado**: Não deve funcionar (parametrização protege)

2. **Teste Session Hijacking**:
   - Faça logout
   - Use botão "Voltar" do navegador
   - **Esperado**: Redirecionado para login

3. **Teste Acesso Direto**:
   - Sem estar logado, tente acessar:
     - http://localhost:5000/Dashboard
     - http://localhost:5000/Chamados/Novo
   - **Esperado**: Redirecionado para login

**Resultado Esperado**:
- ✅ Proteção contra SQL Injection
- ✅ Sessões seguras
- ✅ Páginas protegidas

---

## Checklist Final

Antes de considerar o projeto completo, verifique:

### Funcionalidades Básicas
- [ ] Login funciona
- [ ] Dashboard exibe dados corretos
- [ ] Criar chamado funciona
- [ ] Listar chamados funciona
- [ ] Ver detalhes funciona
- [ ] Adicionar comentário funciona
- [ ] Logout funciona

### Funcionalidades Avançadas (PIM IV)
- [ ] FAQ dinâmica com IA funciona
- [ ] Algoritmo de similaridade retorna resultados relevantes
- [ ] Gráficos são renderizados corretamente
- [ ] Logs LGPD são registrados
- [ ] Views otimizadas são utilizadas
- [ ] API de FAQs responde corretamente

### Qualidade
- [ ] Código compila sem erros
- [ ] Sem warnings críticos
- [ ] Layout responsivo
- [ ] Performance adequada (< 1s para páginas)
- [ ] Validações funcionam
- [ ] Tratamento de erros implementado

### Documentação
- [ ] README.md completo
- [ ] Guia de instalação claro
- [ ] Comentários no código
- [ ] Scripts SQL documentados

---

## Problemas Comuns e Soluções

### Erro: "Npgsql.NpgsqlException: Connection refused"

**Solução**:
```bash
sudo systemctl start postgresql
```

### Erro: "password authentication failed"

**Solução**:
```bash
sudo -u postgres psql
ALTER USER postgres PASSWORD 'nova_senha';
\q

# Atualizar appsettings.json com a nova senha
```

### Erro: "relation does not exist"

**Solução**:
```bash
# Executar migrations novamente
psql -U postgres -d pim -f Data/Scripts/migrations.sql
```

### Gráficos não aparecem

**Solução**:
- Verifique se Chart.js está carregando (F12 > Network)
- Verifique console do navegador (F12 > Console)
- Certifique-se de que há dados no banco

### FAQ dinâmica não funciona

**Solução**:
- Verifique se a API está respondendo: `curl http://localhost:5000/api/faqs/buscar?descricao=teste`
- Verifique console do navegador (F12)
- Certifique-se de que há FAQs no banco

---

## Métricas de Sucesso

O projeto está completo quando:

1. ✅ Todos os 15 testes passam
2. ✅ Checklist final 100% marcado
3. ✅ Nenhum erro crítico no console
4. ✅ Performance < 1s para todas as páginas
5. ✅ Documentação completa

---

**Desenvolvido para**: PIM IV - UNIP 2025
**Tecnologias**: ASP.NET Core 8.0, PostgreSQL, Bootstrap 5, Chart.js
