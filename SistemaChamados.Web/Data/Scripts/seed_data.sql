
-- Script de População de Dados Iniciais


-- Limpar dados existentes (cuidado em produção!)
TRUNCATE TABLE interacoes CASCADE;
TRUNCATE TABLE chamados CASCADE;
TRUNCATE TABLE sugestoes_ia CASCADE;
TRUNCATE TABLE faqs CASCADE;
TRUNCATE TABLE logs_lgpd CASCADE;
TRUNCATE TABLE consentimentos CASCADE;
TRUNCATE TABLE categorias CASCADE;
TRUNCATE TABLE usuarios CASCADE;


-- 1. USUÁRIOS
-- Senhas com hash BCrypt:
-- admin123 -> $2a$11$XKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5
-- tecnico123 -> $2a$11$YKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5
-- user123 -> $2a$11$ZKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5

INSERT INTO usuarios (nome, email, senha, tipo, ativo) VALUES
('Administrador do Sistema', 'admin@sistema.com', '$2a$11$XKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5', 'Admin', true),
('João Silva - Técnico', 'tecnico@sistema.com', '$2a$11$YKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5', 'Técnico', true),
('Maria Santos - Usuária', 'user@sistema.com', '$2a$11$ZKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5', 'Comum', true),
('Pedro Oliveira', 'pedro@empresa.com', '$2a$11$ZKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5', 'Comum', true),
('Ana Costa', 'ana@empresa.com', '$2a$11$ZKJ0YvJQK5vZ8QZJ5Z5Z5e5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5', 'Comum', true);


-- 2. CATEGORIAS
INSERT INTO categorias (nome, descricao, ativo, ordem) VALUES
('Hardware', 'Problemas com equipamentos físicos (computadores, periféricos, etc)', true, 1),
('Software', 'Problemas com programas e aplicativos', true, 2),
('Rede', 'Problemas de conectividade e internet', true, 3),
('E-mail', 'Problemas com contas de e-mail e envio/recebimento', true, 4),
('Acesso', 'Problemas de login e permissões de acesso', true, 5),
('Impressora', 'Problemas com impressoras e digitalização', true, 6),
('Telefonia', 'Problemas com ramais e telefones', true, 7),
('Outros', 'Outros tipos de problemas', true, 99);


-- 3. FAQs (Perguntas Frequentes)
INSERT INTO faqs (pergunta, resposta, categoria, palavras_chave, relevancia) VALUES
(
    'Como resetar minha senha do sistema?',
    'Para resetar sua senha: 1) Clique em "Esqueci minha senha" na tela de login. 2) Digite seu e-mail cadastrado. 3) Você receberá um link por e-mail para criar uma nova senha. 4) O link é válido por 24 horas.',
    'Acesso',
    ARRAY['senha', 'reset', 'esqueci', 'login', 'acesso', 'recuperar'],
    0
),
(
    'Meu computador não liga, o que fazer?',
    'Verifique os seguintes itens: 1) Confira se o cabo de energia está conectado corretamente. 2) Verifique se a tomada está funcionando. 3) Teste com outro cabo de energia se disponível. 4) Verifique se o botão de liga/desliga da fonte está acionado. 5) Se nada funcionar, abra um chamado para o suporte técnico.',
    'Hardware',
    ARRAY['computador', 'ligar', 'energia', 'fonte', 'cabo', 'tomada', 'não liga'],
    0
),
(
    'Não consigo acessar a internet, como resolver?',
    'Siga estes passos: 1) Verifique se o cabo de rede está conectado ou se o Wi-Fi está ativado. 2) Reinicie o roteador (desligue por 30 segundos e ligue novamente). 3) Execute o comando "ipconfig /renew" no prompt de comando (Windows). 4) Verifique se outros dispositivos conseguem acessar a internet. 5) Se o problema persistir, entre em contato com o suporte.',
    'Rede',
    ARRAY['internet', 'rede', 'conexão', 'wifi', 'cabo', 'conectar', 'acesso'],
    0
),
(
    'Como configurar meu e-mail no celular?',
    'Para Android/iPhone: 1) Abra o app de E-mail. 2) Adicione nova conta. 3) Escolha "Outra conta". 4) Digite seu e-mail completo. 5) Senha: sua senha de e-mail. 6) Servidor de entrada (IMAP): imap.empresa.com, porta 993, SSL ativado. 7) Servidor de saída (SMTP): smtp.empresa.com, porta 587, TLS ativado. 8) Seu nome de usuário é seu e-mail completo.',
    'E-mail',
    ARRAY['email', 'configurar', 'celular', 'smartphone', 'imap', 'smtp', 'mobile'],
    0
),
(
    'A impressora não está imprimindo, o que verificar?',
    'Checklist de impressora: 1) Verifique se há papel na bandeja. 2) Confira se há toner/tinta suficiente. 3) Verifique se há papel preso (atolamento). 4) Confirme se a impressora está ligada e conectada ao computador/rede. 5) Tente reiniciar a impressora. 6) No computador, vá em Dispositivos e Impressoras e defina como padrão. 7) Tente imprimir uma página de teste.',
    'Impressora',
    ARRAY['impressora', 'imprimir', 'papel', 'toner', 'tinta', 'atolamento', 'não imprime'],
    0
),
(
    'Esqueci meu ramal telefônico, como descobrir?',
    'Você pode descobrir seu ramal de 3 formas: 1) Disque *100 no seu telefone para ouvir seu próprio ramal. 2) Consulte a lista telefônica interna na intranet. 3) Entre em contato com o suporte informando seu nome e setor.',
    'Telefonia',
    ARRAY['ramal', 'telefone', 'número', 'descobrir', 'esqueci'],
    0
),
(
    'Como solicitar instalação de um novo software?',
    'Para solicitar software: 1) Abra um chamado informando o nome do software. 2) Justifique a necessidade (para qual trabalho/projeto). 3) Se possível, informe o link para download oficial. 4) O suporte verificará se o software é compatível com as políticas de segurança. 5) Após aprovação, o software será instalado remotamente ou agendada visita técnica.',
    'Software',
    ARRAY['software', 'programa', 'instalar', 'instalação', 'aplicativo', 'solicitar'],
    0
),
(
    'Meu computador está muito lento, o que pode ser?',
    'Causas comuns de lentidão: 1) Muitos programas iniciando com o Windows. 2) Disco rígido cheio (menos de 10% livre). 3) Vírus ou malware. 4) Memória RAM insuficiente. 5) Atualizações pendentes. Soluções: 1) Feche programas desnecessários. 2) Limpe arquivos temporários. 3) Execute antivírus. 4) Reinicie o computador. 5) Se persistir, abra chamado para manutenção.',
    'Hardware',
    ARRAY['lento', 'travando', 'lentidão', 'performance', 'devagar', 'computador'],
    0
),
(
    'Não consigo enviar e-mails com anexos grandes',
    'Limitações de anexo: 1) O tamanho máximo por e-mail é 25MB. 2) Para arquivos maiores, use o serviço de compartilhamento de arquivos da empresa. 3) Você pode comprimir (zipar) os arquivos para reduzir o tamanho. 4) Considere enviar múltiplos e-mails se necessário. 5) Para arquivos muito grandes, solicite acesso ao sistema de transferência de arquivos.',
    'E-mail',
    ARRAY['email', 'anexo', 'grande', 'tamanho', 'enviar', 'arquivo'],
    0
),
(
    'Como acessar o sistema remotamente de casa?',
    'Acesso remoto: 1) Certifique-se de ter VPN instalada (solicite ao suporte se não tiver). 2) Conecte-se à VPN usando suas credenciais da empresa. 3) Após conectado à VPN, acesse os sistemas normalmente. 4) Para acesso à sua máquina do trabalho, use o Remote Desktop. 5) Em caso de problemas, verifique sua conexão de internet e entre em contato com o suporte.',
    'Acesso',
    ARRAY['remoto', 'casa', 'vpn', 'home office', 'distância', 'acesso'],
    0
);


