using GrpcGreeter6.Services;

// Needed for .NET 3.x | does nothing for .NET 5/6
//AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

// Configure Kestral to listen on a specific HTTP20_ONLY port
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // causes socket error when uncommented // Comment in for Local development // Uncomment for publishing to App Service - this can get rid of the "Application Error" page
    options.ListenAnyIP(8585, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});
    
// original/template
//    ConfigureWebHostDefaults(webBuilder =>
//{
//    // Configure Kestral to specific HTTP2 only port 
//    webBuilder.ConfigureKestrel(options =>
//    {
//        //options.ListenAnyIP(8080); // causes socket error when uncommented // Comment in for Local development // Uncomment for publishing to App Service - this can get rid of the "Application Error" page
//        options.ListenAnyIP(8585, listenOptions =>
//        {
//            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
//        });
//    });
//    //webBuilder.Build();
//});


// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client-test. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// local dev?
//app.MapGet("/", async context =>
//{
//    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
//});

app.Run();
