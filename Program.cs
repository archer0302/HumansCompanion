using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();

    private DiscordSocketClient _client;
    private readonly IConfiguration _config;

    public Program()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        _client.Ready += ClientReady;
        _client.SlashCommandExecuted += SlashCommandHandler;

        var _builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(path: "config.json");
        _config = _builder.Build();
    }

    public async Task MainAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _config["token"]);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name) 
        {
            case "time":
                await HandleTimeCommand(command);
                break;
            default:
                await command.RespondAsync($"You executed {command.Data.Name}");
                break;
        }
    }

    public async Task HandleTimeCommand(SocketSlashCommand command)
    {
        await command.RespondAsync($"{TimeZoneInfo.Local}\t{DateTime.Now}", ephemeral: true);
    }

    public async Task ClientReady()
    {
        ulong guildId = 823689180571893830;
        var guild = _client.GetGuild(guildId);

        var guildCommand = new SlashCommandBuilder();
        guildCommand.WithName("time");
        guildCommand.WithDescription("Display local time.");

        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("first-global-command");
        globalCommand.WithDescription("First global slash command.");

        try
        {
            await guild.CreateApplicationCommandAsync(guildCommand.Build());
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
        
    }
}
