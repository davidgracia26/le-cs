FROM mcr.microsoft.com/dotnet/sdk:9.0 AS tests
WORKDIR /src
COPY LE.CS.sln ./
COPY LE.CS.Api/LE.CS.Api.csproj LE.CS.Api/
COPY LE.CS.Tests/LE.CS.Tests.csproj LE.CS.Tests/
RUN dotnet restore
COPY . .
CMD ["dotnet", "test", "--no-restore"]

FROM tests AS publish
RUN dotnet publish LE.CS.Api -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS api
WORKDIR /app
COPY --from=publish /out .
ENV ASPNETCORE_URLS=http://+:8080
RUN mkdir /data && chown app /data
USER app
ENTRYPOINT ["dotnet", "LE.CS.Api.dll"]
