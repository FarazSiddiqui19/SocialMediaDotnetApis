using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using SocialMedia.Data;
using SocialMedia.Data.Repository;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.Services;
using SocialMedia.Services.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(
        Options => { 
            Options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
           
        }
    );




// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<SocialMedia.Data.SocialContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<SocialContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            
        });
});


var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableDynamicJson()
    .Build();

builder.Services.AddDbContext<SocialContext>(options => options.UseNpgsql(dataSourceBuilder));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUsersServices, UserServices>();
builder.Services.AddScoped<IPostsServices, PostServices>();
builder.Services.AddScoped<IPostReactionService, PostReactionService>();


builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostReactionRepository, PostReactionRepository>();


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
