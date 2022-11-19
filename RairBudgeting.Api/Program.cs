using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure;
using RairBudgeting.Api.Infrastructure.Data;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//builder.Services.AddScoped<IFactory<IBudget>>(context => new Factory<IBudget>(() => new Budget()));
//builder.Services.AddScoped<IFactory<IBudgetCategory>>(context => new Factory<IBudgetCategory>(() => new BudgetCategory()));
//builder.Services.AddScoped<IFactory<IPayment>>(context => new Factory<IPayment>(() => new Payment()));
//builder.Services.AddScoped<IFactory<INote>>(context => new Factory<INote>(() => new Note()));
builder.Services.AddScoped<IBudget, Budget>();
builder.Services.AddScoped<IBudgetCategory, BudgetCategory>();
builder.Services.AddScoped<INote, Note>();
builder.Services.AddScoped<IPayment, Payment>();
builder.Services.AddScoped<IEntity, Entity>();
//builder.Services.AddScoped<IRepository<IEntity>, Repository<Entity>>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddDbContext<BudgetContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b=> b.MigrationsAssembly("RairBudgeting.Api.Infrastructure"));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(RairBudgeting.Api.v1.DTOs.MapProfile));
var assembly = AppDomain.CurrentDomain.Load("RairBudgeting.Api.v1");
builder.Services.AddMediatR(assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
    DBInitializer.Init(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
