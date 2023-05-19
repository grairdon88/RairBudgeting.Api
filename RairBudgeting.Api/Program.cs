using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<IBudget, Budget>();
builder.Services.AddScoped<IBudgetCategory, BudgetCategory>();
builder.Services.AddScoped<INote, Note>();
builder.Services.AddScoped<IPayment, Payment>();


builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<CosmosClient>(serviceProvider => {
    return new CosmosClient(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(RairBudgeting.Api.v1.DTOs.MapProfile));
var assembly = AppDomain.CurrentDomain.Load("RairBudgeting.Api.v1");
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("RairBudgeting.Api.v1")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope()) {
//    var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
//    DBInitializer.Init(context);
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else {
    if (!app.Environment.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            //options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //options.RoutePrefix = string.Empty;
        });
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {

}
