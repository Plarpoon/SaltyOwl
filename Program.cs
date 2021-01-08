using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WoM_Balance_Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            _ = new GoogleAPI();
            GoogleAPI.Sheets();

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.Log += Log;

            var token = File.ReadAllText("bot-token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        // Bot ignore policies and main trigger.
        private Task CommandHandler(SocketMessage message)
        {
            //  variables
            string command = "";
            int lengthOfCommand = -1;

            //  filtering messages begin here
            if (!message.Content.StartsWith('!'))
                return Task.CompletedTask;

            if (!message.Author.IsBot)
            {
                if (message.Content.Contains(' '))
                    lengthOfCommand = message.Content.IndexOf(' ');
                else
                    lengthOfCommand = message.Content.Length;

                command = message.Content[1..lengthOfCommand].ToLower();

                //  Commands begin here
                if (command.Equals("info")) //  prints infos about the bot
                {
                    message.Channel.SendMessageAsync($@"Hello {message.Author.Mention}, I am here to assist the WoM staff!");
                }
                else if (command.Equals("balance"))
                {
                    message.Channel.SendMessageAsync($@"Your account was created at {message.Author.CreatedAt.DateTime.Date}");
                }

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}