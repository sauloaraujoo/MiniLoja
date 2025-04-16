# Feedback - Avaliação Geral

## Front End
### Navegação

  * Pontos negativos:
    - Não há evidências de um projeto MVC implementado

### Design
    - Será avaliado na entrega final

### Funcionalidade

  * Pontos negativos:
    - Não há evidências de um projeto MVC implementado

## Back End
### Arquitetura
  * Pontos negativos:
    - Não faz sentido algum uma camada "Business" e outra "Domain"
    - Nenhuma das duas camadas são necessárias, apenas um projeto Core englobando Business e Data já atenderia um projeto deste escopo.

### Funcionalidade
  * Pontos positivos:
    - Implementação de API RESTful para operações CRUD, conforme esperado para o back-end.
    - Uso do Entity Framework Core com SQLite, conforme especificação.
    - Documentação da API via Swagger está presente.
    - Identity cria e vincula Vendedor no momento do registro

### Modelagem
  * Pontos positivos:
    - Estrutura de modelagem simples, com entidades e interfaces localizadas na camada `Domain`.

  * Pontos negativos:
    - A distribuição de camadas está muito sem sentido.

## Projeto
### Organização
  * Pontos positivos:
    - O arquivo de solução (`MiniLoja.sln`) está presente na raiz do repositório.

  * Pontos negativos:
    - Não há uso da pasta `src` na raiz, mas a organização por pastas de projeto está adequada.
    
### Documentação
  * Pontos positivos:
    - O repositório possui um arquivo `README.md` bem documentado, com informações do projeto e instruções de execução.
    - O arquivo `FEEDBACK.md` está presente.
    - Documentação automática da API via Swagger.

### Instalação
  * Pontos positivos:
    - Implementação do SQLite como banco de dados.
    - O README indica que o seed de dados e as migrations são aplicados automaticamente ao iniciar a aplicação.
