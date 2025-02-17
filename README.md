# Poc-DDD

Esta aplicação é uma implementação de exemplo de uma arquitetura baseada em Domain-Driven Design (DDD). Ela segue as práticas recomendadas para criar sistemas escaláveis e de fácil manutenção. A arquitetura é composta pelas seguintes camadas:

- **Domain**: Contém o núcleo da lógica de negócios da aplicação.
- **Application**: Gerencia a lógica de aplicação, orquestrando a interação entre o domínio e as outras camadas.
- **Infrastructure**: Contém as implementações de recursos técnicos, como persistência de dados, comunicação externa, etc.
- **Test**: Contém os testes de unidade e de integração.
- **WebAPI**: Expõe a interface RESTful da aplicação.

## Estrutura do Projeto
A estrutura do projeto é organizada da seguinte forma:
/src /Domain 
# Lógica de negócios (Entidades, Agregados, Repositórios, etc.) 

/Application 
# Casos de uso, serviços de aplicação, DTOs, etc. 

/Infrastructure 
# Implementações de persistência, APIs externas, etc. 

/WebAPI 
# Controladores de API, configurações do servidor, etc. 

/tests /UnitTests 
# Testes de unidade /IntegrationTests # Testes de integração

## Requisitos

- .NET 6 ou superior
- Docker (opcional, para rodar os serviços externos como banco de dados e Kafka)

## Como Rodar a Aplicação

### 1. Configuração do Banco de Dados

Antes de rodar a aplicação, você precisa configurar o banco de dados. O projeto utiliza o **SQL Server** e o **MongoDB**.

1. **Docker Compose**: O arquivo `docker-compose.yml` já está configurado para rodar os contêineres necessários.
   
   Execute o seguinte comando para levantar o ambiente de contêineres:
   ```bash
   docker-compose up -d

2. **Migrations**: comandos pra geração do banco de dados.
     Execute o seguinte comando no caminho do PocDDD para criar a pasta e o script:
      -- dotnet ef migrations add NomeDaMigration --project PocInfra/PocInfra.csproj --startup-project PocWebAPI/PocWebAPI.csproj
     Execute o seguinte comando para criar o banco de dados:
     --  dotnet ef database update --startup-project PocWebAPI