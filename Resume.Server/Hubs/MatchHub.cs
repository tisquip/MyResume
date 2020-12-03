using Microsoft.AspNetCore.SignalR;
using Resume.Application;
using System.Threading.Tasks;

namespace Resume.Server.Hubs
{
    public class MatchHub : Hub
    {
        public async Task SendMatchUpdate(string jsLiveMatchStats)
        {
            await Clients.All.SendAsync(Variables.SignalRMethodName_LiveMatch, jsLiveMatchStats);
        }

    }
}
