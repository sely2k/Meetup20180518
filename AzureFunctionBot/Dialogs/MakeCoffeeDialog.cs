using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionBot.Dialogs
{
    [Serializable]
    public class MakeCoffeeDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var message = context.MakeMessage();

            //#region Standard
            //message.Text = $"Ok Preparo un caffè";
            //#endregion

            #region Card

            if (message.Attachments == null)
                message.Attachments = new List<Attachment>();
            var attachment = new Attachment()
            {
                Content = GetAdaptiveCard(), //GetHeroCard(),
                ContentType = "application/vnd.microsoft.card.adaptive",
                Name = "nomeCarta"
            };
            message.Attachments.Add(attachment);

            await context.PostAsync(message);

            #endregion Card

            context.Done("MakeCoffeeDialog.finish");
        }

        private Microsoft.Bot.Connector.Attachment GetHeroCard()
        {
            HeroCard hc = new HeroCard()
            {
                Text = "Preparo *un* caffè",
            };

            return hc.ToAttachment();
        }

        private AdaptiveCard GetAdaptiveCard()
        {
            AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                    {
                        Items = new List<AdaptiveElement>()
                        {
                            new AdaptiveColumnSet()
                            {
                                Columns = new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {
                                        Width = AdaptiveColumnWidth.Auto,
                                        Items = new List<AdaptiveElement>()
                                        {
                                            new AdaptiveImage()
                                            {
                                                Url = new Uri("https://placeholdit.imgix.net/~text?txtsize=65&txt=Adaptive+Cards&w=300&h=300"),
                                                Size = AdaptiveImageSize.Medium,
                                                Style = AdaptiveImageStyle.Person
                                            }
                                        }
                                    },
                                    new AdaptiveColumn()
                                    {
                                        Width = AdaptiveColumnWidth.Stretch,
                                        Items = new List<AdaptiveElement>()
                                        {
                                            new AdaptiveTextBlock()
                                            {
                                                Text =  "Ciao!",
                                                Weight = AdaptiveTextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new AdaptiveTextBlock()
                                            {
                                                Text = "Ti Preparu un caffè",
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
            };

            return card;
        }
    }
}