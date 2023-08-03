using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBot.DAL;
using TelegramBot.Domain.Models;

namespace TelegramBot.Services;

public class BotService
{
	private static ApplicationDBContext _context = GetContext();
	static ITelegramBotClient bot = new TelegramBotClient("YOUR_TOKEN_HERE");
    		//static string groupId = "-906987643";
    		private static string groupId = "YOUR_GROUP_HERE";
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
	            Console.WriteLine($"From {message.From.Id}");

    			if (message.Chat.Id.ToString() == groupId && !message.From.IsBot)
    			{
    				var thread = message.MessageThreadId;
    				if (thread is not null)
                    {
	                    var user = _context.Users.FirstOrDefault(x => x.ThreadId == thread);
	                    if (user is not null)
	                    {
		                    await botClient
			                    .SendTextMessageAsync(chatId: user.ChatId, text: message.Text, cancellationToken: cancellationToken);
	                    }

                    }
    			}
                else
                {
	                if (message.Text.ToLower() == "/start")
	                {
		                await botClient.SendTextMessageAsync(chatId: groupId, $"{message?.From?.Username ?? message?.From?.FirstName} начал общение с ботом");
		                return;
	                }
    			
	                if (message.MessageId == 0 || message.From is null || message.From.Username == null)
		                return;
    			
	                var userByName = _context.Users.FirstOrDefault(x => x.UserName == message.From.Username);
	                if (userByName is null)
	                {
		                var topic = await botClient.CreateForumTopicAsync(chatId: groupId, name: message.From.Username);
		                await AddUserToDb(message.Chat.Id, message.From.Username, topic.MessageThreadId);
		                userByName = _context.Users.FirstOrDefault(x => x.UserName == message.From.Username);
	                }
	                await botClient
		                .ForwardMessageAsync(
			                chatId: groupId, 
			                messageThreadId: userByName?.ThreadId, 
			                fromChatId: message.Chat.Id,
			                messageId: message.MessageId); 
                }
    			
            }
    	}
	    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
	    {
		    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
	    }

	    public static void StartBot()
	    {
		    var cts = new CancellationTokenSource();
		    var cancellationToken = cts.Token;
		    var receiverOptions = new ReceiverOptions
		    {
			    AllowedUpdates = { }, 
		    };
		    Console.WriteLine("Starting bot");
		    bot.StartReceiving(
			    HandleUpdateAsync,
			    HandleErrorAsync,
			    receiverOptions,
			    cancellationToken
		    );
		    
		    
	    }

	    public static void StopBot()
	    {
		    Console.WriteLine("Stopping bot");
		    bot.CloseAsync();
	    }

	    private static async Task AddUserToDb(long chat,string user,int thread)
	    {
		    _context.Users.Add(new Domain.Models.User()
		    {
				ChatId = chat,
				UserName = user,
				ThreadId = thread
		    });
		    await _context.SaveChangesAsync();
	    }

	    private static ApplicationDBContext GetContext()
	    {
		    var options = new DbContextOptionsBuilder<ApplicationDBContext>();
		    options.UseNpgsql("UID=postgres;Password=1111;Host=localhost;Port=5432;Database=telegram;");
		    return new ApplicationDBContext(options.Options);
	    }
}