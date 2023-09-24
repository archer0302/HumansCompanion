using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules.Commands
{
    public class TimeCommand : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;
        private List<TimeZoneInfo> _timeZones;

        public TimeCommand(CommandHandler handler)
        {
            _handler = handler;
            _timeZones = new List<TimeZoneInfo>
            {
                TimeZoneInfo.Local
            };
        }

        [SlashCommand("time", "Display a list of time in different timezones.")]
        public async Task Time()
        {
            await RespondAsync($"{TimeZoneInfo.Local} {DateTime.Now}", ephemeral: true);
        }
    }
}
