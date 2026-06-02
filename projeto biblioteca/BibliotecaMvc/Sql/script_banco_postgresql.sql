CREATE DATABASE biblioteca_db;

-- Execute os comandos abaixo dentro do banco biblioteca_db.
CREATE TABLE "Usuarios" (
    "Id" SERIAL PRIMARY KEY,
    "NomeCompleto" VARCHAR(120) NOT NULL,
    "DataNascimento" TIMESTAMP NOT NULL,
    "Email" VARCHAR(120) NOT NULL UNIQUE,
    "Senha" VARCHAR(80) NOT NULL,
    "Status" INTEGER NOT NULL
);

CREATE TABLE "Livros" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(160) NOT NULL,
    "Autor" VARCHAR(120) NOT NULL,
    "QuantidadeEstoque" INTEGER NOT NULL DEFAULT 0 CHECK ("QuantidadeEstoque" >= 0),
    "FaixaEtariaPermitida" INTEGER NOT NULL,
    "Categoria" VARCHAR(80) NOT NULL,
    "AnoPublicacao" INTEGER NOT NULL
);

CREATE TABLE "Emprestimos" (
    "Id" SERIAL PRIMARY KEY,
    "DataEmprestimo" TIMESTAMP NOT NULL,
    "UsuarioId" INTEGER NOT NULL REFERENCES "Usuarios"("Id") ON DELETE RESTRICT,
    "LivroId" INTEGER NOT NULL REFERENCES "Livros"("Id") ON DELETE RESTRICT,
    "DataPrevistaDevolucao" TIMESTAMP NOT NULL,
    "DataRealDevolucao" TIMESTAMP NULL,
    "Multa" NUMERIC(10,2) NOT NULL DEFAULT 0,
    "Status" INTEGER NOT NULL
);

INSERT INTO "Usuarios" ("NomeCompleto", "DataNascimento", "Email", "Senha", "Status") VALUES
('Administrador Biblioteca', '1990-01-01', 'admin@biblioteca.com', '1234', 1);

INSERT INTO "Livros" ("Nome", "Autor", "QuantidadeEstoque", "FaixaEtariaPermitida", "Categoria", "AnoPublicacao") VALUES
('Dom Casmurro', 'Machado de Assis', 4, 12, 'Romance', 1899),
('Clean Code', 'Robert C. Martin', 2, 14, 'Tecnologia', 2008),
('It: A Coisa', 'Stephen King', 1, 18, 'Terror', 1986);
