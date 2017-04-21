using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ScorableTest.Dialogs.Balance;

namespace ScorableTest.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>, IRememberLastMessageSentToUser
    {
        public string LastMessageSentToUser { get; set; }

        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync($"[RootDialog] I am the root dialog.");
            LastMessageSentToUser = $"[RootDialog] I am the root dialog.";
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            if (activity.Type == ActivityTypes.Message)
            {
                int length = (activity.Text ?? string.Empty).Length;
                await context.PostAsync($"[RootDialog] You sent {activity.Text} which was {length} characters");
                LastMessageSentToUser = $"[RootDialog] You sent {activity.Text} which was {length} characters";
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}