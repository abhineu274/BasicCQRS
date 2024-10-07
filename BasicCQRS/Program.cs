using BasicCQRS;
using BasicCQRS.Data;
using BasicCQRS.Filters;
using BasicCQRS.Mapper;
using BasicCQRS.Middleware;
using BasicCQRS.Services;
using BasicCQRS.Services.AuthService;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
//new way to add swagger

var assembly = typeof(Program).Assembly;

//These are the types of Dependency Injections. Where I am binding the classes loosely with the IEmployeeRepository, IStartuFilter and also MediatR service registration also AddDbContext

/*Service Lifetimes of DI :
*Transient: A new instance of the service is created each time it is requested. This is useful for lightweight, stateless services.
*Scoped: A single instance is created per request (or per scope). This is ideal for services that need to maintain state within a single request, like database contexts.
*Singleton: A single instance is created and shared throughout the application's lifetime. This is suited for services that are expensive to create or maintain global state.
*/
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
});

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddTransient<IStartupFilter,
                      RequestSetOptionsStartupFilter>();
//StartupFilter is called even before hitting the controller api.(Url_Hit -> Middleware -> Controller)
//We can add different things/logic here. e.g. httpContext. This custom middleware is called after Exception Handler,CORS(Built-in middlewares) etc.

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IExceptionHandler, CustomExceptionHandler>();
//dependency injection for ExceptionHandlingMiddleware

builder.Services.AddScoped<LoggingActionFilter>(); // Register the filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggingActionFilter>();
});

//Registration and exection of the filter. Check LoggingActionFilter

//Order of Execution - Middleware -> Action Filters -> Action Method   ???? Need to check

//Validations
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();
});
//entire assembly contianing CreateEmployeeCommandValidator will be registered, so no need to register UpdateEmployeeCommandValidator


builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
//registering automapper

// Bind JwtOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// JWT Configuration
var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;
var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience
    };
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://example.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Use CORS policy - need to add it before UseAuthorization
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

//app.UseMiddleware<ErrorHandlingMiddleware>();
//ErrorHandlingMiddleware - In this class I am doing global exception handling

app.UseMiddleware<ExceptionHandlingMiddleware>();

////This is the new way and above is the old way. In this way we implement the IExceptionHandler

app.MapControllers();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke(); // This calls the next middleware in the pipeline
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});
//Added middleware pipleine. It takes request and response from context

ApplyMigration(); //We will call this method to apply the migrations. Need to add it before app.Run()
app.Run();

/*
 * So, after creatting the model classes, we need to add the migration and then apply the migration.
 * To add the migration, we need to run this command in the Nuget Package Manager Console - add-migration [name]
 * After that we need to run the project and it will apply the migration
 * It will create the database and tables based on the model classes
 * Also, it will create the __EFMigrationsHistory table to keep the track of migrations and create a snapshot of the model classes
 * And it will create a migration file in the Migrations folder
 * This is used for code first approach
 */
void ApplyMigration()
{
    // Apply pending migrations
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<DataContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}


//To apply migrations in code first approach, we need to add this ApplyMigration method and open the Nuget Package Manager Console
// and run this command - add-migration [name]
//e.g add-migration first-migration
//after that just need to start the project



/*
 * EF Core vs MediatR
 * EF Core - Controller -> Service + Service Interface -> DbContext -> Model classes with annotations
 * MediatR - COntroller -> Commands/Queries -> Respective Handlers -> DbContext -> Model classes with/without annotations
 * 
 * *If we are using Dapper -> no model class annotations -> Plain sql query execution -> faster but tedious
 * If we are using EF core then Model class annotations are required -> No plain sql/sp -> Modern way but slower
 */

/*
 * MediatR - Queries and Commands -> Handlers 
 * On Handlers, we can see the Query/Command and response mapping
 * Thats how MediatR identifies which handler to invoke for which Query/Commands
 */

/*
 * Created A .Net 8 Web API project
 * Created Model class Employee
 * Configured DbContext and added connection string in appsettings.json
 * Added MediatR configuration
 * Created EmployeeRepository, added Commands-Queries and Handlers and added CRUD operations
 * Added Middleware, Filters, FluentValidation, AutoMapper, Custom Exception Handling
 * Created JwtOptions, TokenService and added JWT Authentication
 * Added AuthController and AuthService. Created Login Endpoint
 * Authorized the endpoints
 * Added Password Hashing
 * Added CORS policy    
 * Added Unit Tests
 */


/*
 * JWT Authentication : 
 * Added required dependencies - Microsoft.AspNetCore.Authentication.JwtBearer, JWT
 * Added JwtOptions in appsettings.json with Secret, Issuer and Audience
 * Created JwtOptions class and binded it in Program.cs
 * Created ITokenService and TokenService with GenerateToken method
 * Added ITokenService and TokenService in DI container
 * Created AuthController, added Login Command and used MediatR to send the command
 * Injected ITokenService, EmployeeRepository in LoginCommandHandler and used it to validate the username-password and created token
 * Added Generated token to hit the endpoint
 * 
 * Password Hashing :
 * Added required dependencies - Microsoft.AspNetCore.Identity
 * Created IPasswordService and PasswordService with HashPassword and VerifyPasswordAsync methods
 * Registered IPasswordService and PasswordService in DI container
 * Injected PasswordService in CreateEmployeeCommandHandler and used it hash the password while creating the employee
 * Also injected PasswordService in LoginCommandHandler and used it to verify the password
 */