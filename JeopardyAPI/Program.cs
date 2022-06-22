using Serilog;
using DataLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Cors;
// using Microsoft.EntityFrameworkCore; // for EFCORE

var MyAllowedSpecificOrigins = "_myAllowedSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Added Logging using Serilog
builder.Host.UseSerilog(
    (ctx, lc) => lc.WriteTo.Console().WriteTo.File("../logs/jeopardy.txt", rollingInterval: RollingInterval.Day)
);

// add cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowedSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                      });
});

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository>(ctx => new DBRepository("Data Source=../../Jeopardy-SQLite/createSQLdatabase/jeopardyDB.sqlite;Version=3;")); // FOR LOCAL DATABASE USING SQLITE
// builder.Services.AddDbContext<C4DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Junipardy"))); // FOR SQL HOSTED ON AZURE (USING EFCORE)
// builder.Services.AddScoped<IRepository>(ctx => new DBRepository(builder.Configuration.GetConnectionString("JunipardyDB"))); // FOR SQL HOSTED ON AZURE (NOT USING EFCORE)
builder.Services.AddScoped<IBusiness, Business>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowedSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
