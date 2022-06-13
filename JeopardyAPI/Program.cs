using Serilog;
using DataLayer;
using BusinessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Added Logging using Serilog
builder.Host.UseSerilog(
    (ctx, lc) => lc.WriteTo.Console().WriteTo.File("../logs/jeopardy.txt", rollingInterval: RollingInterval.Day)
);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository>(ctx => new DBRepository(builder.Configuration.GetConnectionString("")));
builder.Services.AddScoped<IBusiness, Business>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
