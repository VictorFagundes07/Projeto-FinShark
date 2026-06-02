# Relatório do Projeto

O sistema foi desenvolvido em ASP.NET Core MVC, organizado em Models, Controllers, Views e Data. O Entity Framework Core foi configurado com PostgreSQL por meio da connection string `BibliotecaConnection`.

## Funcionalidades desenvolvidas

- CRUD completo de usuários, incluindo status ativo/inativo e login simples.
- CRUD completo de livros, com busca e filtro de livros disponíveis.
- Registro de empréstimos com validações de login, estoque e faixa etária.
- Registro de devolução com atualização do estoque e cálculo de multa.
- Histórico de empréstimos com filtro por usuário.
- Dashboard inicial com totais do sistema.
- Tema escuro responsivo usando Bootstrap e CSS próprio.

## Regras de negócio

O empréstimo só pode ser feito por usuário logado. Antes de registrar o empréstimo, o sistema confere se o livro possui estoque disponível e se a faixa etária do usuário permite o empréstimo. Na confirmação, o estoque é reduzido em uma unidade. Na devolução, o estoque é aumentado em uma unidade e a multa é calculada em R$ 2,00 por dia de atraso.

## Dificuldades

A principal atenção do desenvolvimento foi manter as regras de empréstimo e devolução consistentes com o estoque, além de organizar telas simples para facilitar a apresentação do projeto.
