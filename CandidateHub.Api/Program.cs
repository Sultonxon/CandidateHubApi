using CandidateHub.Api.Commons.Extensions;
using CandidateHub.Api.Data.MSSQL.Options;
using CandidateHub.Api.Middlewares;
using ServiceLocator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServiceLocator<Program>();
builder.Services.Configure<SqlServerOptions>(o =>
    o.ConnectionString = builder.Configuration.GetSection("ConnectionStrings")["Data"]);
builder.AddSwagger();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.AddSwagger();
}

app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
