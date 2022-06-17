# add base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY . .

RUN dotnet clean ./Jeopardy-Backend.sln
RUN dotnet publish ./JeopardyAPI --configuration Release -o ./publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS run

WORKDIR /app

COPY --from=build /app/publish .

CMD ["dotnet", "JeopardyAPI.dll"]