# StockControll

Este projeto consiste em um sistema de controle de estoque desenvolvido em C# para atender às necessidades de gerenciamento de uma sapataria. O sistema foi projetado para auxiliar na gestão do inventário de produtos, otimizando o controle de estoque e facilitando as operações diárias da sapataria.

## Recursos Principais
Cadastro de Produtos: Os administradores têm a capacidade de cadastrar novos produtos, especificando informações como nome do sapato, descrição, tamanho, preço e quantidade disponível.

Atualização de Estoque: Os funcionários da sapataria podem atualizar facilmente a quantidade de produtos em estoque conforme as vendas e o recebimento de novos produtos. O sistema realiza ajustes automáticos nas quantidades disponíveis.

Consulta de Estoque: Os usuários podem verificar rapidamente a quantidade disponível de um produto específico, simplificando a resposta a perguntas dos clientes sobre a disponibilidade de produtos.

Histórico de Movimentações: O sistema mantém um registro completo de todas as movimentações de estoque, incluindo vendas, compras e ajustes, proporcionando uma visão abrangente das atividades.

Relatórios de Estoque: São gerados relatórios detalhados que oferecem informações essenciais sobre os produtos em estoque, os produtos mais vendidos, bem como aqueles com baixa disponibilidade, entre outros.

Alertas de Estoque Baixo: O sistema é capaz de enviar alertas automáticos quando os níveis de estoque atingem um limite mínimo, permitindo que a sapataria tome medidas proativas para repor os produtos.

Autenticação e Autorização: O acesso ao sistema é protegido por meio de autenticação de usuário e níveis de autorização diferentes, garantindo que apenas os usuários autorizados possam executar operações de gerenciamento.

Interface de Usuário Intuitiva: A interface de usuário foi projetada para ser amigável, permitindo que os usuários naveguem pelo sistema de maneira intuitiva e eficaz.

## Tecnologias Utilizadas
Linguagem de Programação: C# <br />
Ambiente de Desenvolvimento: Visual Studio <br />
Banco de Dados: SQL Server <br />
Interface do Usuário: Windows Forms ou WPF <br />
Mapeamento Objeto-Relacional (ORM): Entity Framework <br />
Autenticação e Autorização: ASP.NET Identity <br />

## Objetivo
O objetivo deste projeto é melhorar a eficiência operacional da sapataria, proporcionando um controle preciso e automatizado do estoque. Isso ajuda a evitar a falta de produtos, otimiza os pedidos de reposição e aprimora a experiência do cliente ao garantir que os produtos desejados estejam disponíveis quando necessário.

# Comandos técnicos

Comando para rodar o banco de testes em docker-compose: <br />
``Docker-compose up -d``

Comando para permitir migrations: <br />
``Enable-Migrations``

Comando para rodar migrations: <br />
``Add-Migration <MIGRATION_NAME>``

Comando para atualizar o banco: <br />
``Update-Database``

Comando para rodar rollback de migrations: <br />
``Update-Database -TargetMigration:"NAME_OF_TARGET_MIGRATION"``