-- 4. CHAMADOS DE EXEMPLO
INSERT INTO chamados (titulo, descricao, categoria, prioridade, data_abertura, solicitante, status) VALUES
(
    'Computador não liga após queda de energia',
    'Ontem houve uma queda de energia no escritório e desde então meu computador não liga mais. Já verifiquei o cabo de energia e a tomada está funcionando.',
    'Hardware',
    'Alta',
    NOW() - INTERVAL '2 days',
    'user@sistema.com',
    'Em análise'
),
(
    'Não consigo acessar a pasta compartilhada',
    'Preciso acessar a pasta \\servidor\documentos mas recebo mensagem de "Acesso negado". Antes eu conseguia acessar normalmente.',
    'Rede',
    'Média',
    NOW() - INTERVAL '1 day',
    'pedro@empresa.com',
    'Aguardando atribuição'
),
(
    'Impressora imprimindo páginas em branco',
    'A impressora do meu setor está imprimindo páginas em branco. Já verifiquei e há papel e toner.',
    'Impressora',
    'Média',
    NOW() - INTERVAL '3 hours',
    'ana@empresa.com',
    'Aguardando atribuição'
);


-- 5. INTERAÇÕES DOS CHAMADOS
INSERT INTO interacoes (chamado_id, data_hora, descricao, usuario) VALUES
(1, NOW() - INTERVAL '2 days', 'Chamado criado', NULL),
(1, NOW() - INTERVAL '1 day', 'Técnico atribuído ao chamado', 'tecnico@sistema.com'),
(1, NOW() - INTERVAL '1 day', 'Realizei testes na fonte de alimentação. Vou substituir a fonte.', 'tecnico@sistema.com'),
(2, NOW() - INTERVAL '1 day', 'Chamado criado', NULL),
(3, NOW() - INTERVAL '3 hours', 'Chamado criado', NULL);


-- 6. CONSENTIMENTOS LGPD
INSERT INTO consentimentos (usuario_email, tipo_consentimento, consentido, data_consentimento) VALUES
('user@sistema.com', 'uso_dados_sistema', true, NOW()),
('pedro@empresa.com', 'uso_dados_sistema', true, NOW()),
('ana@empresa.com', 'uso_dados_sistema', true, NOW());


-- Verificar dados inseridos
SELECT 'Usuários cadastrados:' as info, COUNT(*) as total FROM usuarios;
SELECT 'Categorias cadastradas:' as info, COUNT(*) as total FROM categorias;
SELECT 'FAQs cadastradas:' as info, COUNT(*) as total FROM faqs;
SELECT 'Chamados criados:' as info, COUNT(*) as total FROM chamados;
SELECT 'Interações registradas:' as info, COUNT(*) as total FROM interacoes;


-- IMPORTANTE: ATUALIZAR SENHAS

-- As senhas são exemplos. Para gerar hash BCrypt real

COMMIT;
