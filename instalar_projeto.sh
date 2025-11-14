#!/bin/bash

# =====================================================
# Script de Instalação Automatizada
# Sistema de Chamados - PIM IV
# =====================================================

set -e  # Parar em caso de erro

echo "╔════════════════════════════════════════════════════════════╗"
echo "║   Sistema de Chamados - PIM IV                             ║"
echo "║   Script de Instalação Automatizada                        ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Função para imprimir com cores
print_success() {
    echo -e "${GREEN}✓${NC} $1"
}

print_error() {
    echo -e "${RED}✗${NC} $1"
}

print_info() {
    echo -e "${BLUE}ℹ${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}⚠${NC} $1"
}

# =====================================================
# 1. Verificar Pré-requisitos
# =====================================================
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "  ETAPA 1: Verificando Pré-requisitos"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Verificar .NET
print_info "Verificando .NET SDK..."
export PATH="$PATH:/home/ubuntu/.dotnet"
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    print_success ".NET SDK encontrado: versão $DOTNET_VERSION"
else
    print_error ".NET SDK não encontrado!"
    print_info "Instalando .NET SDK 8.0..."
    
    wget https://dot.net/v1/dotnet-install.sh -O /tmp/dotnet-install.sh
    chmod +x /tmp/dotnet-install.sh
    /tmp/dotnet-install.sh --channel 8.0 --install-dir /home/ubuntu/.dotnet
    
    export PATH="$PATH:/home/ubuntu/.dotnet"
    print_success ".NET SDK instalado com sucesso!"
fi

# Verificar PostgreSQL
print_info "Verificando PostgreSQL..."
if command -v psql &> /dev/null; then
    PG_VERSION=$(psql --version | awk '{print $3}')
    print_success "PostgreSQL encontrado: versão $PG_VERSION"
else
    print_error "PostgreSQL não encontrado!"
    print_info "Por favor, instale o PostgreSQL:"
    echo "    sudo apt update"
    echo "    sudo apt install postgresql postgresql-contrib"
    exit 1
fi

# Verificar se PostgreSQL está rodando
print_info "Verificando serviço PostgreSQL..."
if sudo systemctl is-active --quiet postgresql; then
    print_success "PostgreSQL está rodando"
else
    print_warning "PostgreSQL não está rodando. Iniciando..."
    sudo systemctl start postgresql
    print_success "PostgreSQL iniciado"
fi

# =====================================================
# 2. Configurar Banco de Dados
# =====================================================
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "  ETAPA 2: Configurando Banco de Dados"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Verificar se banco já existe
print_info "Verificando banco de dados 'pim'..."
if sudo -u postgres psql -lqt | cut -d \| -f 1 | grep -qw pim; then
    print_warning "Banco 'pim' já existe!"
    read -p "Deseja recriar o banco? (s/N): " RECREATE
    if [[ $RECREATE =~ ^[Ss]$ ]]; then
        print_info "Removendo banco existente..."
        sudo -u postgres psql -c "DROP DATABASE IF EXISTS pim;"
        sudo -u postgres psql -c "CREATE DATABASE pim;"
        print_success "Banco recriado"
    else
        print_info "Mantendo banco existente"
    fi
else
    print_info "Criando banco de dados 'pim'..."
    sudo -u postgres psql -c "CREATE DATABASE pim;"
    print_success "Banco criado"
fi

# Executar migrations
print_info "Executando migrations..."
cd /home/ubuntu/pim/SistemaChamados.Web
if [ -f "Data/Scripts/migrations.sql" ]; then
    sudo -u postgres psql -d pim -f Data/Scripts/migrations.sql > /tmp/migrations.log 2>&1
    if [ $? -eq 0 ]; then
        print_success "Migrations executadas com sucesso"
    else
        print_error "Erro ao executar migrations. Verifique /tmp/migrations.log"
        exit 1
    fi
else
    print_error "Arquivo migrations.sql não encontrado!"
    exit 1
fi

# =====================================================
# 3. Gerar Hashes de Senha
# =====================================================
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "  ETAPA 3: Gerando Hashes de Senha"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

print_info "Gerando hashes BCrypt para senhas..."

# Criar programa temporário para gerar hashes
cat > /tmp/HashGen.cs << 'HASHEOF'
using System;

var senhas = new[] { 
    ("admin@sistema.com", "admin123"),
    ("tecnico@sistema.com", "tecnico123"),
    ("user@sistema.com", "user123"),
    ("pedro@empresa.com", "user123"),
    ("ana@empresa.com", "user123")
};

foreach (var (email, senha) in senhas)
{
    var hash = BCrypt.Net.BCrypt.HashPassword(senha);
    Console.WriteLine($"UPDATE usuarios SET senha = '{hash}' WHERE email = '{email}';");
}
HASHEOF

# Executar gerador de hashes
cd /home/ubuntu/pim/SistemaChamados.Web
dotnet restore > /dev/null 2>&1
dotnet script /tmp/HashGen.cs > /tmp/update_senhas.sql 2>/dev/null

