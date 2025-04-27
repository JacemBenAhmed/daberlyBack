using Microsoft.AspNetCore.SignalR;

namespace DaberlyProjet
{
    public class AnnonceHub : Hub
    {
        public async Task SendAnnonceCreated(int annonceId)
        {
            await Clients.All.SendAsync("AnnonceCreated", annonceId);
        }
        public async Task SendAnnonceUpdated(int annonceId)
        {
            await Clients.All.SendAsync("AnnonceUpdated",annonceId);
        }

        public async Task SendAnnonceDeleted(int annonceId)
        {
            await Clients.All.SendAsync("AnnonceDeleted",annonceId);
        }
    }

}
