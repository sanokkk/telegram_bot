using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Domain.Models;

public class User
{
    [Key]
    public long ChatId { get; set; }

    public string UserName { get; set; }

    public int ThreadId { get; set; }
}