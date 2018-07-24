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
    public class QandADialog : IDialog<object>
    {
        private QandAService _QnAService =
        new QandAService
            (
                ConfigurationManager.AppSettings["QnAHost"],
                ConfigurationManager.AppSettings["QnAKnowledgebaseId"],
                ConfigurationManager.AppSettings["QnAEndPointKey"]
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
            }
            else
            {
                await context.PostAsync(answer);
            }
        }
    }
}



            //    {
            //        private string _qandaServiceHostName;
            //        private string _knowledgeBaseId;
            //        private string _endpointKey;

//        //CREATE THE Q AND A DIALOG
//        public QandADialog(string hostname, string knowledgeBasedId, string endpointKey)
//        {
//            _qandaServiceHostName = hostname;
//            _knowledgeBaseId = knowledgeBasedId;
//            _endpointKey = endpointKey;
//        }

//        //CALL THE Q AND A ENDPOINT AND GET A RESPONSE
//        public async Task<string> GetAnswer(string query)
//        {
//            var client = new HttpClient() {
//            };

//            //Add a base address to the httpClient
//            //double check that the _qandaServices starts with a https://
//            string baseUrlString = String.Format("{0}/knowledgebases/{1}/generateAnswer", _qandaServiceHostName,_knowledgeBaseId);
//            var baseURI = new Uri(baseUrlString);
//            client.BaseAddress = baseURI;

//            //CREATE THE NECESSARY AUTH + HEADERS
//            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("authorization", "EndpointKey " + _endpointKey);
//            client.DefaultRequestHeaders.Add("content-type", "application/json");
////          HttpRequestMessage myPostCreateAccessPolicyRequest = new HttpRequestMessage(HttpMethod.Post, String.Format("https://xamcammediaservice.restv2.westus.media.azure.net/api/AccessPolicies"));

//            var request = new HttpRequestMessage() { };
//            request.Method = HttpMethod.Post;

//            var requestQueryQandAObject = new RootObject() {
//                question = query
//            };

//            string serializedRequestQueryQandAObject = JsonConvert.SerializeObject(requestQueryQandAObject);
//            request.Content = new StringContent( serializedRequestQueryQandAObject, Encoding.UTF8, "application/json");

//            //RetrieveResponseMessage(request);
//            var response = await client.SendAsync(request);

//            //var responseResponse = await response;
//            if (response.StatusCode == System.Net.HttpStatusCode.OK)
//            {

//                //EXTRACT THE RESPONSE FROM HTTP RESPONSE MESSAGE
//                var responseContentAsString = response.Content.ReadAsStringAsync().Result;

//                // Deserialize the response JSON                 
//                QnAAnswer answer = JsonConvert.DeserializeObject<QnAAnswer>(responseContentAsString);

//                // Return the answer if present                 
//                if (answer.Answers.Count > 0)
//                {
//                    if
//                    (answer.Answers[0].answer != "No good match found in KB.")
//                    { return answer.Answers[0].answer; }
//                    else
//                    {
//                        return "";
//                    }
//                }
//                else
//                {
//                    return "";
//                }
//            }
//            else
//            {
//                throw new Exception
//                    ($"QnAMaker call failed with status code {response.StatusCode.ToString()} { response.ReasonPhrase })            
//            }
//        }

//        public Task StartAsync(IDialogContext context)
//        {
//            context.Wait(MessageReceivedAsync);

//            return Task.CompletedTask;
//        }

//        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
//        {
//            var activity = await result as IMessageActivity;

//            // TODO: Put logic for handling user message here

//            context.Wait(MessageReceivedAsync);
//        }






//public class RootObject
//{
//    public string question { get; set; }
//}

//public class Metadata { public string Name { get; set; } public string Value { get; set; } }

//public class Answer { public IList<string> Questions { get; set; } public string answer { get; set; } public double Score { get; set; } public int Id { get; set; } public string Source { get; set; } public IList<object> Keywords { get; set; } public IList<Metadata> Metadata { get; set; } }

//public class QnAAnswer { public IList<Answer> Answers { get; set; } }



//async Task<HttpResponseMessage> RetrieveResponseMessage(HttpResponseMessage hrm)
//{
//    // GetAsync returns a Task<HttpResponseMessage>.   
//    HttpResponseMessage response = await client.GetAsync(url, ct);

//    // Retrieve the website contents from the HttpResponseMessage.  
//    byte[] urlContents = await response.Content.ReadAsByteArrayAsync();

//    return urlContents.Length;
//}
