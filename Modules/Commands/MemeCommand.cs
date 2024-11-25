using Discord;
using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules.Commands
{
    public class MemeCommand : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly string bucketName = "meme-storage-asdf";
        private readonly Dictionary<string, string> supportedImageTypes = new Dictionary<string, string>
        {
            { "image/jpeg", "jpg" },
            { "image/png", "png" },
            { "image/gif", "gif" },
        };
        public InteractionService Commands { get; set; }
        private readonly CommandHandler _handler;
        private readonly AWSS3BucketService awsS3BucketService;
        private readonly Dictionary<string, string> cachedNameURLpairs = new();

        public MemeCommand(CommandHandler handler, AWSS3BucketService awsS3BucketService)
        {
            _handler = handler;
            this.awsS3BucketService = awsS3BucketService;
        }

        [SlashCommand("addmeme", "Add a new meme. Use `file:` to upload image. Supports jpg, png and gif.")]
        public async Task AddMeme(string name, IAttachment? attachment = null)
        {
            var result = false;
            if (attachment != null)
            {
                try
                {
                    name = $"{name}.{GetExtensionFromAttachmentContentType(attachment.ContentType)}";
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

        private string GetExtensionFromAttachmentContentType(string contentType)
        {
            if (supportedImageTypes.TryGetValue(contentType, out var extension))
            {
                return extension;
            }
            else
            {
                throw new ArgumentException($"Content type of {contentType} is not supported.");
            }
        }
    }
}
