using AndroidServerSide.DbContexts;
using AndroidServerSide.Interfaces;
using AndroidServerSide.Models;
using AndroidServerSide.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using System.Data;
using UserApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddTransient<IRepository<User>, Repository<User>>();
builder.Services.AddTransient<IServiceProviderRepository, ServiceProviderRepository>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();
//builder.Services.AddTransient<IBookmarkRepository, BookmarkRepository>();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AndroidServerSide v1"));
}

app.UseHttpsRedirection();

// Corrected line: Use builder.Environment
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")), // Corrected line
    RequestPath = "/Images"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
