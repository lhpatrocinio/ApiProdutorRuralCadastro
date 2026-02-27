# 🌱 AgroSolutions - API de Cadastro

Microsserviço responsável pelo **cadastro e gestão** de produtores rurais, propriedades, talhões e culturas da plataforma AgroSolutions.

## Visão Geral

| Item | Detalhe |
|------|---------|
| **Porta padrão** | 5001 |
| **Banco de dados** | AgroCadastro (SQL Server) |
| **Endpoints** | 18 |
| **Testes unitários** | 35 |
| **Autenticação** | JWT Bearer (obrigatório) |

## Responsabilidades

- Gerenciar o cadastro de produtores rurais (PF e PJ)
- Gerenciar propriedades rurais e seus talhões
- Gerenciar culturas plantadas em cada talhão
- Receber eventos `AlertCreatedEvent` via RabbitMQ e atualizar o status dos talhões automaticamente

## Estrutura do Projeto (Clean Architecture)

```
ApiProdutorRuralCadastro/
├── ProdutorRuralCadastro.Domain/         # Entidades, interfaces, regras de domínio
├── ProdutorRuralCadastro.Application/    # Use cases, DTOs, handlers de eventos
├── ProdutorRuralCadastro.Infrastructure/ # EF Core, SQL Server, RabbitMQ Consumer
├── ProdutorRuralCadastro.Api/            # Controllers, middlewares, Swagger
└── ProdutorRuralCadastro.Tests/          # Testes unitários (xUnit + Moq)
```

## Endpoints

### Produtores (`/api/v1/ProdutoresRurais`)

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`    | `/` | Lista todos os produtores |
| `GET`    | `/{id}` | Busca produtor por ID |
| `POST`   | `/` | Cria novo produtor |
| `PUT`    | `/{id}` | Atualiza produtor |
| `DELETE` | `/{id}` | Remove produtor |

### Propriedades (`/api/v1/Propriedades`)

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`    | `/` | Lista todas as propriedades |
| `GET`    | `/{id}` | Busca propriedade por ID |
| `POST`   | `/` | Cria nova propriedade |
| `PUT`    | `/{id}` | Atualiza propriedade |
| `DELETE` | `/{id}` | Remove propriedade |

### Talhões (`/api/v1/Talhoes`)

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`    | `/` | Lista todos os talhões |
| `GET`    | `/{id}` | Busca talhão por ID |
| `POST`   | `/` | Cria novo talhão |
| `PUT`    | `/{id}` | Atualiza talhão |
| `DELETE` | `/{id}` | Remove talhão |

### Culturas (`/api/v1/Culturas`)

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`    | `/` | Lista culturas disponíveis |
| `GET`    | `/{id}` | Busca cultura por ID |
| `POST`   | `/` | Cadastra nova cultura |

## Como Executar Localmente

### Pré-requisitos

- .NET 8 SDK
- SQL Server + RabbitMQ rodando (via Docker — ver [AgroSolutions-Infra](https://github.com/marceloms17/AgroSolutions-Infra))
- Token JWT obtido via [API de Autenticação](https://github.com/lhpatrocinio/ApiProdutorRuralAutenticacao)

### Executar

```powershell
git clone https://github.com/lhpatrocinio/ApiProdutorRuralCadastro.git
cd ApiProdutorRuralCadastro
dotnet restore
dotnet run --project ProdutorRuralCadastro.Api
```

Swagger disponível em: `http://localhost:5001/swagger`

### Executar Testes

```powershell
dotnet test
```

## Mensageria RabbitMQ

Este serviço **consome** o evento `AlertCreatedEvent` publicado pela API de Monitoramento:

| Direção | Exchange | Routing Key | Evento |
|---------|----------|-------------|--------|
| **Consome** | `agro.events` | `alert.created.{talhaoId}` | `AlertCreatedEvent` → atualiza status do talhão |

## Tecnologias

- .NET 8 / ASP.NET Core
- Entity Framework Core 8 + SQL Server
- RabbitMQ (MassTransit) — Consumer
- JWT Bearer Authentication
- Polly (Resilience: Retry, Circuit Breaker)
- Swagger / OpenAPI
- xUnit + Moq + FluentAssertions
- GitHub Actions (CI/CD)