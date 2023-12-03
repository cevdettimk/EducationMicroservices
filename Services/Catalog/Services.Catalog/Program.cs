using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services.Catalog.Dtos;
using Services.Catalog.Services;
using Services.Catalog.Settings;
using System;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region Services
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddControllers(opt =>
//{
//    opt.Filters.Add(new AuthorizeFilter());
//});
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddAuthentication()
    .AddCookie();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var data = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_catalog";
    options.RequireHttpsMetadata = false;
});


#endregion

#region App
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

    if (!(await categoryService.GetAllAsync()).Data.Any())
    {
        await categoryService.CreateAsync(new CategoryDto { Name = "Asp.net Core Kursu" });
        await categoryService.CreateAsync(new CategoryDto { Name = "Asp.net Core API Kursu" });
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (a, b) =>
{
    try
    {

        await b();
    }
    catch (Exception ex)
    {
        throw;
    }
});

app.Run();


#endregion