using Microsoft.AspNet.SignalR.Hubs;

namespace PokerTable.Web.Hubs
{
    /// <summary>
    /// Poker Table Hub
    /// </summary>
    public class PokerTableHub : Hub
    {
        /// <summary>
        /// Join a group
        /// </summary>
        /// <param name="groupName">Name of group to join</param>
        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Leave a group
        /// </summary>
        /// <param name="groupName">Name of Group to leave.</param>
        public void LeaveGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
        }
    }
}