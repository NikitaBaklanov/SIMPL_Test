using Microsoft.AspNetCore.SignalR;

namespace NotificationService.WebApi.Hubs;

/// <summary>
/// SignalR Hub для уведомлений.
/// Клиенты подписываются на группу по своему email и получают сообщения об оплате.
/// </summary>
public class NotificationHub : Hub
{
    /// <summary>
    /// Подписка текущего подключения на получение уведомлений для указанного email.
    /// </summary>
    /// <param name="email">Email клиента (используется как имя группы).</param>
    public async Task Subscribe(string email)
    {
        if (!string.IsNullOrEmpty(email))
            await Groups.AddToGroupAsync(Context.ConnectionId, email);
    }

    /// <summary>
    /// Отписка от группы.
    /// </summary>
    public async Task Unsubscribe(string email)
    {
        if (!string.IsNullOrEmpty(email))
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, email);
    }
}