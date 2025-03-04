public class SetPositionCommand : ICommand
{
    private readonly List<dynamic> _positions;
    public SetPositionCommand(List<dynamic> PositionList) => _positions = PositionList;

    public void Execute()
    {
        var ships = IoC.Resolve<List<IUObject>>("Game.Ships.All");

        for (var i = 0; i < ships.Count; i++)
        {
            ships[i].SetProperty("Position", _positions[i]);
        }
    }
}
