using DaberlyProjet.DTO;
using DaberlyProjet.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DaberlyProjet.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageCreated(int messageId)
        {
            await Clients.All.SendAsync("MessageCreated", messageId);
        }


    }
}