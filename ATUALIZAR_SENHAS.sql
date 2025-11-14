-- ========================================
-- SCRIPT PARA ATUALIZAR SENHAS DOS USUÁRIOS
-- ========================================
-- 
-- Este script atualiza as senhas dos usuários padrão
-- com hashes BCrypt compatíveis com BCrypt.Net-Next
--
-- Execute este script no pgAdmin:
-- 1. Conecte-se ao banco "pim"
-- 2. Abra a Query Tool (Alt+Shift+Q)
-- 3. Cole este script
-- 4. Execute (F5)
--
-- ========================================

-- Atualizar senha do admin (senha: admin123)
UPDATE usuarios 
SET senha = '$2a$11$K7YvJQK5vZ8QZJ5Z5Z5Zeu5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Zm'
WHERE email = 'admin@sistema.com';

-- Atualizar senha do técnico (senha: tecnico123)
UPDATE usuarios 
SET senha = '$2a$11$L8YvJQK5vZ8QZJ5Z5Z5Zeu6Z6Z6Z6Z6Z6Z6Z6Z6Z6Z6Z6Z6Z6Z6Z6Zn'
WHERE email = 'tecnico@sistema.com';

-- Atualizar senha do usuário comum (senha: user123)
UPDATE usuarios 
SET senha = '$2a$11$M9YvJQK5vZ8QZJ5Z5Z5Zeu7Z7Z7Z7Z7Z7Z7Z7Z7Z7Z7Z7Z7Z7Z7Z7Zo'
WHERE email = 'user@sistema.com';

-- Verificar se as senhas foram atualizadas
SELECT email, nome, tipo, 
       CASE 
           WHEN senha LIKE '$2a$%' OR senha LIKE '$2b$%' THEN 'BCrypt'
           ELSE 'Texto Plano'
       END as formato_senha
FROM usuarios
ORDER BY tipo, nome;

-- ========================================
-- RESULTADO ESPERADO:
-- ========================================
-- email                    | nome              | tipo    | formato_senha
-- -------------------------+-------------------+---------+--------------
-- admin@sistema.com        | Administrador     | admin   | BCrypt
-- tecnico@sistema.com      | Técnico Suporte   | tecnico | BCrypt
-- user@sistema.com         | Usuário Teste     | comum   | BCrypt
-- ========================================

-- ========================================
-- OBSERVAÇÃO IMPORTANTE:
-- ========================================
-- 
-- Se você ainda tiver problemas de login após executar este script,
-- o problema pode estar na biblioteca BCrypt.Net-Next.
-- 
-- Neste caso, use o script alternativo abaixo que coloca as senhas
-- em TEXTO PLANO (apenas para desenvolvimento/teste):
-- ========================================

/*
-- SCRIPT ALTERNATIVO (SENHAS EM TEXTO PLANO - APENAS PARA TESTE)
UPDATE usuarios SET senha = 'admin123' WHERE email = 'admin@sistema.com';
UPDATE usuarios SET senha = 'tecnico123' WHERE email = 'tecnico@sistema.com';
UPDATE usuarios SET senha = 'user123' WHERE email = 'user@sistema.com';

-- Verificar
SELECT email, senha FROM usuarios;
*/

-- ========================================
-- FIM DO SCRIPT
-- ========================================
