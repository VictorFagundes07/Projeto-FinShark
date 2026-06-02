# Sistema de Biblioteca - ASP.NET Core MVC

Projeto individual de CRUD em .NET MVC para gerenciamento de usuários, livros, empréstimos e devoluções.

## Tecnologias

- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core
- PostgreSQL
- HTML, CSS e Bootstrap

## Como executar

1. Crie o banco no PostgreSQL usando `Sql/script_banco_postgresql.sql`.
2. Ajuste usuário e senha em `appsettings.json`, se necessário.
3. Execute o projeto com `dotnet run`.

Login inicial:

- E-mail: `admin@biblioteca.com`
- Senha: `1234`

## Regras implementadas

- Somente usuário logado registra empréstimo.
- Livro 18+ exige usuário maior de idade.
- Empréstimo não é permitido quando o estoque é zero.
- O estoque diminui no empréstimo e aumenta na devolução.
- A multa é de R$ 2,00 por dia de atraso.
- Campos obrigatórios são validados.
- A quantidade em estoque não pode ser negativa.
