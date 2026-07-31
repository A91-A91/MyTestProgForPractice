using Microsoft.EntityFrameworkCore;
using System;
using MyTestProgForPractice.Data;
using System.Text;
using Microsoft.OpenApi.Models;
using Npgsql;
using MyTestProgForPractice.Models;
using MyTestProgForPractice.Services;
using MyTestProgForPractice.Classes;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("DefaultConnection"));


var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<DbForPracticeContext>(options =>
    options.UseNpgsql(dataSource));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Operations_DB>();
builder.Services.AddScoped<CsvParser>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();