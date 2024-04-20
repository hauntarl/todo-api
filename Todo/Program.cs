// * The builder variable is used for dependency injection and configuration
using Todo.Services.Tasks;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddControllers();
    // * Inject TaskService dependency
    builder.Services.AddScoped<ITaskService, TaskService>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

// * The app variable is used to manage and configure the HTTP request pipeline
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    /*
     * Global error handling
     - Add UseExceptionHandler middleware 
     - We have a pipeline that our request goes through
     - The UseExceptionHandler adds a try/catch surrounding the following middlewares
     - If an exception is thrown, it catches that exception and it changes the request route to the
       route that we define, then it re-executes the request
     
     * What is a middleware?
     - Imagine a sandwich of code that runs before and after the next piece of middleware where
       somewhere down the road one of the middlewares actually invokes the controller.
    */
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}
