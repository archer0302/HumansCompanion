using Discord;
using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules.Commands
{
    public class MemeCommand : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private readonly CommandHandler _handler;
        private readonly Dictionary<string, string> cachedNameURLpairs = new();

        public MemeCommand(CommandHandler handler)
        {
            _handler = handler;
        }

        [SlashCommand("addmeme", "Add a new meme. Use `file:` or `url:` to upload image.")]
        public async Task AddMeme(string name, IAttachment? attachment = null, string url = "")
        {
            await RespondAsync("Meme added.", ephemeral: true);
        }
    }
}
