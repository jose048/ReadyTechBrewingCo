using ReadyTechBrewingCoApi.Interfaces;
using ReadyTechBrewingCoApi.Wrappers;
using Serilog;
using Serilog.Events;
//using Serilog.Sinks.console
using Serilog.Sinks.File;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder => {
    builder.AddDebug();
    builder.AddSerilog();
});

Log.Logger = new LoggerConfiguration()    
    .WriteTo.File("logs/ReadyTechBrewingColog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

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
