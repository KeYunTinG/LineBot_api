using LineBot_api.Filters;
using LineBot_api.Service;
using LineBot_api.Service.Interface;
using LineBot_api.Services;
using LineBot_api.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSerilog();

builder.Services.AddScoped<ReturnFormatFilter>();

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddTransient<IIDCreate, IDCreate>();

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
