using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Simple
/*Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt")
    .CreateLogger();*/

//Daily rolling
/*Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day
        )
    .CreateLogger();*/

//appsettigs logging basic
/*Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();*/

//Enrich
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.FromLogContext()
        .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddOpenApi();

// Services
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
        }
    );
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
