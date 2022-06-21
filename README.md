# Jeopardy-Backend

Welcome to the Jeopardy backend! In this repository, an API(JeopardyAPI) was created using ASP.NET. JeoparyAPI will be used with Angular to retrieve information from a local database created using SQLite.

## About

Jeopary is not an original idea. It uses the original game temple to make learning fun. The game questions revolve around multiple topics. To name a few: Angular, C#, SQL, Principles/Methodologies, etc.

## Instructions

To use Junipardy with local database:
* Clone Frontend, Backend, SQLite into one directory
* In `Jeopardy-Backend/JeopardyAPI/program.cs` comment line 38 and uncomment line 36
* In `Jeopardy-Frontend/Jeopardy/src/app/service/http.service.ts` change 'apiAzureURL' to 'apiBaseURL'
  * Ensure the local host url in `Jeopardy-Frontend/Jeopardy/src/environments/environment.ts` mathes the one provided when running `dotnet run` in the next step
* In `Jeopardy-Backend/JeopardyAPI` run the command `dotnet run`
* In `Jeopardy-Frontend/Jeopardy` run the command `ng serve`
* In your browser go to the local host site stated

## Entity Relationship Diagram

![Entity Relationship Diagram](https://github.com/jxm8022/Jeopardy-Backend/blob/main/Jeopardy_ERD.jpeg?raw=true)
