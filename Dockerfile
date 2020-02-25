FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.2

COPY .\SerializationPerformer .

WORKDIR .\SerializationPerformer

RUN nuget install .\packages.config

CMD dotnet .\bin\Debug\SerializationPerformer.exe