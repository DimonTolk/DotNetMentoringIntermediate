FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /WebApplication1
COPY . .
RUN dotnet restore
RUN dotnet publish -o /WebApplication1/published-app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /WebApplication1
COPY --from=build /WebApplication1/published-app /WebApplication1
ENTRYPOINT [ "dotnet", "/WebApplication1/WebApplication1.dll" ]