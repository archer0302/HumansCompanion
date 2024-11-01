using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules.Commands
{
    public class MemeCommand : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;

        public MemeCommand(CommandHandler handler)
        {
            _handler = handler;
        }
    }
}
