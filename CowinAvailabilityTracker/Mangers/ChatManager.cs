using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CowinAvailabilityTracker.Mangers
{
    public class ChatManager
    {
        public static async Task PostToChatAsync(string message, bool newCentre = false)
        {
            if (AppContext.chatWebhookUrl.Length > 0)
            {

                var messageList = new List<string>() { newCentre ? "New Center Available" : null, Environment.NewLine, message };

                var text = new Dictionary<string, string>() { { "text", String.Join(Environment.NewLine, messageList.ToArray()) } };

                var json = JsonConvert.SerializeObject(text);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    await client.PostAsync(AppContext.chatWebhookUrl, data);
                }
            }
        }
    }
}
