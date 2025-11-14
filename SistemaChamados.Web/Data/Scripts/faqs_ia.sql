-- Script para popular FAQs para o sistema de IA


-- Inserir FAQs comuns de suporte técnico
INSERT INTO faqs (pergunta, resposta, categoria, relevancia, criado_em, atualizado_em) VALUES

-- Hardware
('Computador não liga', 'Verifique se o cabo de energia está conectado corretamente. Teste a tomada com outro equipamento. Verifique se o botão de liga/desliga da fonte está ativado. Se o problema persistir, pode ser necessário trocar a fonte de alimentação.', 'Hardware', 'Alta', NOW(), NOW()),

('Tela azul no Windows (BSOD)', 'A tela azul geralmente indica problema de hardware ou driver. Anote o código de erro exibido. Reinicie o computador em modo de segurança. Atualize os drivers de vídeo e chipset. Execute o verificador de memória do Windows (mdsched.exe).', 'Hardware', 'Alta', NOW(), NOW()),

('Mouse não funciona', 'Se for mouse USB, teste em outra porta USB. Verifique se o cabo não está danificado. Para mouse sem fio, troque as pilhas e verifique se o receptor está conectado. Teste o mouse em outro computador para confirmar se o problema é no mouse ou no PC.', 'Hardware', 'Média', NOW(), NOW()),

('Teclado digitando caracteres errados', 'Verifique se o layout do teclado está configurado corretamente (ABNT2 para português). Acesse Configurações > Hora e Idioma > Idioma > Teclado. Reinicie o computador. Se persistir, teste o teclado em outro computador.', 'Hardware', 'Média', NOW(), NOW()),

('Computador muito lento', 'Verifique o uso de CPU e memória no Gerenciador de Tarefas (Ctrl+Shift+Esc). Desative programas que iniciam com o Windows. Execute limpeza de disco (cleanmgr). Verifique se há vírus com o antivírus. Considere adicionar mais memória RAM se o uso estiver sempre alto.', 'Hardware', 'Alta', NOW(), NOW()),

('Impressora não imprime', 'Verifique se a impressora está ligada e conectada ao computador. Confirme se há papel e tinta/toner. Verifique a fila de impressão e cancele trabalhos travados. Reinicie o serviço de spooler de impressão. Reinstale o driver da impressora.', 'Impressora', 'Alta', NOW(), NOW()),

('Notebook não carrega bateria', 'Verifique se o carregador está conectado corretamente. Teste o carregador em outra tomada. Remova a bateria e conecte apenas o carregador para testar. Se funcionar, a bateria pode estar com defeito. Verifique se o LED de carga acende.', 'Hardware', 'Média', NOW(), NOW()),

-- Software
('Não consigo instalar programa', 'Verifique se você tem permissões de administrador. Clique com botão direito no instalador e selecione "Executar como administrador". Desative temporariamente o antivírus. Verifique se há espaço suficiente em disco. Baixe novamente o instalador caso esteja corrompido.', 'Software', 'Média', NOW(), NOW()),

('Programa trava ou fecha sozinho', 'Atualize o programa para a versão mais recente. Verifique se há atualizações do Windows pendentes. Execute o programa como administrador. Desinstale e reinstale o programa. Verifique os logs de eventos do Windows para identificar o erro.', 'Software', 'Média', NOW(), NOW()),

('Excel não abre arquivo', 'Verifique se o arquivo não está corrompido. Tente abrir em modo de segurança (mantenha Ctrl ao abrir o Excel). Desative complementos do Excel. Repare a instalação do Office através do Painel de Controle. Verifique se a extensão do arquivo está correta (.xlsx, .xls).', 'Software', 'Média', NOW(), NOW()),

('Erro ao abrir PDF', 'Atualize o Adobe Reader para a versão mais recente. Tente abrir o PDF em outro visualizador (navegador, Edge). Verifique se o arquivo não está corrompido pedindo para reenviar. Repare a instalação do Adobe Reader. Desative o modo protegido temporariamente.', 'Software', 'Baixa', NOW(), NOW()),

('Windows Update não funciona', 'Execute o solucionador de problemas do Windows Update (Configurações > Atualização e Segurança > Solucionar problemas). Reinicie o serviço Windows Update. Limpe a pasta SoftwareDistribution. Execute os comandos: sfc /scannow e DISM /Online /Cleanup-Image /RestoreHealth.', 'Software', 'Alta', NOW(), NOW()),

-- Rede
('Sem conexão com a internet', 'Verifique se o cabo de rede está conectado (luz verde piscando). Para Wi-Fi, verifique se está conectado à rede correta. Reinicie o roteador e o computador. Execute o solucionador de problemas de rede do Windows. Verifique se o adaptador de rede está habilitado.', 'Rede', 'Alta', NOW(), NOW()),