if [ -f /tmp/update_senhas.sql ]; then
    print_success "Hashes gerados"
    
    # Aplicar hashes no banco
    print_info "Atualizando senhas no banco..."
    sudo -u postgres psql -d pim -f /tmp/update_senhas.sql > /dev/null 2>&1
    print_success "Senhas atualizadas"
else
    print_warning "Não foi possível gerar hashes automaticamente"
    print_info "Execute manualmente: bash /home/ubuntu/pim/gerar_hashes.sh"
fi

# Popular dados iniciais
print_info "Populando dados iniciais..."
if [ -f "Data/Scripts/seed_data.sql" ]; then
    sudo -u postgres psql -d pim -f Data/Scripts/seed_data.sql > /tmp/seed.log 2>&1
    print_success "Dados iniciais inseridos"
fi

# =====================================================
# 4. Compilar Projeto
# =====================================================
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "  ETAPA 4: Compilando Projeto"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

cd /home/ubuntu/pim/SistemaChamados.Web

print_info "Restaurando pacotes NuGet..."
dotnet restore > /tmp/restore.log 2>&1
if [ $? -eq 0 ]; then
    print_success "Pacotes restaurados"
else
    print_error "Erro ao restaurar pacotes. Verifique /tmp/restore.log"
    exit 1
fi

print_info "Compilando projeto..."
dotnet build > /tmp/build.log 2>&1
if [ $? -eq 0 ]; then
    print_success "Projeto compilado com sucesso"
else
    print_error "Erro na compilação. Verifique /tmp/build.log"
    exit 1
fi

# =====================================================
# 5. Criar Script de Execução
# =====================================================
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "  ETAPA 5: Criando Scripts de Execução"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Script para iniciar o sistema
cat > /home/ubuntu/pim/iniciar_sistema.sh << 'STARTEOF'
#!/bin/bash

echo "Iniciando Sistema de Chamados..."
echo ""

# Verificar PostgreSQL
if ! sudo systemctl is-active --quiet postgresql; then
    echo "Iniciando PostgreSQL..."
    sudo systemctl start postgresql
fi

# Navegar para o projeto
cd /home/ubuntu/pim/SistemaChamados.Web

# Adicionar .NET ao PATH
export PATH="$PATH:/home/ubuntu/.dotnet"

# Executar aplicação
echo "Aplicação rodando em:"
echo "  → http://localhost:5000"
echo "  → https://localhost:5001"
echo ""
echo "Pressione Ctrl+C para parar"
echo ""

dotnet run
STARTEOF

chmod +x /home/ubuntu/pim/iniciar_sistema.sh
print_success "Script de inicialização criado: iniciar_sistema.sh"

# Script para parar o sistema
cat > /home/ubuntu/pim/parar_sistema.sh << 'STOPEOF'
#!/bin/bash

echo "Parando Sistema de Chamados..."

# Encontrar e matar processos dotnet
pkill -f "dotnet.*SistemaChamados.Web"

echo "Sistema parado"
STOPEOF

chmod +x /home/ubuntu/pim/parar_sistema.sh
print_success "Script de parada criado: parar_sistema.sh"

# =====================================================
# 6. Verificar Instalação
# =====================================================
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "  ETAPA 6: Verificando Instalação"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Verificar tabelas no banco
print_info "Verificando tabelas no banco..."
TABLES=$(sudo -u postgres psql -d pim -t -c "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public';")
print_success "Tabelas criadas: $TABLES"

# Verificar usuários
USERS=$(sudo -u postgres psql -d pim -t -c "SELECT COUNT(*) FROM usuarios;")
print_success "Usuários cadastrados: $USERS"

# Verificar FAQs
FAQS=$(sudo -u postgres psql -d pim -t -c "SELECT COUNT(*) FROM faqs;")
print_success "FAQs cadastradas: $FAQS"

# =====================================================
# Instalação Concluída
# =====================================================
echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║   ✓ INSTALAÇÃO CONCLUÍDA COM SUCESSO!                      ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""
echo "Para iniciar o sistema, execute:"
echo "  ${GREEN}bash /home/ubuntu/pim/iniciar_sistema.sh${NC}"
echo ""
echo "Ou manualmente:"
echo "  ${BLUE}cd /home/ubuntu/pim/SistemaChamados.Web${NC}"
echo "  ${BLUE}dotnet run${NC}"
echo ""
echo "Acesse a aplicação em:"
echo "  → ${GREEN}http://localhost:5000${NC}"
echo "  → ${GREEN}https://localhost:5001${NC}"
echo ""
echo "Credenciais de teste:"
echo "  Admin:    admin@sistema.com / admin123"
echo "  Técnico:  tecnico@sistema.com / tecnico123"
echo "  Usuário:  user@sistema.com / user123"
echo ""
echo "Documentação:"
echo "  → README: /home/ubuntu/pim/README_WEB.md"
echo "  → Testes: /home/ubuntu/pim/GUIA_TESTES.md"
echo "  → Entrega: /home/ubuntu/pim/ENTREGA_PROJETO.md"
echo ""
print_success "Instalação finalizada!"
