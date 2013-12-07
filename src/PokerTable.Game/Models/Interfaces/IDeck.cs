using System.Collections.Generic;

namespace PokerTable.Game.Models.Interfaces
{
    public interface IDeck
    {
        List<Card> Cards { get; set; }
    }
}
