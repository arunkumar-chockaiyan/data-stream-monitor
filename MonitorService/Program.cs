using Microsoft.Extensions.Options;
using MonitorService.Configuration;
using MonitorService.Repository;
using MonitorService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddAppConfig(builder);
AddAppServices(builder);

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

static void AddAppConfig(WebApplicationBuilder builder)
{
    builder.Services.Configure<MetricsConfig>(builder.Configuration.GetSection(MetricsConfig.SECTION_NAME));
    builder.Services.AddTransient(_ => _.GetRequiredService<IOptions<MetricsConfig>>().Value);
}
static void AddAppServices(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<ITweetService, TweetService>();
    builder.Services.AddSingleton<ITweetRepository, InMemTweetRepository>();
    builder.Services.AddSingleton<ITagRepository, InMemTagRepository>();
    builder.Services.AddSingleton<ITopNTagRepository, InMemTopNTagRepository>();
    builder.Services.AddSingleton<ITagService, TopNTagService>();
}