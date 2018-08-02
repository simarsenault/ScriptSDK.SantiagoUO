using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public class DiscordChannelSender
    {
        private string channelId;

        private static readonly HttpClient httpClient = new HttpClient();
        private static DiscordChannelSender instance = new DiscordChannelSender();

        private DiscordChannelSender()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", WindowsRegistry.GetValue(@"Software\ScriptSDK.SantiagoUO\Discord", "BotToken"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            this.channelId = WindowsRegistry.GetValue(@"Software\ScriptSDK.SantiagoUO\Discord", "ChannelId");
        }

        public static void SendMessage(string message)
        {
            var postData = new Dictionary<string, string>
            {
               { "content", message }
            };

            var content = new FormUrlEncodedContent(postData);

            httpClient.PostAsync("https://discordapp.com/api/v6/channels/" + instance.channelId + "/messages", content);
        }
    }
}
