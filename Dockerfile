FROM mcr.microsoft.com/dotnet/sdk:9.0 AS tests
WORKDIR /src
COPY LE.CS.sln ./
COPY LE.CS.Api/LE.CS.Api.csproj LE.CS.Api/
COPY LE.CS.Tests/LE.CS.Tests.csproj LE.CS.Tests/
RUN dotnet restore
COPY . .
CMD ["dotnet", "test", "--no-restore"]
