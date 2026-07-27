using Microsoft.EntityFrameworkCore;
using System;
using MyTestProgForPractice.Data;
using System.Text;
using Microsoft.OpenApi.Models;
using Npgsql;
using MyTestProgForPractice.Models;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("DefaultConnection"));


var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<DbForPracticeContext>(options =>
    options.UseNpgsql(dataSource));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



///builder.Services.AddScoped<Operations_tasks>();

//builder.Services.AddScoped<Operations_authorization>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();