using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionBot.Dialogs
{
    [LuisModel("****************", "***********", Log = true)]
    [Serializable]
    public class LanguageDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task FallbackIntentHandlerAsync(IDialogContext context, LuisResult result)
        {
            await context.SayAsync($"Non riesco ad interpretare il messaggio");
            context.Done("None");
        }

        #region MakeCoffeeDialog

        [LuisIntent("make.coffee")]
        public async Task MakeCoffee(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var activity = await message as Activity;
            await context.Forward(new MakeCoffeeDialog(), this.ResumeAfterMakeCoffeeDialog, activity, CancellationToken.None);
        }

        private async Task ResumeAfterMakeCoffeeDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("MakeCoffeeDialog.ResumeAftHelp");
        }

        #endregion MakeCoffeeDialog
    }
}