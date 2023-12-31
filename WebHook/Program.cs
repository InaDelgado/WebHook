using WebHook.Infrastructure;
using WebHook.Interfaces;
using WebHook.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<IGitHubClientConfiguration, GitHubClientConfiguration>();
        builder.Services.AddSingleton<IReceiveWebhook, ReceiverWebhook>();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}



