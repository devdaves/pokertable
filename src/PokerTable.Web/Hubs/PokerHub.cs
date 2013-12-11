using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PokerTable.Game;
using PokerTable.Game.Models;

namespace PokerTable.Web.Hubs
{
    public class PokerHub : Hub
    {
        private IEngine engine;

        public PokerHub()
        {
            this.engine = new Engine();
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
        }

        public Table GetTable(Guid tableId)
        {
            this.engine.LoadTable(tableId);
            return this.engine.Table;
        }
    }
}