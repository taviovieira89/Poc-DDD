# Base services
FROM confluentinc/cp-zookeeper:latest AS zookeeper
ENV ZOOKEEPER_CLIENT_PORT=2181
ENV ZOOKEEPER_TICK_TIME=2000

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

FROM mcr.microsoft.com/mssql/server:latest AS sqlserver
ENV ACCEPT_EULA=Y \
    SA_PASSWORD=123Passw0rd \
    MSSQL_PID=Express

FROM mongo:latest AS mongodb
ENV MONGO_INITDB_ROOT_USERNAME=admin \
    MONGO_INITDB_ROOT_PASSWORD=admin123

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
WORKDIR /src

COPY ["libs/", "libs/"]
COPY ["*.csproj", "./"]
COPY NuGet.Config ./

RUN apt-get update && \
    apt-get install -y ca-certificates && \
    rm -rf /var/lib/apt/lists/* && \
    update-ca-certificates

ENV NUGET_SSL_VERIFY=false \
    NUGET_REQUEST_TIMEOUT=600

RUN dotnet nuget remove source nuget.org || true && \
    dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

COPY . .
RUN dotnet restore "PocWebApi/PocWebApi.csproj"
RUN dotnet publish "PocWebApi/PocWebApi.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=builder /app/publish .
ENTRYPOINT ["dotnet", "PocWebApi.dll"]
