using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using PokerTable.Game;
using PokerTable.Game.Models;
using PokerTable.Web.Models.JsonModels;

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

        public CreateTableJson CreateTable(string tableName)
        {
            return this.FillResponse<CreateTableJson>(r =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    r.Status = 1;
                    r.FailureMessage = "Table Name is required.";
                }
                else
                {
                    this.engine.CreateNewTable(10, tableName);
                    r.TableId = this.engine.Table.Id;
                }
            }, refresh: false);
        }

        public JoinTableJson JoinTable(string tableCode, string playerName)
        {
            return this.FillResponse<JoinTableJson>(r =>
            {
                var playerId = this.engine.JoinTable(tableCode, playerName);
                r.TableId = this.engine.Table.Id;
                r.PlayerId = playerId;

                Clients.Group(this.engine.Table.Id.ToString()).playerJoined(playerName);
            }, refresh: false);
        }

        public AddPlayerToSeatJson AddPlayerToSeat(Guid tableId, int seatId, Guid playerId)
        {
            return this.FillResponse<AddPlayerToSeatJson>(r =>
            {
                this.engine.LoadTable(tableId);
                this.engine.AssignSeatToPlayer(seatId, playerId);
            });
        }

        public RemovePlayerFromSeatJson RemovePlayerFromSeat(Guid tableId, int seatId)
        {
            return this.FillResponse<RemovePlayerFromSeatJson>(r =>
            {
                this.engine.LoadTable(tableId);
                this.engine.RemovePlayerFromSeat(seatId);
            });
        }

        public SetDealerJson SetDealer(Guid tableId, int seatId)
        {
            return this.FillResponse<SetDealerJson>(r =>
            {
                this.engine.LoadTable(tableId);
                this.engine.SetDealer(seatId);
            });
        }

        private TResponse FillResponse<TResponse>(Action<TResponse> action, bool refresh = true)
            where TResponse : JsonBase, new()
        {
            var response = new TResponse();

            try
            {
                action.Invoke(response);

                if (refresh)
                {
                    Clients.Group(this.engine.Table.Id.ToString()).refresh();
                }
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.FailureMessage = ex.Message;
            }

            return response;
        }
    }
}