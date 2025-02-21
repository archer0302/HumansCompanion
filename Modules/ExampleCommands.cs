﻿using Discord.Interactions;
using HumansCompanion.Services;

namespace HumansCompanion.Modules
{
    public class ExampleCommands : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;

        public ExampleCommands(CommandHandler handler)
        {
            _handler = handler;
        }

        [SlashCommand("8ball", "find your answer!")]
        public async Task EightBall(string question)
        {
            var replies = new List<string>
            {
                "yes",
                "no",
                "maybe",
                "hazzzzy...."
            };

            // get the answer
            var answer = replies[new Random().Next(replies.Count - 1)];

            // reply with the answer
            await RespondAsync($"You asked: [**{question}**], and your answer is: [**{answer}**]");
        }
    }
}
