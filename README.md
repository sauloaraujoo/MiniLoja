# **[Mini Loja] - Aplicação Simples com MVC e API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **[Gestão de Mini Loja Virtual com Cadastro de Produtos e Categorias]**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Introdução ao Desenvolvimento ASP.NET Core**.
Desenvolver uma aplicação web básica usando conceitos do Módulo 1 (C#, ASP.NET Core MVC, SQL, EF Core, APIs REST) para gestão simplificada de produtos e categorias em um formato tipo e-commerce marketplace.

### **Autor(es)**
- **Saulo Araujo**

## **2. Proposta do Projeto**
O projeto inclui:
- **🖥 Aplicação MVC:** Interface web para interação com a mini loja.
- **🌐 API RESTful:** Exposição dos recursos da mini loja para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **🔒 Autenticação e Autorização:** Usuários vendedores podem registrar/login e gerenciar seus próprios produtos.
- **💾 Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**
- **🛠 Linguagem de Programação:** C# (.NET 8)
- **📚 Frameworks:**
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - Entity Framework Core
- **💾 Banco de Dados:** SQLite
- **🔐 Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **📄 Documentação da API:** Swagger

## **4. Estrutura do Projeto**
A estrutura do projeto é organizada da seguinte forma:
```
MiniLoja/
  └── src/
      ├── MiniLoja.Api/         # API RESTful
      ├── MiniLoja.App/         # Aplicação ASP.NET Core MVC
      ├── MiniLoja.Core/
      │   ├── MiniLoja.Business/    # Camada de regras de negócio e validações
      │   ├── MiniLoja.Domain/      # Entidades e interfaces de repositório
      │   └── MiniLoja.Infra.Data/  # EF Core, mapeamentos, contextos
README.md               	# Arquivo de Documentação do Projeto
FEEDBACK.md             	# Arquivo para Consolidação dos Feedbacks
.gitignore              	# Arquivo de Ignoração do Git

```

## **5. Funcionalidades Implementadas**
- **🔑 Autenticação e Autorização:** Com Identity
- **🌍 API RESTful:** Exposição de endpoints para operações CRUD via API.
- **📑 Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**
- .NET SDK 8.0 ou superior
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**
1. **Clone o Repositório:**
   ```bash
   git https://github.com/sauloaraujoo/MiniLoja.git
   cd MiniLoja
   ```

2. **Configuração do Banco de Dados:**
   - O projeto usa SQLite por padrão.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos.

3. **Executar a Aplicação MVC:**
   ```bash
   cd src/MiniLoja.App/
   dotnet run
   ```
   - Acesse a aplicação em: https://localhost:7099

4. **Executar a API:**
   ```bash
   cd src/MiniLoja.Api/
   dotnet run
   ```
   - Acesse a documentação da API em: [http://localhost:7276/swagger](http://localhost:7276/swagger)

## **7. Instruções de Configuração**
- **🔑 JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **⚙️ Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido à configuração do Seed de dados.

## **8. Documentação da API**
A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em: [http://localhost:7276/swagger](http://localhost:7276/swagger)

## **9. Avaliação**
- Este projeto é parte de um curso acadêmico e não aceita contribuições externas.
- Para feedbacks ou dúvidas, utilize o recurso de Issues.
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
