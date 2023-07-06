using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Helpers;
using RairBudgeting.Api.Infrastructure;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography.Pkcs;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(new Action<JwtBearerOptions>(options => {
        builder.Configuration.Bind("AzureAd", options);
        options.Authority = "https://login.microsoftonline.com/f4ef51df-8ac0-43c4-890f-5bd447020b22/v2.0";
        options.TokenValidationParameters.NameClaimType = "name";
        options.IncludeErrorDetails = true;
        //options.TokenValidationParameters.ValidateIssuer = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidateIssuerSigningKey = false;
    }))
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"), "AzureAD");
// Add services to the container.
builder.Services.AddScoped<IBudget, Budget>();
builder.Services.AddScoped<IBudgetCategory, BudgetCategory>();
builder.Services.AddScoped<INote, Note>();
builder.Services.AddScoped<IPayment, Payment>();


builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<CosmosClient>(await Task.Run(() => CosmosDBInitializerHelpers.InitializeCosmosClientInstanceAsync(builder.Configuration)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(RairBudgeting.Api.v1.DTOs.MapProfile));
var assembly = AppDomain.CurrentDomain.Load("RairBudgeting.Api.v1");
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("RairBudgeting.Api.v1")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {

}

