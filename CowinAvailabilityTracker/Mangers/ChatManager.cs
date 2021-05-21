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
            var messageList = new List<string>() { newCentre ? "New Center Available" : null, Environment.NewLine, message };
            var formattedMessage = String.Join(Environment.NewLine, messageList.ToArray());

            if (AppContext.notifyOn == Constants.Enums.NotifyOn.GoogleChat)
            {
                await PostToGoogleChat(formattedMessage);
            }
            else if (AppContext.notifyOn == Constants.Enums.NotifyOn.Telegram)
            {
                await PostToTelegram(formattedMessage);
            }
        }

        private static async Task PostToGoogleChat(string message)
        {
            if (AppContext.GoogleChatWebhookUrl.Length > 0)
            {
                var text = new Dictionary<string, string>() { { "text", message } };
                var json = JsonConvert.SerializeObject(text);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    await client.PostAsync(AppContext.GoogleChatWebhookUrl, data);
                }
            }
        }

        private static async Task PostToTelegram(string message)
        {
            var url = $"https://api.telegram.org/bot{AppContext.telegram_ApiToken}/sendMessage";
            var parameters = new Dictionary<string, string> { { "chat_id", AppContext.telegram_ChatId }, { "text", message } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (var client = new HttpClient())
            {
                await client.PostAsync(url, encodedContent);
            }
        }
    }
}