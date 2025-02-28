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

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=builder /app/publish .
ENTRYPOINT ["dotnet", "PocWebApi.dll"]
