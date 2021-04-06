FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

WORKDIR /app

COPY AvaloniaSerializer/*.csproj ./

RUN dotnet restore

COPY AvaloniaSerializer/* ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.0

WORKDIR /app

COPY --from=build-env /app/out .

RUN apt update
RUN apt install x11-apps -y
RUN apt install castxml -y

ENTRYPOINT ["dotnet", "AvaloniaSerializer.dll"]
