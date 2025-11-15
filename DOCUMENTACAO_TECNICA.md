# Documentação Técnica - Air Quality API

## Visão Geral do Projeto

A **Air Quality API** é uma solução desenvolvida em **C# .NET 8** para atender aos requisitos do desafio "O Futuro do Trabalho" da FIAP. O sistema permite o monitoramento da qualidade do ar através de medições de poluentes como PM2.5, PM10 e CO2, armazenando os dados em um banco de dados e disponibilizando-os através de uma API RESTful completa.

## Arquitetura da Solução

A arquitetura segue o padrão **MVC (Model-View-Controller)** adaptado para APIs, com separação clara de responsabilidades entre camadas.

### Componentes Principais

**Camada de Apresentação (Controllers)**

A camada de apresentação é responsável por receber as requisições HTTP, validar os dados de entrada e retornar as respostas adequadas. O controller principal `AirQualityController` implementa todos os endpoints REST necessários.

**Camada de Domínio (Models)**

Os modelos representam as entidades de negócio da aplicação. A entidade principal `AirQualityReading` contém as propriedades necessárias para armazenar uma medição de qualidade do ar, incluindo localização, valores dos poluentes, categoria calculada e data da medição.

**Camada de Dados (Data)**

Utiliza o **Entity Framework Core** como ORM para abstração do acesso ao banco de dados. O contexto `AirQualityContext` gerencia as operações de persistência e inclui dados iniciais para facilitar os testes.

**Camada de Transferência (DTOs)**

Os Data Transfer Objects (DTOs) são utilizados para desacoplar a representação interna dos dados da API pública. Isso permite maior flexibilidade para evoluir o modelo interno sem quebrar contratos com clientes.

## Modelo de Dados

### Entidade AirQualityReading

| Campo | Tipo | Descrição | Validação |
|-------|------|-----------|-----------|
| `Id` | int | Identificador único (chave primária) | Gerado automaticamente |
| `Location` | string | Local da medição (ex: "Rio de Janeiro") | Obrigatório, 2-200 caracteres |
| `PM25` | double | Material Particulado 2.5 em µg/m³ | Obrigatório, 0-1000 |
| `PM10` | double | Material Particulado 10 em µg/m³ | 0-1000 |
| `CO2` | double | Nível de CO2 em ppm | 0-10000 |
| `Category` | string | Categoria calculada automaticamente | "Boa", "Moderada", "Ruim", "Péssima" |
| `MeasurementDate` | DateTime | Data e hora da medição | Obrigatório |
| `Description` | string | Observações adicionais | Opcional, até 500 caracteres |

### Lógica de Categorização

A categorização da qualidade do ar é baseada nos padrões da **EPA (US Environmental Protection Agency)** para PM2.5:

- **Boa**: PM2.5 ≤ 12.0 µg/m³
- **Moderada**: 12.0 < PM2.5 ≤ 35.4 µg/m³
- **Ruim**: 35.4 < PM2.5 ≤ 55.4 µg/m³
- **Péssima**: PM2.5 > 55.4 µg/m³

Esta categorização é calculada automaticamente através do método `CalculateCategory()` sempre que uma medição é criada ou atualizada.

## Endpoints da API

### Listar Todas as Leituras

**Endpoint:** `GET /api/v1/AirQuality`

Retorna todas as leituras de qualidade do ar ordenadas por data de medição (mais recentes primeiro).

**Resposta de Sucesso (200 OK):**
```json
[
  {
    "id": 1,
    "location": "São Paulo - Centro",
    "pm25": 45.5,
    "pm10": 68.3,
    "co2": 450,
    "category": "Ruim",
    "measurementDate": "2025-11-15T01:00:00Z",
    "description": "Medição realizada no centro da cidade"
  }
]
```

### Obter Leitura por ID

**Endpoint:** `GET /api/v1/AirQuality/{id}`

Retorna uma leitura específica identificada pelo ID.

**Parâmetros:**
- `id` (path): Identificador da leitura

**Respostas:**
- `200 OK`: Leitura encontrada
- `404 Not Found`: Leitura não encontrada

### Criar Nova Leitura

**Endpoint:** `POST /api/v1/AirQuality`

Cria uma nova leitura de qualidade do ar. A categoria é calculada automaticamente com base no valor de PM2.5.

**Corpo da Requisição:**
```json
{
  "location": "Rio de Janeiro - Zona Sul",
  "pm25": 65.2,
  "pm10": 95.7,
  "co2": 520,
  "description": "Poluição exagerada devido ao tráfego intenso"
}
```

**Resposta de Sucesso (201 Created):**
```json
{
  "id": 4,
  "location": "Rio de Janeiro - Zona Sul",
  "pm25": 65.2,
  "pm10": 95.7,
  "co2": 520,
  "category": "Péssima",
  "measurementDate": "2025-11-15T03:00:00Z",
  "description": "Poluição exagerada devido ao tráfego intenso"
}
```

### Atualizar Leitura

**Endpoint:** `PUT /api/v1/AirQuality/{id}`

Atualiza uma leitura existente. Apenas os campos fornecidos serão atualizados. A categoria é recalculada automaticamente.

