#!/bin/bash

# Script para criar as páginas Razor restantes do projeto
# PIM IV - Sistema de Chamados

PROJECT_DIR="/home/ubuntu/pim/SistemaChamados.Web"

echo "Criando páginas Razor restantes..."

# ===== Página: Novo Chamado =====
cat > "$PROJECT_DIR/Pages/Chamados/Novo.cshtml" << 'EOF'
@page
@model SistemaChamados.Web.Pages.Chamados.NovoModel
@{
    ViewData["Title"] = "Abrir Novo Chamado";
}

<div class="row">
    <div class="col-lg-8">
        <div class="card">
            <div class="card-header bg-white">
                <h5 class="mb-0">Abrir Novo Chamado de Suporte</h5>
            </div>
            <div class="card-body">
                <form method="post">
                    <div class="mb-3">
                        <label for="titulo" class="form-label">Título do Chamado *</label>
                        <input type="text" 
                               class="form-control" 
                               id="titulo" 
                               name="titulo" 
                               placeholder="Ex: Computador não liga" 
                               required 
                               maxlength="255">
                    </div>

                    <div class="mb-3">
                        <label for="descricao" class="form-label">Descrição do Problema *</label>
                        <textarea class="form-control" 
                                  id="descricao" 
                                  name="descricao" 
                                  rows="5" 
                                  placeholder="Descreva detalhadamente o problema..." 
                                  required></textarea>
                        <small class="text-muted">Digite sua descrição para ver sugestões de solução abaixo</small>
                    </div>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="categoria" class="form-label">Categoria *</label>
                            <select class="form-select" id="categoria" name="categoria" required>
                                <option value="">Selecione...</option>
                                <option value="Hardware">Hardware</option>
                                <option value="Software">Software</option>
                                <option value="Rede">Rede</option>
                                <option value="E-mail">E-mail</option>
                                <option value="Acesso">Acesso</option>
                                <option value="Impressora">Impressora</option>
                                <option value="Telefonia">Telefonia</option>
                                <option value="Outros">Outros</option>
                            </select>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="prioridade" class="form-label">Prioridade *</label>
                            <select class="form-select" id="prioridade" name="prioridade" required>
                                <option value="">Selecione...</option>
                                <option value="Baixa">Baixa</option>
                                <option value="Média">Média</option>
                                <option value="Alta">Alta</option>
                                <option value="Urgente">Urgente</option>
                            </select>
                        </div>
                    </div>

                    <div class="d-grid">
                        <button type="submit" class="btn btn-primary btn-lg">
                            <i class="bi bi-send me-2"></i>Registrar Chamado
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <div class="col-lg-4">
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h6 class="mb-0"><i class="bi bi-lightbulb me-2"></i>Possíveis Soluções (FAQ Dinâmica)</h6>
            </div>
            <div class="card-body" id="faqContainer">
                <p class="text-muted text-center">
                    <i class="bi bi-info-circle"></i><br>
                    Digite a descrição do problema para ver sugestões automáticas
                </p>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        const descricaoInput = document.getElementById('descricao');
        const faqContainer = document.getElementById('faqContainer');

        // Debounce para evitar muitas requisições
        let timeoutId;
        descricaoInput.addEventListener('input', function() {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => buscarFAQs(), 500);
        });

        async function buscarFAQs() {
            const descricao = descricaoInput.value.trim();
            
            if (descricao.length < 10) {
                faqContainer.innerHTML = '<p class="text-muted text-center"><i class="bi bi-info-circle"></i><br>Digite mais detalhes para ver sugestões</p>';
                return;
            }

            faqContainer.innerHTML = '<div class="text-center"><div class="spinner-border spinner-border-sm"></div> Buscando soluções...</div>';

            try {
                const response = await fetch(`/api/faqs/buscar?descricao=${encodeURIComponent(descricao)}`);
                const faqs = await response.json();

                if (faqs.length === 0) {
                    faqContainer.innerHTML = '<p class="text-muted text-center">Nenhuma solução encontrada. Seu chamado será analisado por um técnico.</p>';
                    return;
                }

                let html = '';
                faqs.forEach(faq => {
                    html += `
                        <div class="faq-card border rounded p-3 mb-2">
                            <div class="faq-question">${faq.pergunta}</div>
                            <div class="faq-answer">${faq.resposta}</div>
                            <small class="text-muted">Categoria: ${faq.categoria}</small>
                        </div>
                    `;
                });

                faqContainer.innerHTML = html;
            } catch (error) {
                faqContainer.innerHTML = '<p class="text-danger">Erro ao buscar soluções</p>';
            }
        }
    </script>
}
EOF

