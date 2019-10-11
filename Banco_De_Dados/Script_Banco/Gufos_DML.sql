USE Gufos;

/*** Inserindo os dados nas tabelas do projeto Gufos ***/

-- Tabela Tipo_Usuario
INSERT INTO Tipo_usuario(Titulo) 
VALUES('Administrador'),
('Aluno'); 

-- Tabela Usuario
INSERT INTO Usuario(Nome, Email, Senha, Tipo_usuario_id) 
VALUES('Administrador', 'adm@adm.com', '123', 1),
('Ariel', 'ariel@gmail.com', '123', 2);

-- Tabela Localizacao
INSERT INTO Localizacao(CNPJ, Razao_social, Endereco) 
VALUES('84350569000177', 'Escola SENAI de Informática', 'Alameda Barão de Limeira, 539 - Santa Cecilia, São Paulo - SP, 01202-001');

-- Tabela Categoria
INSERT INTO Categoria(Titulo) 
VALUES('Desenvolvimento de Sistemas'),
('HTML e CSS'),('Marketing'),('Data Sciense');


-- Tabela Evento
INSERT INTO Evento(Titulo, Categoria_id, Acesso_livre, Data_evento, Localizacao_id) 
VALUES('C#', 2, 0, '2019-08-07 18:00:00', 1),
('Estrutura Semântica', 2, 1, GETDATE(), 1);

-- Tabela de Presenca
INSERT INTO Presenca(Evento_id, Usuario_id, Presenca_status) 
VALUES(2, 2, 'AGUARDANDO'),(3, 1, 'CONFIRMADO');

/*** Inserindo os dados nas tabelas do projeto Gufos ***/


SELECT * FROM Evento