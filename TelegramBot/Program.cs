using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBot.DAL;
using TelegramBot.Services;


namespace TelegramBot;

public class Program
{
	/*static ITelegramBotClient bot = new TelegramBotClient("6543562600:AAEFKoiyX6bohbNoleUGZPeBWFReiDgMlpM");
		//static string groupId = "-906987643";
		private static string groupId = "-1001838930024";
		private static Dictionary<string, int> UsersIDs = new Dictionary<string, int>();
	public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
	{
		if (update.Message is null || update.Message.Text is null || update.Message.From is null) 
		{
			return;
		}
		Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
		
		if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
		{
			var message = update.Message;
			if (message.Chat.Id.ToString() == groupId)
			{
				var thread = message.MessageThreadId;
				if (thread is not null)
				{
					if (UsersIDs.ContainsValue(thread.Value))
					{
						
					}
					
				}
			}
			if (message.Text.ToLower() == "/start")
			{
				await botClient.SendTextMessageAsync(chatId: groupId, $"{message?.From?.Username ?? message?.From?.FirstName} начал общение с ботом");
				return;
			}
			
			if (message.MessageId == 0 || message.From is null || message.From.Username == null)
				return;
			if (!UsersIDs.ContainsKey(message.From.Username))
			{
				var topic = await botClient.CreateForumTopicAsync(chatId: groupId, name: message.From.Username);
				UsersIDs.Add(message.From.Username, topic.MessageThreadId);
				await botClient.ForwardMessageAsync(chatId: groupId, messageThreadId: UsersIDs[message.From.Username], fromChatId: message?.From?.Id,messageId: message.MessageId);
				var chat = await botClient.GetChatAsync(groupId, cancellationToken);
			}
			else
			{
				await botClient.ForwardMessageAsync(chatId: groupId, messageThreadId: UsersIDs[message.From.Username], fromChatId: message?.From?.Id,messageId: message.MessageId);
				
			}
			
			
		}
	}

	public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
	{
		Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
	}*/


	static  void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder();
		
		builder.Services.AddSingleton<BotService>();
		builder.Services.AddDbContext<ApplicationDBContext>(opt => 
			opt.UseNpgsql(builder.Configuration.GetConnectionString("Old")));
		
		BotService.StartBot();
		var app = builder.Build();
		app.Run();

		

	}
}