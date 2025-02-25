# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Configura o proxy (se necessário)
# ENV HTTP_PROXY=http://seu-proxy:porta
# ENV HTTPS_PROXY=http://seu-proxy:porta

# Copia o NuGet.Config para o diretório correto dentro do contêiner
COPY NuGet.Config ./

# Copia a pasta packages para o contêiner
COPY ./packages /app/packages

# Verifica se a pasta packages foi copiada corretamente
RUN ls -la /app/packages
RUN ls -la /app/packages/ToolKit/1.0.4/

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
RUN dotnet nuget remove source packages || true

# Adiciona a fonte packages
RUN dotnet nuget add source /app/packages -n packages

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

# Copia os arquivos compilados da etapa de build
COPY --from=build /app/out .

# Configura o ponto de entrada do contêiner
ENTRYPOINT [ "dotnet", "PocWebApi.dll" ]