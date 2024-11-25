using Discord;
using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules.Commands
{
    public class MemeCommand : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly string bucketName = "meme-storage-asdf";
        public InteractionService Commands { get; set; }
        private readonly CommandHandler _handler;
        private readonly AWSS3BucketService awsS3BucketService;
        private readonly Dictionary<string, string> cachedNameURLpairs = new();

        public MemeCommand(CommandHandler handler, AWSS3BucketService awsS3BucketService)
        {
            _handler = handler;
            this.awsS3BucketService = awsS3BucketService;
        }

        [SlashCommand("addmeme", "Add a new meme. Use `file:` to upload image.")]
        public async Task AddMeme(string name, IAttachment? attachment = null)
        {
            var result = false;
            if (attachment != null)
            {
                try
                {
                    result = await awsS3BucketService.UploadAttachment(attachment, bucketName, name);
                }
                catch (Exception ex)
                {
                    await RespondAsync($"Failed to add meme. {ex.Message}", ephemeral: true);
                    return;
                }
            }

            if (result)
            {
                await RespondAsync("Meme added.", ephemeral: true);
            }
            else
            {
                await RespondAsync("Failed to add meme.", ephemeral: true);
            }
        }

        [SlashCommand("meme", "Show a meme.")]
        public async Task Meme(string name)
        {
            
        }
    }
}
