using Microsoft.EntityFrameworkCore;
using TelegramBot.DAL;
using TelegramBot.Services;


var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton<BotService>();
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Old")));


var app = builder.Build();
BotService.StartBot();

app.Run();
Console.ReadKey();
BotService.StopBot();