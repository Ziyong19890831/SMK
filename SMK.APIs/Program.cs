using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using SMK.APIs.Models;
using SMK.APIs.Services.Foundation;
using SMK.Data;
using SMK.Data.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string dbstring = builder.Configuration.GetSection("ConnectionStrings")["db"]; //抓到appsetting的資料
builder.Services.AddDbContext<SMKWEBContext>(opt => opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["db"]));

//string test = builder.Configuration.GetSection("AllToken")["QuickSmokingToken"];
builder.Services.Configure<AllToken>(builder.Configuration.GetSection("AllToken"));

builder.Services.AddTransient<HospBscAllService>();

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
