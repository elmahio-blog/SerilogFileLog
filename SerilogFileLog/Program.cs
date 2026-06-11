using Serilog;

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


try
{
    var builder = WebApplication.CreateBuilder(args);

    //Enrich
    Log.Logger = new LoggerConfiguration()
        
        .ReadFrom.Configuration(builder.Configuration)
        //.Enrich.WithMachineName()
        //.Enrich.WithEnvironmentUserName()
        .Enrich.FromLogContext()
        .CreateLogger();
    
    Log.Information("Starting up!");
    
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
    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during startup");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}