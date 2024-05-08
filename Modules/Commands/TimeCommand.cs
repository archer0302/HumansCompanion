using Discord.Interactions;
using HumansCompanion.Extensions;
using HumansCompanion.Services;
using System.ComponentModel;

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
                TimeZoneInfo.Local,
                TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
            };
        }

        [SlashCommand("time", "Display a list of time in different timezones.")]
        public async Task Time(TimeZoneChoice timeZoneChoice)
        {
            var timeZoneDescription = timeZoneChoice.GetDescription();
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneDescription);
            await RespondAsync($"{timeZoneInfo.DisplayName} {TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo)}", ephemeral: true);
        }
        
        public enum TimeZoneChoice
        {
            [Description("Pacific Standard Time")]
            PST = 0,
            [Description("E. Australia Standard Time")]
            AEST = 1,
            [Description("Taipei Standard Time")]
            Taipei = 2,
            [Description("Eastern Standard Time")]
            EST = 3,
            [Description("Greenwich Standard Time")]
            GST = 4,
            [Description("W. Europe Standard Time")]
            WEST = 5
        }
    }
}
