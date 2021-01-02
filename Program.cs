using Discord;

namespace WoM_Balance_Bot
{
	public class Program
	{
		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

             var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
    }
}