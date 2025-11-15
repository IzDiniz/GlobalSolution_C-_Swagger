# Membros


Iago Diniz Fontes RM 553776
<br>
Lugas Garcia RM 554070
<br>
Pedro Henrique Chaves RM 553988

<br> <br>
### Vídeo Explicativo: https://youtu.be/EYOK7faC_EM


# Air Quality API - Monitoramento de Qualidade do Ar

API RESTful desenvolvida em **C# .NET 8** para o desafio "O Futuro do Trabalho" da FIAP. A solução permite o cadastro e monitoramento de medições de qualidade do ar em diferentes localidades, seguindo as melhores práticas de desenvolvimento de software.

## 🚀 Sobre o Projeto

O objetivo deste projeto é fornecer uma solução tecnológica para monitorar a qualidade do ar, um tema diretamente ligado à produtividade e bem-estar no ambiente de trabalho. A API permite que dispositivos, como um Arduino, enviem dados de sensores (PM2.5, PM10, CO2) que são armazenados, processados e disponibilizados através de endpoints REST.

### Funcionalidades Principais

- **Cadastro de Medições**: Armazena dados de poluição por localização.
- **Categorização Automática**: Classifica a qualidade do ar em "Boa", "Moderada", "Ruim" ou "Péssima" com base nos níveis de PM2.5.
- **Endpoints REST Completos**: Implementação dos verbos HTTP (GET, POST, PUT, DELETE) para manipulação dos dados.
- **Persistência de Dados**: Utiliza o Entity Framework Core para integração com banco de dados (SQL Server ou em memória).
- **Documentação Interativa**: Interface Swagger para explorar e testar a API de forma intuitiva.
- **Versionamento**: A API é versionada para garantir compatibilidade e facilitar futuras atualizações (ex: `/api/v1/...`).

## 🛠️ Tecnologias Utilizadas

- **.NET 8**: Plataforma de desenvolvimento da Microsoft.
- **ASP.NET Core**: Framework para construção de aplicações web e APIs.
- **Entity Framework Core**: ORM para interação com o banco de dados.
- **SQL Server**: Banco de dados relacional (configurado para produção).
- **Swagger (Swashbuckle)**: Ferramenta para documentação de APIs.
- **C#**: Linguagem de programação principal.

## ⚙️ Como Executar o Projeto

Siga os passos abaixo para configurar e executar a aplicação em seu ambiente local.

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Um editor de código de sua preferência (Visual Studio, VS Code, Rider).
- Git para clonar o repositório.

### Instalação

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/AirQualityAPI.git
   cd AirQualityAPI
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Configure o Banco de Dados:**
   - **Para desenvolvimento (em memória):** Nenhuma configuração adicional é necessária. A API usará um banco de dados em memória por padrão.
   - **Para produção (SQL Server):**
     - Abra o arquivo `appsettings.json`.
     - Modifique a `ConnectionStrings:DefaultConnection` com os dados do seu servidor SQL Server.
     - No arquivo `Program.cs`, comente a linha `options.UseInMemoryDatabase("AirQualityDB");` e descomente as linhas referentes ao `UseSqlServer`.

4. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

A API estará disponível em `https://localhost:5001` (HTTPS) e `http://localhost:5000` (HTTP). A documentação Swagger pode ser acessada na raiz da aplicação (`https://localhost:5001`).

## 📖 Endpoints da API

A API está versionada sob o prefixo `/api/v1`.

| Verbo  | Endpoint                               | Descrição                                         |
|--------|----------------------------------------|-----------------------------------------------------|
| `GET`    | `/api/v1/AirQuality`                   | Obtém todas as leituras de qualidade do ar.         |
| `GET`    | `/api/v1/AirQuality/{id}`              | Obtém uma leitura específica por ID.                |
| `POST`   | `/api/v1/AirQuality`                   | Cria uma nova leitura de qualidade do ar.           |
| `PUT`    | `/api/v1/AirQuality/{id}`              | Atualiza uma leitura existente.                     |
| `DELETE` | `/api/v1/AirQuality/{id}`              | Exclui uma leitura.                                 |
| `GET`    | `/api/v1/AirQuality/location/{location}` | Filtra leituras por localização.                    |
| `GET`    | `/api/v1/AirQuality/category/{category}` | Filtra leituras por categoria (Boa, Moderada, etc). |
| `GET`    | `/api/v1/AirQuality/statistics`        | Obtém estatísticas gerais sobre as medições.        |

### Exemplo de Requisição (POST)

**Endpoint:** `POST /api/v1/AirQuality`

**Corpo da Requisição (JSON):**
```json
{
  "location": "São Paulo - Pinheiros",
  "pm25": 25.5,
  "pm10": 45.2,
  "co2": 410,
  "description": "Medição próxima ao parque."
}
```

### Exemplo de Resposta (201 Created)

```json
{
  "id": 4,
  "location": "São Paulo - Pinheiros",
  "pm25": 25.5,
  "pm10": 45.2,
  "co2": 410,
  "category": "Moderada",
  "measurementDate": "2025-11-15T23:10:00.000Z",
  "description": "Medição próxima ao parque."
}
```

## 🏗️ Arquitetura

O fluxo da arquitetura foi desenhado para ser simples e escalável, seguindo os princípios de uma API RESTful. O diagrama abaixo ilustra os principais componentes da solução.

![Arquitetura da API](arquitetura.png)

## 🤝 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir *issues* e *pull requests* para melhorar o projeto.

---

*Projeto desenvolvido como parte da avaliação da disciplina de Desenvolvimento de Software da FIAP.*