# ===== PageModel: Novo Chamado =====
cat > "$PROJECT_DIR/Pages/Chamados/Novo.cshtml.cs" << 'EOF'
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Pages.Chamados
{
    public class NovoModel : PageModel
    {
        private readonly IChamadoService _chamadoService;

        public NovoModel(IChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        public IActionResult OnGet()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string titulo, string descricao, string categoria, string prioridade)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var chamado = new Chamado
                {
                    Titulo = titulo,
                    Descricao = descricao,
                    Categoria = categoria,
                    Prioridade = prioridade,
                    Solicitante = userEmail
                };

                var chamadoId = await _chamadoService.CriarChamadoAsync(chamado, userEmail);

                TempData["SuccessMessage"] = $"Chamado #{chamadoId} criado com sucesso!";
                return RedirectToPage("/Chamados/Detalhes", new { id = chamadoId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao criar chamado: {ex.Message}";
                return Page();
            }
        }
    }
}
EOF

# ===== Página: Meus Chamados =====
cat > "$PROJECT_DIR/Pages/Chamados/Meus.cshtml" << 'EOF'
@page
@model SistemaChamados.Web.Pages.Chamados.MeusModel
@{
    ViewData["Title"] = "Meus Chamados";
}

<div class="row mb-3">
    <div class="col-md-6">
        <h2>Meus Chamados</h2>
    </div>
    <div class="col-md-6 text-end">
        <a href="/Chamados/Novo" class="btn btn-primary">
            <i class="bi bi-plus-circle me-2"></i>Novo Chamado
        </a>
    </div>
</div>

<div class="card">
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Título</th>
                        <th>Categoria</th>
                        <th>Status</th>
                        <th>Prioridade</th>
                        <th>Data Abertura</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    @if (Model.Chamados.Count == 0)
                    {
                        <tr>
                            <td colspan="7" class="text-center text-muted py-4">
                                <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                                Você ainda não possui chamados
                            </td>
                        </tr>
                    }
                    else
                    {
                        @foreach (var chamado in Model.Chamados)
                        {
                            <tr>
                                <td><strong>#@chamado.Id</strong></td>
                                <td>@chamado.Titulo</td>
                                <td>@chamado.Categoria</td>
                                <td><span class="@chamado.StatusBadgeClass">@chamado.Status</span></td>
                                <td><span class="@chamado.PrioridadeBadgeClass">@chamado.Prioridade</span></td>
                                <td>@chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm")</td>
                                <td>
                                    <a href="/Chamados/Detalhes?id=@chamado.Id" class="btn btn-sm btn-outline-primary">
                                        <i class="bi bi-eye me-1"></i>Ver Detalhes
                                    </a>
                                </td>
                            </tr>
                        }
                    }
                </tbody>
            </table>
        </div>
    </div>
</div>
EOF

# ===== PageModel: Meus Chamados =====
cat > "$PROJECT_DIR/Pages/Chamados/Meus.cshtml.cs" << 'EOF'
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models;
using SistemaChamados.Web.Services;
using System.Collections.Generic;

namespace SistemaChamados.Web.Pages.Chamados
{
    public class MeusModel : PageModel
    {
        private readonly IChamadoService _chamadoService;

        public MeusModel(IChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        public List<Chamado> Chamados { get; set; } = new List<Chamado>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                Chamados = await _chamadoService.ListarChamadosUsuarioAsync(userEmail);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao listar chamados: {ex.Message}";
                return Page();
            }
        }
    }
}
EOF

# ===== Página: Detalhes do Chamado =====
cat > "$PROJECT_DIR/Pages/Chamados/Detalhes.cshtml" << 'EOF'
@page
@model SistemaChamados.Web.Pages.Chamados.DetalhesModel
@{
    ViewData["Title"] = $"Chamado #{Model.Chamado.Id}";
}

<div class="row mb-3">
    <div class="col">
        <a href="/Chamados/Meus" class="btn btn-outline-secondary">
            <i class="bi bi-arrow-left me-2"></i>Voltar
        </a>
    </div>
</div>

