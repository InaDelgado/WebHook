using WebHook;
using WebHook.infrastructure;
using WebHook.Interfaces;
using WebHook.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("GitHub"));
builder.Services.AddSingleton<IGitHubClientConfiguration, GitHubClientConfiguration>();
builder.Services.AddSingleton<IReceiveWebhook, ReceiverWebhook>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
