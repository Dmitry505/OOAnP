public class SetPositionCommand : ICommand
{
    private readonly List<dynamic> _positions;
    public SetPositionCommand(List<dynamic> PositionList) => _positions = PositionList;

    public void Execute()
    {
        var ships = IoC.Resolve<List<IUObject>>("Game.Ships.All");

        ships.Select((ship, index) =>
        {
            ship.SetProperty("Position", _positions[index]);
            return ship;
        }).ToList();
    }
}