**Parâmetros:**
- `id` (path): Identificador da leitura

**Corpo da Requisição:**
```json
{
  "pm25": 30.0,
  "description": "Medição corrigida"
}
```

**Respostas:**
- `200 OK`: Leitura atualizada com sucesso
- `400 Bad Request`: Dados inválidos
- `404 Not Found`: Leitura não encontrada

### Excluir Leitura

**Endpoint:** `DELETE /api/v1/AirQuality/{id}`

Remove uma leitura do banco de dados.

**Parâmetros:**
- `id` (path): Identificador da leitura

**Respostas:**
- `204 No Content`: Leitura excluída com sucesso
- `404 Not Found`: Leitura não encontrada

### Filtrar por Localização

**Endpoint:** `GET /api/v1/AirQuality/location/{location}`

Retorna todas as leituras que contenham o texto especificado na localização.

**Parâmetros:**
- `location` (path): Texto para busca na localização

**Exemplo:** `GET /api/v1/AirQuality/location/Rio`

### Filtrar por Categoria

**Endpoint:** `GET /api/v1/AirQuality/category/{category}`

Retorna todas as leituras de uma categoria específica.

**Parâmetros:**
- `category` (path): Categoria ("Boa", "Moderada", "Ruim", "Péssima")

**Exemplo:** `GET /api/v1/AirQuality/category/Péssima`

### Obter Estatísticas

**Endpoint:** `GET /api/v1/AirQuality/statistics`

Retorna estatísticas agregadas sobre as medições.

**Resposta de Sucesso (200 OK):**
```json
{
  "totalReadings": 3,
  "averagePM25": 39.73,
  "averagePM10": 59.73,
  "averageCO2": 450.0,
  "categoryDistribution": [
    { "category": "Boa", "count": 1 },
    { "category": "Ruim", "count": 1 },
    { "category": "Péssima", "count": 1 }
  ],
  "worstLocations": [
    {
      "location": "Rio de Janeiro - Zona Sul",
      "count": 1,
      "avgPM25": 65.2
    }
  ]
}
```

## Configuração do Banco de Dados

### Banco de Dados em Memória (Desenvolvimento)

Por padrão, a aplicação utiliza um banco de dados em memória para facilitar o desenvolvimento e testes. Esta configuração está ativa no arquivo `Program.cs`:

```csharp
builder.Services.AddDbContext<AirQualityContext>(options =>
{
    options.UseInMemoryDatabase("AirQualityDB");
});
```

### SQL Server (Produção)

Para utilizar o SQL Server em produção, siga os passos abaixo:

**Passo 1:** Configure a connection string no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu_servidor;Database=AirQualityDB;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
  }
}
```

**Passo 2:** Modifique o arquivo `Program.cs` para usar SQL Server:

```csharp
builder.Services.AddDbContext<AirQualityContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
```

**Passo 3:** Execute as migrations para criar o banco de dados:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Boas Práticas Implementadas

O projeto segue diversas boas práticas de desenvolvimento de software, garantindo qualidade, manutenibilidade e escalabilidade.

### Status Codes HTTP Adequados

Todos os endpoints retornam os códigos de status HTTP apropriados para cada situação, facilitando a integração e o tratamento de erros pelos clientes da API.

### Versionamento da API

A API é versionada através do prefixo `/api/v1/` nas rotas. Isso permite a evolução da API sem quebrar a compatibilidade com clientes existentes.

### Validação de Dados

Todas as entradas são validadas utilizando Data Annotations e validação de modelo do ASP.NET Core. Erros de validação retornam respostas detalhadas com código 400 (Bad Request).

### Separação de Responsabilidades

A aplicação segue o princípio de separação de responsabilidades, com camadas distintas para apresentação, lógica de negócio e acesso a dados.

### Logging

O sistema utiliza o ILogger do ASP.NET Core para registrar operações importantes, facilitando a depuração e o monitoramento em produção.

### Documentação Interativa

A documentação Swagger é gerada automaticamente a partir dos comentários XML no código, garantindo que a documentação esteja sempre atualizada com a implementação.

## Requisitos Atendidos (FIAP)

### 1. Boas Práticas REST (30 pts)

✅ Implementação de status codes adequados (200, 201, 204, 400, 404)
✅ Uso correto dos verbos HTTP (GET, POST, PUT, DELETE)

### 2. Versionamento da API (10 pts)

✅ Estruturação das versões da API com controle adequado em rotas (`/api/v1/`)
✅ Explicação no README sobre o versionamento

### 3. Integração e Persistência (30 pts)

✅ Integração com banco de dados relacional (SQL Server) usando Entity Framework Core
✅ Utilização de Migrations para gerenciamento do schema
✅ Suporte para banco de dados em memória para desenvolvimento

### 4. Documentação (30 pts)

✅ Implementação do fluxo de arquitetura (diagrama Mermaid)
✅ Documentação Swagger das APIs com comentários XML
✅ Link do vídeo demonstrativo (a ser adicionado)

---

**Desenvolvido por:** Equipe de Desenvolvimento  
**Disciplina:** Desenvolvimento de Software - FIAP  
**Data:** Novembro de 2025
