using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LuisAndQandA1.Dialogs
{
    [Serializable]
    public class CoordinatedQandADialog : IDialog<bool> //IDialog<object>
    {
        private QandAService _QnAService =
        new QandAService
            (
                ConfigurationManager.AppSettings["QandAHost1"],
                ConfigurationManager.AppSettings["QandAKnowledgebaseId1"],
                ConfigurationManager.AppSettings["QandAEndPointKey1"]
            );

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedASync);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedASync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string safeText = HttpUtility.UrlEncode(((IMessageActivity)context.Activity).Text);
            string answer = await _QnAService.GetAnswer(safeText);
            if (string.IsNullOrEmpty(answer))
            {
                await context.PostAsync("No good match found in KB.");
                context.Done(false);
            }
            else
            {
                await context.PostAsync(answer);
                context.Done(true);
            }
        }
    }
}