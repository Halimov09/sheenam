//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Services.Foundations.Guests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddFoundationServices(builder);
AddBrokers(builder);


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

static void AddBrokers(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<IstorageBroker, StorageBroker>();
    builder.Services.AddTransient<IloggingBroker, LoggingBroker>();
}

static void AddFoundationServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<IGuestService, GuestService>();
}

