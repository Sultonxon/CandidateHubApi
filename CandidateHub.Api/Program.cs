using CandidateHub.Api.Data.MSSQL.Options;
using Microsoft.Extensions.Options;
using ServiceLocator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceLocator<Program>();
builder.Services.Configure<SqlServerOptions>(o =>
    o.ConnectionString = builder.Configuration.GetSection("ConnectionStrings")["Data"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
