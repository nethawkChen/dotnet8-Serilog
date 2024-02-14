using Serilog.Events;
using Serilog;
using Serilog.LogBranch.Extensions;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var setting = builder.Configuration;
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)  //從設定檔中讀取
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.With(new LogEnricher())
        .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e =>
            e.Properties["ControllerName"].ToString().Contains("Controller"))
            .WriteTo.File(setting["Serilog:WriteTo:2:Args:Path"],
                rollingInterval: Enum.Parse<RollingInterval>(setting["Serilog:WriteTo:2:Args:rollingInterval"]),
                retainedFileCountLimit: int.Parse(setting["Serilog:WriteTo:2:Args:retainedFileCountLimit"]),
                outputTemplate: setting["Serilog:WriteTo:2:Args:outputTemplate"]))
        .WriteTo.Logger(lc=>lc.Filter.ByExcluding(e=>
            e.Properties["SourceContext"].ToString().Contains("Controller"))
            .WriteTo.File(setting["Serilog:WriteTo:3:Args:Path"],
                rollingInterval: Enum.Parse<RollingInterval>(setting["Serilog:WriteTo:3:Args:rollingInterval"]),
                retainedFileCountLimit: int.Parse(setting["Serilog:WriteTo:3:Args:retainedFileCountLimit"])))
        .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e =>
            e.Properties["SourceContext"].ToString().Contains("Controller"))
            .WriteTo.File(new CompactJsonFormatter(), setting["Serilog:WriteTo:5:Args:Path"],
                rollingInterval: Enum.Parse<RollingInterval>(setting["Serilog:WriteTo:5:Args:rollingInterval"]),
                retainedFileCountLimit: int.Parse(setting["Serilog:WriteTo:5:Args:retainedFileCountLimit"])))
    );

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
} catch(Exception er) {
    Log.Fatal(er, "Application terminated unexpectedly");
} finally {
    Log.CloseAndFlush();
}


