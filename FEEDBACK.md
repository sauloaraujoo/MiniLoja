# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC com navegação funcional e views completas para produtos, categorias e autenticação.

  * Pontos negativos:
    - Nenhum.

### Design
  - Interface clara e funcional, adequada ao propósito administrativo da aplicação.

### Funcionalidade
  * Pontos positivos:
    - CRUD completo de produtos e categorias na API e no MVC.
    - Autenticação funcional com Identity (JWT na API, Cookie no MVC).
    - Criação de vendedor associada ao usuário do Identity (compartilhando ID) está corretamente implementada.
    - Migrations automáticas, seed de dados e uso do SQLite funcionais.
    - Arquitetura enxuta e aderente ao escopo do projeto.

  * Pontos negativos:
    - `Vendedor` faz composição com `IdentityUser`, o que causa um acoplamento indesejado e corrompe a responsabilidade da implementação.

## Back End

### Arquitetura
  * Pontos positivos:
    - Estrutura com três camadas bem definidas: API, App, Core.
    - Boas práticas na configuração de serviços, autenticação e rotas.

  * Pontos negativos:
    - Idealmente, `Vendedor` deveria ser uma entidade independente com o mesmo ID do usuário do Identity.

### Funcionalidade
  * Pontos positivos:
    - Todas as funcionalidades principais implementadas de forma robusta.

  * Pontos negativos:
    - Nenhum.

### Modelagem
  * Pontos positivos:
    - Entidades bem estruturadas, coerentes e com relações de domínio claras.

## Projeto

### Organização
  * Pontos positivos:
    - Uso correto da pasta `src`, `.sln` na raiz, separação adequada por camadas.
    - `README.md` e `FEEDBACK.md` presentes.
    - Organização dos arquivos coerente.

  * Pontos negativos:
    - Nenhum.

### Documentação
  * Pontos positivos:
    - Documentação clara e padronizada.
    - Swagger configurado na API.

  * Pontos negativos:
    - Nenhum.

### Instalação
  * Pontos positivos:
    - Execução simples com SQLite e migrations automáticas.
    - Banco populado com seed de dados no startup.

  * Pontos negativos:
    - Nenhum.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 10       | 3,0                                      |
| **Qualidade do Código**       | 20%      | 10       | 2,0                                      |
| **Eficiência e Desempenho**   | 20%      | 9        | 1,8                                      |
| **Inovação e Diferenciais**   | 10%      | 9        | 0,9                                      |
| **Documentação e Organização**| 10%      | 10       | 1,0                                      |
| **Resolução de Feedbacks**    | 10%      | 10       | 1,0                                      |
| **Total**                     | 100%     | -        | **9,7**                                  |

## 🎯 **Nota Final: 9,7 / 10**
