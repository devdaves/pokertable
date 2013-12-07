namespace PokerTable.Game.Models.Interfaces
{
    public interface ICard
    {
        Card.Colors Color { get; set; }

        Card.States State { get; set; }

        Card.Suites Suite { get; set; }

        int Value { get; set; }

        string Name();
    }
}