('Wi-Fi lento', 'Aproxime-se do roteador para testar. Verifique quantos dispositivos estão conectados. Reinicie o roteador. Altere o canal do Wi-Fi nas configurações do roteador. Atualize o driver da placa de rede. Verifique se não há interferência de outros equipamentos.', 'Rede', 'Média', NOW(), NOW()),

('Não consigo acessar site específico', 'Limpe o cache do navegador (Ctrl+Shift+Delete). Tente em modo anônimo. Teste em outro navegador. Limpe o cache DNS (ipconfig /flushdns no prompt). Verifique se o site está fora do ar em downdetector.com. Desative temporariamente extensões do navegador.', 'Rede', 'Média', NOW(), NOW()),

('VPN não conecta', 'Verifique suas credenciais de acesso. Confirme se está conectado à internet. Reinicie o serviço de VPN. Verifique se o firewall não está bloqueando a VPN. Teste com outra rede (4G do celular). Entre em contato com o suporte de TI para verificar configurações.', 'Rede', 'Alta', NOW(), NOW()),

-- E-mail
('Não recebo e-mails', 'Verifique a caixa de spam/lixo eletrônico. Confirme se a caixa de entrada não está cheia (limite de armazenamento). Verifique as regras de e-mail que podem estar movendo mensagens. Teste enviando e-mail para você mesmo. Verifique configurações de encaminhamento.', 'E-mail', 'Alta', NOW(), NOW()),

('Não consigo enviar e-mails', 'Verifique se o tamanho dos anexos não excede o limite (geralmente 25MB). Confirme as configurações do servidor SMTP. Verifique se sua conta não foi bloqueada por spam. Teste sem anexos. Verifique se há erro de autenticação.', 'E-mail', 'Alta', NOW(), NOW()),

('Outlook pede senha constantemente', 'Reconfigure a conta removendo e adicionando novamente. Limpe as credenciais salvas do Windows (Painel de Controle > Credenciais). Desative autenticação de dois fatores temporariamente. Crie uma senha de aplicativo se usar Gmail/Office365. Repare o perfil do Outlook.', 'E-mail', 'Média', NOW(), NOW()),

-- Acesso
('Esqueci minha senha', 'Use a opção "Esqueci minha senha" na tela de login. Verifique seu e-mail de recuperação. Entre em contato com o suporte de TI para resetar a senha. Verifique se o Caps Lock não está ativado. Tente usar a senha antiga se foi alterada recentemente.', 'Acesso', 'Alta', NOW(), NOW()),

('Conta bloqueada após várias tentativas', 'Aguarde 30 minutos para desbloqueio automático. Entre em contato com o suporte de TI para desbloquear imediatamente. Verifique se não há script ou programa tentando fazer login automaticamente. Altere sua senha após desbloquear.', 'Acesso', 'Alta', NOW(), NOW()),

('Não consigo acessar pasta compartilhada', 'Verifique se você tem permissão de acesso. Confirme se está conectado à rede da empresa. Tente acessar usando o caminho completo (\\\\servidor\\pasta). Verifique se o serviço de compartilhamento está ativo. Entre em contato com TI para verificar permissões.', 'Acesso', 'Média', NOW(), NOW()),

-- Telefonia
('Ramal não funciona', 'Verifique se o cabo está conectado corretamente. Teste o cabo em outro ramal. Verifique se há tom de discagem. Reinicie o telefone IP desconectando e conectando novamente. Entre em contato com suporte de telefonia se o problema persistir.', 'Telefonia', 'Média', NOW(), NOW()),

('Não consigo fazer ligações externas', 'Verifique se está discando o prefixo correto (geralmente 0). Confirme se seu ramal tem permissão para ligações externas. Teste fazer ligação interna primeiro. Entre em contato com o suporte de telefonia para verificar configurações.', 'Telefonia', 'Média', NOW(), NOW()),

-- Outros
('Preciso instalar software específico', 'Verifique se o software está na lista de programas aprovados pela empresa. Abra um chamado solicitando a instalação e justificando a necessidade. Aguarde aprovação do gestor. O TI fará a instalação remotamente ou agendará visita.', 'Outros', 'Baixa', NOW(), NOW()),

('Backup de arquivos', 'Salve arquivos importantes no OneDrive ou servidor da empresa. Configure backup automático. Não salve arquivos importantes apenas no desktop. Verifique regularmente se o backup está funcionando. Entre em contato com TI para restaurar arquivos de backup.', 'Outros', 'Média', NOW(), NOW()),

('Solicitar novo equipamento', 'Abra chamado descrevendo a necessidade. Justifique a solicitação (equipamento quebrado, insuficiente, etc). Aguarde aprovação do gestor. O TI entrará em contato para agendar entrega/instalação. Devolva equipamento antigo se aplicável.', 'Outros', 'Baixa', NOW(), NOW());

-- Atualizar contador de FAQs
SELECT COUNT(*) as total_faqs FROM faqs;
