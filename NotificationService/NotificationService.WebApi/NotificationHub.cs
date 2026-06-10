using Microsoft.AspNetCore.SignalR;

namespace NotificationService.WebApi.Hubs;

public class NotificationHub : Hub
{
    public async Task Subscribe(string email)
    {
        if (!string.IsNullOrEmpty(email))
            await Groups.AddToGroupAsync(Context.ConnectionId, email);
    }

    public async Task Unsubscribe(string email)
    {
        if (!string.IsNullOrEmpty(email))
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, email);
    }
}