using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title       = "Backend API",
        Version     = "v1",
        Description = "ASP.NET Core Web API for Employee Management"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRoleService,     RoleService>();
builder.Services.AddScoped<IAccountService,  AccountService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var connStr = builder.Configuration.GetConnectionString("MySqlDb")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
           .EnableSensitiveDataLogging(false) 
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression(); 
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API v1");
    c.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseStaticFiles();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

try 
{
    await DbInitializer.InitializeAsync(connStr);
}
catch (Exception ex)
{
    Console.WriteLine($"Database initialization failed: {ex.Message}");
}

app.Run();