<div class="row">
    <div class="col-lg-8">
        <div class="card mb-3">
            <div class="card-header bg-white">
                <div class="d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">Chamado #@Model.Chamado.Id</h5>
                    <span class="@Model.Chamado.StatusBadgeClass">@Model.Chamado.Status</span>
                </div>
            </div>
            <div class="card-body">
                <h4>@Model.Chamado.Titulo</h4>
                <p class="text-muted">@Model.Chamado.Descricao</p>
                
                @if (!string.IsNullOrEmpty(Model.Chamado.Solucao))
                {
                    <div class="alert alert-success">
                        <strong><i class="bi bi-check-circle me-2"></i>Solução:</strong>
                        <p class="mb-0">@Model.Chamado.Solucao</p>
                    </div>
                }
            </div>
        </div>

        <div class="card">
            <div class="card-header bg-white">
                <h6 class="mb-0">Timeline de Interações</h6>
            </div>
            <div class="card-body">
                <div class="timeline">
                    @foreach (var interacao in Model.Chamado.Interacoes)
                    {
                        <div class="timeline-item @(string.IsNullOrEmpty(interacao.Usuario) ? "system" : "")">
                            <div class="timeline-content">
                                <div class="timeline-time">
                                    @interacao.DataHora.ToString("dd/MM/yyyy HH:mm")
                                    @if (!string.IsNullOrEmpty(interacao.Usuario))
                                    {
                                        <span> - @interacao.Usuario</span>
                                    }
                                </div>
                                <div>@interacao.Descricao</div>
                            </div>
                        </div>
                    }
                </div>

                @if (Model.Chamado.Status != "Finalizado")
                {
                    <hr>
                    <form method="post">
                        <input type="hidden" name="chamadoId" value="@Model.Chamado.Id" />
                        <div class="mb-3">
                            <label for="comentario" class="form-label">Adicionar Comentário</label>
                            <textarea class="form-control" id="comentario" name="comentario" rows="3" required></textarea>
                        </div>
                        <button type="submit" class="btn btn-primary">
                            <i class="bi bi-chat-left-text me-2"></i>Adicionar Comentário
                        </button>
                    </form>
                }
            </div>
        </div>
    </div>

    <div class="col-lg-4">
        <div class="card">
            <div class="card-header bg-white">
                <h6 class="mb-0">Informações</h6>
            </div>
            <div class="card-body">
                <dl class="row mb-0">
                    <dt class="col-sm-5">Categoria:</dt>
                    <dd class="col-sm-7">@Model.Chamado.Categoria</dd>

                    <dt class="col-sm-5">Prioridade:</dt>
                    <dd class="col-sm-7"><span class="@Model.Chamado.PrioridadeBadgeClass">@Model.Chamado.Prioridade</span></dd>

                    <dt class="col-sm-5">Solicitante:</dt>
                    <dd class="col-sm-7">@Model.Chamado.Solicitante</dd>

                    <dt class="col-sm-5">Técnico:</dt>
                    <dd class="col-sm-7">@(Model.Chamado.Tecnico ?? "Não atribuído")</dd>

                    <dt class="col-sm-5">Aberto em:</dt>
                    <dd class="col-sm-7">@Model.Chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm")</dd>

                    @if (Model.Chamado.DataFechamento.HasValue)
                    {
                        <dt class="col-sm-5">Fechado em:</dt>
                        <dd class="col-sm-7">@Model.Chamado.DataFechamento.Value.ToString("dd/MM/yyyy HH:mm")</dd>
                    }
                </dl>
            </div>
        </div>
    </div>
</div>
EOF

# ===== PageModel: Detalhes do Chamado =====
cat > "$PROJECT_DIR/Pages/Chamados/Detalhes.cshtml.cs" << 'EOF'
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Pages.Chamados
{
    public class DetalhesModel : PageModel
    {
        private readonly IChamadoService _chamadoService;

        public DetalhesModel(IChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        public Chamado Chamado { get; set; } = new Chamado();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var chamado = await _chamadoService.ObterChamadoPorIdAsync(id, userEmail);
                
                if (chamado == null)
                {
                    return NotFound();
                }

                Chamado = chamado;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar chamado: {ex.Message}";
                return RedirectToPage("/Chamados/Meus");
            }
        }

        public async Task<IActionResult> OnPostAsync(int chamadoId, string comentario)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _chamadoService.AdicionarComentarioAsync(chamadoId, comentario, userEmail);
                TempData["SuccessMessage"] = "Comentário adicionado com sucesso!";
                return RedirectToPage(new { id = chamadoId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao adicionar comentário: {ex.Message}";
                return RedirectToPage(new { id = chamadoId });
            }
        }
    }
}
EOF

# ===== Página: Logout =====
cat > "$PROJECT_DIR/Pages/Logout.cshtml" << 'EOF'
@page
@model SistemaChamados.Web.Pages.LogoutModel
EOF

cat > "$PROJECT_DIR/Pages/Logout.cshtml.cs" << 'EOF'
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SistemaChamados.Web.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
EOF

echo "✅ Todas as páginas foram criadas com sucesso!"
echo ""
echo "Páginas criadas:"
echo "  - /Chamados/Novo.cshtml (+ .cs)"
echo "  - /Chamados/Meus.cshtml (+ .cs)"
echo "  - /Chamados/Detalhes.cshtml (+ .cs)"
echo "  - /Logout.cshtml (+ .cs)"
echo ""
echo "Próximos passos:"
echo "  1. Execute as migrations: psql -U postgres -d pim -f Data/Scripts/migrations.sql"
echo "  2. Compile o projeto: dotnet build"
echo "  3. Execute a aplicação: dotnet run"
echo ""
EOF

chmod +x /home/ubuntu/pim/criar_paginas_restantes.sh

echo "Script criado com sucesso!"
