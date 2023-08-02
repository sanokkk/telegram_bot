using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;


namespace TelegramBot;

public class Program
{
	static ITelegramBotClient bot = new TelegramBotClient("6543562600:AAEFKoiyX6bohbNoleUGZPeBWFReiDgMlpM");
	public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
	{
		if (update.Message.Text == null)
		{
			return;
		}
		Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
		if (update is null)
			return;
		if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
		{
			var message = update.Message;
			if (message.Text.ToLower() == "/start")
			{
				await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
				return;
			}
			await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
			string groupId = "-906987643";
			//var chat = await botClient.GetChatAsync(groupId);
			//var chatTitle = message.From.Username;
			
			await botClient.ForwardMessageAsync(chatId: groupId, fromChatId: message?.From.Id.ToString(),messageId: message.MessageId);
			
			
			
			
		}
	}

	public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
	{
		Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
	}


	static  void Main(string[] args)
	{

		Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

		var cts = new CancellationTokenSource();
		var cancellationToken = cts.Token;
		var receiverOptions = new ReceiverOptions
		{
			AllowedUpdates = { }, 
		};
		bot.StartReceiving(
			HandleUpdateAsync,
			HandleErrorAsync,
			receiverOptions,
			cancellationToken
		);
		Console.ReadLine();
		bot.CloseAsync();

	}
}