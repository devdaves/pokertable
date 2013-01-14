using Microsoft.AspNet.SignalR.Hubs;

namespace PokerTable.Web.Hubs
{
    /// <summary>
    /// Poker Table Hub
    /// </summary>
    public class PokerTableHub : Hub
    {
        /// <summary>
        /// Refresh 
        /// </summary>
        public void RefreshTable()
        {
            Clients.All.refreshTable();
        }
    }
}