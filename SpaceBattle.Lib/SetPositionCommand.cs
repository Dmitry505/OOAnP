public class SetPositionCommand : ICommand
{
    private readonly List<Vector> _positions;
    public SetPositionCommand(List<Vector> PositionList) => _positions = PositionList;

    public void Execute()
    {
        var ships = IoC.Resolve<List<IUObject>>("Game.Ships.All");

        for (var i = 0; i < ships.Count; i++)
        {
            ships[i].SetProperty("Position", _positions[i]);
        }
    }
}
