#!/bin/bash

cd /home/ubuntu/pim/SistemaChamados.Web/Pages

# Criar Meus Chamados
mkdir -p Chamados
cat > Chamados/Meus.cshtml << 'EOF'
@page
@model SistemaChamados.Web.Pages.Chamados.MeusModel
@{
    ViewData["Title"] = "Meus Chamados";
    ViewData["PageTitle"] = "Meus Chamados";
    ViewData["ActivePage"] = "Meus";
}

<div class="topbar">
    <div class="search">
        <input id="searchInput" type="text" placeholder="Buscar chamado..." />
    </div>
    <div class="center">
        <a href="/Chamados/Novo" class="btn btn-primary">Novo Chamado</a>
    </div>
</div>

<section class="card">
    <div class="card-title">Meus Chamados</div>
    <div class="tickets mt-2">
        @if (Model.Chamados != null && Model.Chamados.Any())
        {
            @foreach (var chamado in Model.Chamados)
            {
                <div class="ticket">
                    <div>
                        <div style="font-weight:700;color:#e8fbff">@chamado.Titulo</div>
                        <div class="meta small text-muted">
                            ID #@chamado.Id — Criado em @chamado.CriadoEm.ToString("dd/MM/yyyy HH:mm")
                        </div>
                    </div>
                    <div style="display:flex;gap:10px;align-items:center">
                        <div class="status @(chamado.Status == "Aguardando atribuição" ? "status-open" : chamado.Status == "Finalizado" ? "status-closed" : "")">
                            @chamado.Status.ToUpper()
                        </div>
                        <a href="/Chamados/Detalhes?id=@chamado.Id" class="btn btn-ghost">Ver</a>
                    </div>
                </div>
            }
        }
        else
        {
            <div class="empty">Você ainda não tem chamados abertos.</div>
        }
    </div>
</section>
EOF

# Criar Perfil
cat > Perfil.cshtml << 'EOF'
@page
@{
    ViewData["Title"] = "Perfil";
    ViewData["PageTitle"] = "Perfil";
    ViewData["ActivePage"] = "Perfil";
}

<div class="card" style="padding:18px;">
    <div class="card-title">Meu Perfil</div>
    <div class="mb-3">
        <label class="small text-muted">Nome</label>
        <input type="text" class="input" value="@Context.Session.GetString("UsuarioNome")" readonly>
    </div>
    <div class="mb-3">
        <label class="small text-muted">E-mail</label>
        <input type="email" class="input" value="@Context.Session.GetString("UsuarioEmail")" readonly>
    </div>
    <div class="mb-3">
        <label class="small text-muted">Tipo de Usuário</label>
        <input type="text" class="input" value="@Context.Session.GetString("UsuarioTipo")" readonly>
    </div>
    <button class="btn btn-ghost">Editar Perfil (Em breve)</button>
</div>
EOF

# Criar Relatórios
cat > Relatorios.cshtml << 'EOF'
@page
@{
    ViewData["Title"] = "Relatórios";
    ViewData["PageTitle"] = "Relatórios";
    ViewData["ActivePage"] = "Relatorios";
}

<div class="card" style="padding:18px;">
    <div class="card-title">Relatórios</div>
    <p class="text-muted">Funcionalidade de relatórios em desenvolvimento.</p>
    <div class="empty">Relatórios estarão disponíveis em breve.</div>
</div>
EOF

# Criar Configurações
cat > Configuracoes.cshtml << 'EOF'
@page
@{
    ViewData["Title"] = "Configurações";
    ViewData["PageTitle"] = "Configurações";
    ViewData["ActivePage"] = "Configuracoes";
}

<div class="card" style="padding:18px;">
    <div class="card-title">Configurações</div>
    <div class="mb-3">
        <label class="small text-muted">Tema</label>
        <p class="text-muted small">Use o botão "Modo" no topo para alternar entre tema claro e escuro.</p>
    </div>
    <div class="mb-3">
        <label class="small text-muted">Notificações</label>
        <div>
            <input type="checkbox" id="notif1"> <label for="notif1" class="small text-muted">E-mail ao atualizar chamado</label>
        </div>
    </div>
    <button class="btn btn-primary">Salvar (Em breve)</button>
</div>
EOF

echo "Páginas criadas com sucesso!"
