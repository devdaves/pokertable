using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;

namespace PokerTable.Web
{
    public class PokerTableHub : Hub
    {
        public string Send(string data)
        {
            return data;
        }
    }
}