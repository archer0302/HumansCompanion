using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules.Commands
{
    public class MemeCommand : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private readonly CommandHandler _handler;
        private readonly Dictionary<string, string> cachedNameURLpairs;

        public MemeCommand(CommandHandler handler, Dictionary<string, string> cachedNameURLpairs)
        {
            _handler = handler;
            this.cachedNameURLpairs = cachedNameURLpairs;
        }
    }
}
