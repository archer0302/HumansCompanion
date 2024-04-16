using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HumansCompanion.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HumansCompanion;

public class Program
{
    private readonly IConfiguration _config;
    private DiscordSocketClient? _client;
    private InteractionService? _commands;

    public static Task Main(string[] args) => new Program().MainAsync();

    public Program()
    {
        var _builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("config.json");
        _config = _builder.Build();
    }

    public async Task MainAsync()
    {
        using (var services = ConfigureServices())
        {
            var client = services.GetRequiredService<DiscordSocketClient>();
            var commands = services.GetRequiredService<InteractionService>();
            _client = client;
            _commands = commands;

            client.Log += Log;
            commands.Log += Log;
            client.Ready += ClientReadyAsync;

            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public async Task ClientReadyAsync()
    {

        if (IsDebug())
        {
            var testGuildId = ulong.Parse(_config["testGuildId"]);
            // this is where you put the id of the test discord guild
            Console.WriteLine($"In debug mode, adding commands to {testGuildId}...");
            await _commands.RegisterCommandsToGuildAsync(testGuildId);
        }
        else
        {
            // this method will add commands globally, but can take around an hour
            await _commands.RegisterCommandsGloballyAsync(true);
        }

        Console.WriteLine($"Connected as -> [{_client.CurrentUser}] :)");
    }

    private ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddSingleton(_config)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<CommandHandler>()
            .BuildServiceProvider();
    }

    static bool IsDebug()
    {
#if DEBUG
        return true;
#else
    return false;
#endif
    }
}