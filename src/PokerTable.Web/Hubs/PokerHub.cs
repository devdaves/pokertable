using Microsoft.AspNet.SignalR;

namespace PokerTable.Web.Hubs
{
    public class PokerHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}