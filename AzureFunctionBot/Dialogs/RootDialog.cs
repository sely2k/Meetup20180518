using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.Forward(new LanguageDialog(), this.ResumeAfterLanguageDialog, activity, CancellationToken.None);
        }

        private async Task ResumeAfterLanguageDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("root.finish");
        }
    }



}