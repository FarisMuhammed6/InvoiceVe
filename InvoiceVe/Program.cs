/*using InvoiceVe.DataContext;
using InvoiceVe.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OcrSrvice>();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddDbContext<InvoiceDbContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceConnectionString")));

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
*/
/*using InvoiceVe.DataContext;
using InvoiceVe.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom services
builder.Services.AddScoped<OcrSrvice>(); // Note: Ensure this service is correctly named and implemented

// Configure logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Adds console logging
    logging.AddDebug();   // Adds debug window logging
});

// Configure Entity Framework Core
builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceConnectionString"))); // Ensure connection string is correct

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();          // Enable Swagger in development
    app.UseSwaggerUI();        // Enable Swagger UI in development
}

app.UseHttpsRedirection();   // Redirect HTTP requests to HTTPS
app.UseStaticFiles();        // Serve static files from wwwroot
app.UseAuthorization();      // Enable authorization middleware

app.MapControllers();        // Map controller routes

app.Run();                   // Run the application
*/

using InvoiceVe.DataContext;
using InvoiceVe.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy11",
        builder => builder.WithOrigins("http://localhost:5008")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<OcrSrvice>();


builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceConnectionString")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Policy11");
app.UseAuthorization();

app.MapControllers();

app.Run();
