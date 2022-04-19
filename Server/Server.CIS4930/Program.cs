using Lib.CIS4930.Standard.Utils;
using Microsoft.EntityFrameworkCore;
using Server.CIS4930.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt => opt.SerializerSettings.Converters
    .Add(new TaskJsonConverter()));
builder.Services
    .AddDbContext<DataContext>(
        opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
