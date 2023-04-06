#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# set key and ssh
RUN apt-get update && \
	apt-get install -y openssh-client && \
	rm -rf /var/lib/apt/lists/*
RUN mkdir /keys && \
	chmod 400 /keys
RUN echo 'StrictHostKeyChecking no ' >> /etc/ssh/ssh_config
RUN echo 'Port 2222 ' >> /etc/ssh/ssh_config
RUN echo 'IdentityFile /keys/id_rsa ' >> /etc/ssh/ssh_config


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeExecutorService/CodeExecutorService.csproj", "CodeExecutorService/"]
RUN dotnet restore "CodeExecutorService/CodeExecutorService.csproj"
COPY . .
WORKDIR "/src/CodeExecutorService"
RUN dotnet build "CodeExecutorService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeExecutorService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN mkdir /sourcecodes
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeExecutorService.dll"]