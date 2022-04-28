#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["gcp-net-sample/gcp-net-sample.csproj", "gcp-net-sample/"]
RUN dotnet restore "gcp-net-sample/gcp-net-sample.csproj"
COPY . .
WORKDIR "/src/gcp-net-sample"
RUN dotnet build "gcp-net-sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gcp-net-sample.csproj" -c Release -o /app/publish

FROM base AS final
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gcp-net-sample.dll"]