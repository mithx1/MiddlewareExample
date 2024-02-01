var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

// This middleware logs the request processing status before and after the next middleware in the pipeline is invoked.
app.Use(async (context, next) =>
{
    // Log before executing the next middleware
    Console.WriteLine($"Logic before executing the next delegate in the Use method");

    // Invoke the next middleware in the pipeline
    await next.Invoke();

    // Log after executing the next middleware
    Console.WriteLine($"Logic after executing the next delegate in the Use method");
});

// This middleware is specific to the "/usingmapbranch" route.
app.Map("/usingmapbranch", builder =>
{
    // This middleware logs the request processing status before and after the next middleware in the pipeline is invoked.
    builder.Use(async (context, next) =>
    {
        // Log before executing the next middleware
        Console.WriteLine("Map branch logic in the Use method before the next delegate");

        // Invoke the next middleware in the pipeline
        await next.Invoke();

        // Log after executing the next middleware
        Console.WriteLine("Map branch logic in the Use method after the next delegate");
    });

    // This middleware sends a response to the client.
    builder.Run(async context =>
    {
        // Log the response
        Console.WriteLine($"Map branch response to the client in the Run method");

        // Write the response to the client
        await context.Response.WriteAsync("Hello from the map branch.");
    });

});

// This middleware is invoked only when the request contains a query string key "testquerystring".
app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
{
    // This middleware sends a response to the client.
    builder.Run(async context =>
    {
        // Write the response to the client
        await context.Response.WriteAsync("Hello from the MapWhen branch.");
    });
});

// This middleware sends a response to the client.
app.Run(async context =>
{
    // Log the response
    Console.WriteLine($"Writing the response to the client in the Run method");

    // Write the response to the client
    await context.Response.WriteAsync("Hello from the middleware component.");
});


app.MapControllers();

app.Run();
