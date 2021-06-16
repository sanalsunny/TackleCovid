using CowinAvailabilityTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (AppContext.googleChatWebhookUrl.Length > 0)
            {
                var text = new Dictionary<string, string>() { { "text", message } };
                var json = JsonConvert.SerializeObject(text);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    await client.PostAsync(AppContext.googleChatWebhookUrl, data);
                }
            }
        }

        private static async Task PostToTelegram(string message)
        {
            var getUrl = $"https://api.telegram.org/bot{AppContext.telegram_ApiToken}/getUpdates";
            var postUrl = $"https://api.telegram.org/bot{AppContext.telegram_ApiToken}/sendMessage";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(getUrl);
                var jsonString = await response.Content.ReadAsStringAsync();
                var activeMessages = JsonConvert.DeserializeObject<TelegramMessageModel>(jsonString);

                var chatIds = activeMessages.Result.Where(x => x.Message != null).Select(x => x.Message.From.Id).Distinct();

                foreach (var chatId in chatIds)
                {
                    var parameters = new Dictionary<string, string> { { "chat_id", chatId }, { "text", message } };
                    var encodedContent = new FormUrlEncodedContent(parameters);
                    await client.PostAsync(postUrl, encodedContent);
                }
            }
        }
    }
}