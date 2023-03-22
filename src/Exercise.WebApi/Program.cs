using Exercise.Core;
using Exercise.Infrastructure;
using Exercise.Model;
using Exercise.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IGenericRepository<ModelType1>, GenericRepository<ModelType1>>();
builder.Services.AddScoped<IGenericRepository<ModelType2>, GenericRepository<ModelType2>>();
builder.Services.AddSingleton<ICustomAuthenticationService, CustomAuthenticationService>();

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<AsyncActionFilter>();
    options.Filters.Add<AsyncResourceFilter>();
    options.Filters.Add<AsyncAuthorizationFilter>();
});

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
