using AzureFunctionBot.Dialogs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionBot
{
    public static class Message
    {
        [FunctionName("Messages")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "messages")]
            HttpRequestMessage request,
            TraceWriter log
        )
        {
            log.Info("C# HTTP trigger function processed a request.");

            using (BotService.Initialize())
            {
                using (BotService.Initialize())
                {
                    string jsonContent = await request.Content.ReadAsStringAsync();
                    var activity = JsonConvert.DeserializeObject<Activity>(jsonContent);

                    if (!await BotService.Authenticator.TryAuthenticateAsync(request, new[] { activity }, CancellationToken.None))
                    {
                        return BotAuthenticator.GenerateUnauthorizedResponse(request);
                    }

                    if (activity != null)
                    {
                        switch (activity.GetActivityType())
                        {
                            case ActivityTypes.Message:
                                await Conversation.SendAsync(activity, () => new RootDialog());
                                break;

                            case ActivityTypes.ConversationUpdate:
                            case ActivityTypes.ContactRelationUpdate:
                            case ActivityTypes.Typing:
                            case ActivityTypes.DeleteUserData:
                            case ActivityTypes.Ping:
                            default:
                                break;
                        }
                    }

                    return request.CreateResponse(HttpStatusCode.Accepted);
                }
            }
        }
    }
}