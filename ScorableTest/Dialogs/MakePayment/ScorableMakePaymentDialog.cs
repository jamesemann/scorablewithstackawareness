using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ScorableTest.Dialogs.Balance;

namespace ScorableTest.Dialogs
{
    [Serializable]
    public class ScorableMakePaymentDialog : IDialog<object>, IRememberLastMessageSentToUser
    {
        protected string payee;
        protected string amount;

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"[ScorableMakePaymentDialog] Who would you like to pay?");
            LastMessageSentToUser = $"[ScorableMakePaymentDialog] Who would you like to pay?";
            // State transition - wait for 'payee' message from user
            context.Wait(MessageReceivedPayee);
        }

        public string LastMessageSentToUser { get; set; }

        public async Task MessageReceivedPayee(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            this.payee = message.Text;

            await context.PostAsync($"[ScorableMakePaymentDialog] {this.payee}, got it{Environment.NewLine}How much should I pay?");
            LastMessageSentToUser = $"[ScorableMakePaymentDialog] {this.payee}, got it{Environment.NewLine}How much should I pay?";

            // State transition - wait for 'amount' message from user
            context.Wait(MessageReceivedAmount);
        }

        public async Task MessageReceivedAmount(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            this.amount = message.Text;

            await context.PostAsync($"[ScorableMakePaymentDialog] Thank you, I've paid {this.amount} to {this.payee} 💸");

            IRememberLastMessageSentToUser lastMessageSentToUser = null;
            foreach (var frame in context.Frames)
            {
                if (frame.Target.GetType() != this.GetType() && frame.Target is IRememberLastMessageSentToUser)
                {
                    lastMessageSentToUser = frame.Target as IRememberLastMessageSentToUser;
                    break;
                }
            }

            if (lastMessageSentToUser != null)
            {
                await context.PostAsync("** LAST MESSAGE: " + lastMessageSentToUser.LastMessageSentToUser);
            }

            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}