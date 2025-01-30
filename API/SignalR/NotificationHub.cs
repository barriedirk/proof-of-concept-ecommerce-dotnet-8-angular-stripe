using System.Collections.Concurrent;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class NotificationHub : Hub
{
  // this is a example, for a single server, for multiple servers, you need another solution

  private static readonly ConcurrentDictionary<string, string> UserConnections = new();

  public override Task OnConnectedAsync()
  {
    var email = Context.User?.GetEmail();

    if (!string.IsNullOrEmpty(email)) UserConnections[email] = Context.ConnectionId;

    return base.OnConnectedAsync();
  }

  public override Task OnDisconnectedAsync(Exception? exception)
  {
    var email = Context.User?.GetEmail();

    if (!string.IsNullOrEmpty(email)) UserConnections.TryRemove(email, out _);

    return base.OnDisconnectedAsync(exception);
  }

  public static string? GetConnectionIdByEmail(string email)
  {
    UserConnections.TryGetValue(email, out var connectionId);

    return connectionId;
  }
}