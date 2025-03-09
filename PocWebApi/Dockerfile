<<<<<<< HEAD
# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
=======
# Zookeeper
FROM confluentinc/cp-zookeeper:latest AS zookeeper
ENV ZOOKEEPER_CLIENT_PORT=2181
ENV ZOOKEEPER_TICK_TIME=2000

# Kafka
FROM confluentinc/cp-kafka:latest AS kafka
ENV KAFKA_BROKER_ID=1
ENV KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181
ENV KAFKA_LISTENERS=INTERNAL://0.0.0.0:9092,EXTERNAL://0.0.0.0:9093
ENV KAFKA_ADVERTISED_LISTENERS=INTERNAL://172.24.48.1:9092,EXTERNAL://localhost:9093
ENV KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=INTERNAL:PLAINTEXT,EXTERNAL:PLAINTEXT
ENV KAFKA_INTER_BROKER_LISTENER_NAME=INTERNAL
ENV KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1
ENV KAFKA_DEFAULT_REPLICATION_FACTOR=1
ENV KAFKA_NUM_PARTITIONS=1
ENV KAFKA_AUTO_CREATE_TOPICS_ENABLE="true"

# SQL Server
FROM mcr.microsoft.com/mssql/server:latest AS sqlserver
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=123Passw0rd
ENV MSSQL_PID=Express

# MongoDB
FROM mongo:latest AS mongodb
ENV MONGO_INITDB_ROOT_USERNAME=admin
ENV MONGO_INITDB_ROOT_PASSWORD=admin123
# Build stage for WebAPI
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
WORKDIR /src

#RUN dotnet nuget remove source github
# Add debug logging
RUN echo "Starting GitHub package authentication..."
RUN echo "Current directory: $(pwd)"
RUN echo "Contents of /run/secrets: $(ls -la /run/secrets)"

# Original authentication command
RUN --mount=type=secret,id=gh_token \
    export GITHUB_TOKEN="$(cat /run/secrets/gh_token)" && \
    dotnet nuget add source https://nuget.pkg.github.com/taviovieira89/index.json \
    --name github \
    --username taviovieira89 \
    --password $GITHUB_TOKEN \
    --store-password-in-clear-text

COPY ["PocWebApi/PocWebApi.csproj", "PocWebApi/"]
COPY ["PocDomain/PocDomain.csproj", "PocDomain/"]
COPY ["PocApplication/PocApplication.csproj", "PocApplication/"]
COPY ["PocInfra/PocInfra.csproj", "PocInfra/"]

RUN dotnet restore "PocWebApi/PocWebApi.csproj"

COPY . .
RUN dotnet publish "PocWebApi/PocWebApi.csproj" -c Release -o /app/publish
>>>>>>> main

# Configura o proxy (se necessário)
# ENV HTTP_PROXY=http://seu-proxy:porta
# ENV HTTPS_PROXY=http://seu-proxy:porta

# Copia o NuGet.Config para o diretório correto dentro do contêiner
COPY NuGet.Config ./

# Copia a pasta packages para o contêiner
COPY ./local-packages /app/local-packages

# Verifica se a pasta packages foi copiada corretamente
RUN ls -la /app/local-packages
RUN ls -la /app/local-packages/ToolKit/1.0.4/

# Atualiza pacotes do sistema e instala certificados SSL
RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*

# Atualiza os certificados instalados
RUN update-ca-certificates

# Ignora erros de certificado SSL (apenas para desenvolvimento)
ENV NUGET_SSL_VERIFY false

# Aumenta o timeout do NuGet (opcional)
ENV NUGET_REQUEST_TIMEOUT 600

# Remove a fonte nuget.org se ela já existir
RUN dotnet nuget remove source nuget.org || true

# Adiciona a fonte nuget.org novamente
RUN dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

# Remove a fonte packages se ela já existir
RUN dotnet nuget remove source local-packages || true

# Adiciona a fonte packages
RUN dotnet nuget add source /app/local-packages -n local-packages

# Copia os arquivos de projeto para restaurar dependências
COPY PocWebApi/*.csproj ./PocWebApi/
COPY PocDomain/*.csproj ./PocDomain/
COPY PocInfra/*.csproj ./PocInfra/
COPY PocApplication/*.csproj ./PocApplication/

# Restaura as dependências de cada projeto
RUN dotnet restore PocDomain/PocDomain.csproj
RUN dotnet restore PocInfra/PocInfra.csproj
RUN dotnet restore PocApplication/PocApplication.csproj

# Restaura as dependências do projeto principal
RUN dotnet restore PocWebApi/PocWebApi.csproj

# Copia o restante dos arquivos
COPY . ./

# Compila e publica a aplicação
RUN dotnet publish PocWebApi -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
<<<<<<< HEAD

# Copia os arquivos compilados da etapa de build
COPY --from=build /app/out .

# Configura o ponto de entrada do contêiner
ENTRYPOINT [ "dotnet", "PocWebApi.dll" ]
=======
COPY --from=builder /app/publish .
ENTRYPOINT ["dotnet", "PocWebApi.dll"]
>>>>>>> main
