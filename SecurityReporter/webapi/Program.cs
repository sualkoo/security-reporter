using System.Configuration;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using webapi.ProjectSearch.Services;
using webapi.ProjectSearch.Services.Extractor;
using webapi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICosmosService, CosmosService>();
builder.Services.AddSingleton<IProjectDataValidator, ProjectDataValidator>();
builder.Services.AddSingleton<IProjectDataParser, ProjectDataParser>();
builder.Services.AddSingleton<IDBProjectDataParser, DbProjectDataParser>();
builder.Services.AddSingleton<IProjectReportService, ProjectReportService>();
builder.Services.AddSingleton<IPdfBuilder, PdfBuilder>();
builder.Services.AddSingleton<IAzureBlobService, AzureBlobService>();

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();