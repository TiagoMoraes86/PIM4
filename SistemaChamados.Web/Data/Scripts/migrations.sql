
-- Script de Migrations - PIM IV
-- Sistema de Chamados com IA e LGPD

-- Se não existirem, criar:

-- Tabela de usuários (caso não exista)
CREATE TABLE IF NOT EXISTS usuarios (
    email VARCHAR(255) PRIMARY KEY,
    senha VARCHAR(255) NOT NULL,
    nome VARCHAR(255) NOT NULL,
    tipo VARCHAR(50) NOT NULL DEFAULT 'comum', -- 'comum', 'tecnico', 'admin'
    ativo BOOLEAN DEFAULT TRUE,
    criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de chamados (caso não exista)
CREATE TABLE IF NOT EXISTS chamados (
    id SERIAL PRIMARY KEY,
    titulo VARCHAR(255) NOT NULL,
    descricao TEXT NOT NULL,
    categoria VARCHAR(100) NOT NULL,
    prioridade VARCHAR(50) NOT NULL, -- 'baixa', 'media', 'alta', 'urgente'
    data_abertura TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_fechamento TIMESTAMP NULL,
    solicitante VARCHAR(255) NOT NULL,
    tecnico VARCHAR(255) NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'Aguardando atribuição',
    solucao TEXT NULL,
    FOREIGN KEY (solicitante) REFERENCES usuarios(email),
    FOREIGN KEY (tecnico) REFERENCES usuarios(email)
);

-- Tabela de interações (caso não exista)
CREATE TABLE IF NOT EXISTS interacoes (
    id SERIAL PRIMARY KEY,
    chamado_id INTEGER NOT NULL,
    data_hora TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    descricao TEXT NOT NULL,
    usuario VARCHAR(255) NULL,
    FOREIGN KEY (chamado_id) REFERENCES chamados(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario) REFERENCES usuarios(email)
);

-- NOVAS TABELAS - PIM IV

-- Tabela de FAQs dinâmicas
CREATE TABLE IF NOT EXISTS faqs (
    id SERIAL PRIMARY KEY,
    pergunta TEXT NOT NULL,
    resposta TEXT NOT NULL,
    categoria VARCHAR(100) NOT NULL,
    relevancia INTEGER DEFAULT 0, -- Contador de quantas vezes foi útil
    palavras_chave TEXT[], -- Array de palavras-chave para busca
    criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    atualizado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Índice para busca rápida em FAQs
CREATE INDEX IF NOT EXISTS idx_faqs_categoria ON faqs(categoria);
CREATE INDEX IF NOT EXISTS idx_faqs_palavras_chave ON faqs USING GIN(palavras_chave);

-- Tabela de sugestões da IA
CREATE TABLE IF NOT EXISTS sugestoes_ia (
    id SERIAL PRIMARY KEY,
    chamado_id INTEGER NOT NULL,
    sugestao TEXT NOT NULL,
    confianca DECIMAL(5,2) DEFAULT 0.0, 
    aceita BOOLEAN DEFAULT FALSE,
    feedback TEXT NULL, 
    criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (chamado_id) REFERENCES chamados(id) ON DELETE CASCADE
);

-- Índice para busca de sugestões por chamado
CREATE INDEX IF NOT EXISTS idx_sugestoes_chamado ON sugestoes_ia(chamado_id);

-- Tabela de logs LGPD
CREATE TABLE IF NOT EXISTS logs_lgpd (
    id SERIAL PRIMARY KEY,
    usuario_email VARCHAR(255) NOT NULL,
    acao VARCHAR(100) NOT NULL, -- 'acesso', 'modificacao', 'exclusao', 'exportacao'
    tabela VARCHAR(100) NOT NULL,
    registro_id INTEGER NULL,
    dados_acessados TEXT NULL, -- JSON com campos acessados
    ip_address VARCHAR(45) NULL, -- Suporta IPv4 e IPv6
    user_agent TEXT NULL,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_email) REFERENCES usuarios(email)
);

-- Índices para consultas de LGPD
CREATE INDEX IF NOT EXISTS idx_logs_usuario ON logs_lgpd(usuario_email);
CREATE INDEX IF NOT EXISTS idx_logs_timestamp ON logs_lgpd(timestamp);
CREATE INDEX IF NOT EXISTS idx_logs_acao ON logs_lgpd(acao);

-- Tabela de consentimentos LGPD
CREATE TABLE IF NOT EXISTS consentimentos (
    id SERIAL PRIMARY KEY,
    usuario_email VARCHAR(255) NOT NULL,
    tipo_consentimento VARCHAR(100) NOT NULL, -- 'uso_dados', 'marketing', 'compartilhamento'
    consentido BOOLEAN NOT NULL,
    data_consentimento TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ip_address VARCHAR(45) NULL,
    revogado BOOLEAN DEFAULT FALSE,
    data_revogacao TIMESTAMP NULL,
    FOREIGN KEY (usuario_email) REFERENCES usuarios(email)
);

-- Índice para busca de consentimentos
CREATE INDEX IF NOT EXISTS idx_consentimentos_usuario ON consentimentos(usuario_email);

-- Tabela de categorias de chamados (para padronização)
CREATE TABLE IF NOT EXISTS categorias (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL UNIQUE,
    descricao TEXT NULL,
    ativo BOOLEAN DEFAULT TRUE,
    ordem INTEGER DEFAULT 0
);

-- Tabela de estatísticas agregadas (para performance do dashboard)
CREATE TABLE IF NOT EXISTS estatisticas_diarias (
    id SERIAL PRIMARY KEY,
    data DATE NOT NULL UNIQUE,
    total_chamados INTEGER DEFAULT 0,
    chamados_abertos INTEGER DEFAULT 0,
    chamados_em_analise INTEGER DEFAULT 0,
    chamados_finalizados INTEGER DEFAULT 0,
    tempo_medio_resolucao INTERVAL NULL,
    atualizado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Índice para busca por data
CREATE INDEX IF NOT EXISTS idx_estatisticas_data ON estatisticas_diarias(data DESC);

-- DADOS INICIAIS

-- Inserir usuário admin padrão (senha: admin123)
INSERT INTO usuarios (email, senha, nome, tipo) 
VALUES ('admin@sistema.com', '$2b$12$1Za3/oo.VTrPRrT6HTAFuuDVgydTOZ35ENQikvPuToK9OKA4g99ne', 'Administrador', 'admin')
ON CONFLICT (email) DO NOTHING;

-- Inserir usuário técnico padrão (senha: tecnico123)
INSERT INTO usuarios (email, senha, nome, tipo) 
VALUES ('tecnico@sistema.com', '$2b$12$Qpm9lNbdernTAt4KicW73O/ZmIV41.rsTchi0LTOIoGpoJPUSJcPG', 'Técnico Suporte', 'tecnico')
ON CONFLICT (email) DO NOTHING;

-- Inserir usuário comum padrão (senha: user123)
INSERT INTO usuarios (email, senha, nome, tipo) 
VALUES ('user@sistema.com', '$2b$12$/SsYbzYUv.SAqjnxqmtdXerd0zPCsn6O5OOGntT/ch9mCNnu/XESG', 'Usuário Teste', 'comum')
ON CONFLICT (email) DO NOTHING;

-- Inserir categorias padrão
INSERT INTO categorias (nome, descricao, ordem) VALUES
    ('Hardware', 'Problemas relacionados a equipamentos físicos', 1),
    ('Software', 'Problemas com aplicativos e sistemas', 2),
    ('Rede', 'Problemas de conectividade e internet', 3),
    ('E-mail', 'Problemas com contas de e-mail', 4),
    ('Acesso', 'Problemas de login e permissões', 5),
    ('Impressora', 'Problemas com impressoras e scanners', 6),
    ('Telefonia', 'Problemas com telefones e ramais', 7),
    ('Outros', 'Outros tipos de solicitações', 99)
ON CONFLICT (nome) DO NOTHING;

-- Inserir FAQs iniciais
INSERT INTO faqs (pergunta, resposta, categoria, palavras_chave, relevancia) VALUES
    (
        'Como resetar minha senha?',
        'Para resetar sua senha, acesse a página de login e clique em "Esqueci minha senha". Você receberá um e-mail com instruções para criar uma nova senha.',
        'Acesso',
        ARRAY['senha', 'reset', 'esqueci', 'login', 'acesso'],
        0
    ),
    (
        'Meu computador está lento, o que fazer?',
        'Tente reiniciar o computador. Se o problema persistir, verifique se há muitos programas abertos. Feche aplicativos desnecessários e limpe arquivos temporários. Se continuar lento, abra um chamado para análise técnica.',
        'Hardware',
        ARRAY['lento', 'computador', 'travando', 'performance', 'devagar'],
        0
    ),
    (
        'Não consigo acessar a internet',
        'Primeiro, verifique se o cabo de rede está conectado ou se o Wi-Fi está ativado. Tente reiniciar o roteador. Se o problema persistir, entre em contato com o suporte de TI.',
        'Rede',
        ARRAY['internet', 'rede', 'conexao', 'wifi', 'conectar'],
        0
    ),
    (
        'A impressora não está imprimindo',
        'Verifique se a impressora está ligada e conectada ao computador ou rede. Confira se há papel e tinta/toner. Tente reiniciar a impressora. Se não resolver, abra um chamado.',
        'Impressora',
        ARRAY['impressora', 'imprimir', 'papel', 'toner', 'tinta'],
        0
    ),
    (
        'Não consigo abrir um arquivo',
        'Verifique se você tem o programa correto instalado para abrir o arquivo. Tente abrir com outro aplicativo. Se o arquivo estiver corrompido, pode ser necessário recuperá-lo de um backup.',
        'Software',
        ARRAY['arquivo', 'abrir', 'programa', 'corrompido', 'documento'],
        0
    ),
    (
        'Meu e-mail não está enviando mensagens',
        'Verifique sua conexão com a internet. Confirme se o endereço de e-mail do destinatário está correto. Verifique se sua caixa de saída não está cheia. Se o problema persistir, contate o suporte.',
        'E-mail',
        ARRAY['email', 'enviar', 'mensagem', 'correio', 'outlook'],
        0
    )
ON CONFLICT DO NOTHING;


-- FUNÇÕES AUXILIARES

-- Função para atualizar estatísticas diárias
CREATE OR REPLACE FUNCTION atualizar_estatisticas_diarias()
RETURNS void AS $$
BEGIN
    INSERT INTO estatisticas_diarias (
        data,
        total_chamados,
        chamados_abertos,
        chamados_em_analise,
        chamados_finalizados,
        tempo_medio_resolucao
    )
    SELECT
        CURRENT_DATE,
        COUNT(*),
        COUNT(*) FILTER (WHERE status IN ('Aguardando atribuição', 'Aberto')),
        COUNT(*) FILTER (WHERE status = 'Em análise'),
        COUNT(*) FILTER (WHERE status = 'Finalizado' AND DATE(data_fechamento) = CURRENT_DATE),
        AVG(data_fechamento - data_abertura) FILTER (WHERE status = 'Finalizado' AND DATE(data_fechamento) = CURRENT_DATE)
    FROM chamados
    ON CONFLICT (data) DO UPDATE SET
        total_chamados = EXCLUDED.total_chamados,
        chamados_abertos = EXCLUDED.chamados_abertos,
        chamados_em_analise = EXCLUDED.chamados_em_analise,
        chamados_finalizados = EXCLUDED.chamados_finalizados,
        tempo_medio_resolucao = EXCLUDED.tempo_medio_resolucao,
        atualizado_em = CURRENT_TIMESTAMP;
END;
$$ LANGUAGE plpgsql;

-- Função para incrementar relevância de FAQ
CREATE OR REPLACE FUNCTION incrementar_relevancia_faq(faq_id INTEGER)
RETURNS void AS $$
BEGIN
    UPDATE faqs 
    SET relevancia = relevancia + 1,
        atualizado_em = CURRENT_TIMESTAMP
    WHERE id = faq_id;
END;
$$ LANGUAGE plpgsql;


-- TRIGGERS


-- Trigger para atualizar timestamp de FAQ
CREATE OR REPLACE FUNCTION atualizar_timestamp_faq()
RETURNS TRIGGER AS $$
BEGIN
    NEW.atualizado_em = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_atualizar_faq
BEFORE UPDATE ON faqs
FOR EACH ROW
EXECUTE FUNCTION atualizar_timestamp_faq();


-- VIEWS ÚTEIS

-- View para dashboard - resumo de chamados
CREATE OR REPLACE VIEW vw_dashboard_resumo AS
SELECT
    COUNT(*) AS total_chamados,
    COUNT(*) FILTER (WHERE status IN ('Aguardando atribuição', 'Aberto')) AS chamados_abertos,
    COUNT(*) FILTER (WHERE status = 'Em análise') AS chamados_em_analise,
    COUNT(*) FILTER (WHERE status = 'Finalizado') AS chamados_finalizados,
    COUNT(*) FILTER (WHERE DATE(data_abertura) >= CURRENT_DATE - INTERVAL '30 days' AND status = 'Finalizado') AS finalizados_mes,
    AVG(EXTRACT(EPOCH FROM (data_fechamento - data_abertura))/3600) FILTER (WHERE status = 'Finalizado' AND DATE(data_fechamento) >= CURRENT_DATE - INTERVAL '30 days') AS tempo_medio_horas
FROM chamados;

-- View para chamados por status (para gráfico de pizza)
CREATE OR REPLACE VIEW vw_chamados_por_status AS
SELECT
    status,
    COUNT(*) AS quantidade,
    ROUND(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER (), 2) AS percentual
FROM chamados
GROUP BY status
ORDER BY quantidade DESC;

-- View para chamados por mês (para gráfico de barras)
CREATE OR REPLACE VIEW vw_chamados_por_mes AS
SELECT
    TO_CHAR(data_abertura, 'YYYY-MM') AS mes,
    TO_CHAR(data_abertura, 'Mon/YYYY') AS mes_formatado,
    COUNT(*) AS quantidade,
    COUNT(*) FILTER (WHERE status = 'Finalizado') AS finalizados
FROM chamados
WHERE data_abertura >= CURRENT_DATE - INTERVAL '6 months'
GROUP BY TO_CHAR(data_abertura, 'YYYY-MM'), TO_CHAR(data_abertura, 'Mon/YYYY')
ORDER BY mes DESC;

-- View para FAQs mais relevantes
CREATE OR REPLACE VIEW vw_faqs_populares AS
SELECT
    id,
    pergunta,
    resposta,
    categoria,
    relevancia,
    palavras_chave
FROM faqs
WHERE relevancia > 0
ORDER BY relevancia DESC
LIMIT 10;


-- COMENTÁRIOS NAS TABELAS (DOCUMENTAÇÃO)


COMMENT ON TABLE faqs IS 'Armazena perguntas frequentes geradas dinamicamente com base no histórico de chamados';
COMMENT ON TABLE sugestoes_ia IS 'Registra sugestões automáticas geradas pela IA para resolução de chamados';
COMMENT ON TABLE logs_lgpd IS 'Logs de auditoria para conformidade com LGPD - registra todos os acessos a dados pessoais';
COMMENT ON TABLE consentimentos IS 'Registra consentimentos dos usuários conforme LGPD';
COMMENT ON TABLE estatisticas_diarias IS 'Cache de estatísticas agregadas para melhorar performance do dashboard';


-- FINALIZAÇÃO

-- Atualizar estatísticas iniciais
SELECT atualizar_estatisticas_diarias();

-- Mensagem de sucesso
DO $$
BEGIN
    RAISE NOTICE 'Migrations executadas com sucesso! Banco de dados PIM IV configurado.';
END $$;
