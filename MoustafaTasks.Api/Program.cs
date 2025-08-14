using MoustafaTasks.Application;
using MoustafaTasks.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IFilteringService, FilteringService>();
builder.Services.AddScoped(typeof(IGenericFilterService<>), typeof(GenericFilterService<>));
builder.Services.AddScoped<ISecUserService, SecUsersServices>();
builder.Services.AddDbContext<TestingDbContext>();
#region SwaggerConfig

#endregion
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
