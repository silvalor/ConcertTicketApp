
using ConcertTicketApp.Db;
using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;  // Header info for clients to see the supported versions
});

//These are useful for testing. Should be removed before production.
var testRepository = new TestRepository();
builder.Services.AddSingleton<IConcertRepository, TestRepository>(s => testRepository);
builder.Services.AddSingleton<ITicketRepository, TestRepository>(s => testRepository);
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

var testPaymentProcessor = new TestPaymentProcessor();
builder.Services.AddSingleton<IPaymentProcessingService, TestPaymentProcessor>(s => testPaymentProcessor);
var testIdempotencyCache = new TestIdempotencyCache();
builder.Services.AddSingleton<IIdempotencyCache, TestIdempotencyCache>(s => testIdempotencyCache);

builder.Services.AddControllers();
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

app.UseRouting();
app.UseCors("AllowAll"); //Useful for swagger testing. Remove for production.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
