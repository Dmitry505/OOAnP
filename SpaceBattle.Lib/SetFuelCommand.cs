public class SetFuelCommand : ICommand
{
    private readonly double _fuelCount;
    public SetFuelCommand(double value) => _fuelCount = value;

    public void Execute()
    {
        var ships = IoC.Resolve<List<IUObject>>("Game.Ships.All");
        foreach (var ship in ships)
        {
            ship.SetProperty("Fuel", _fuelCount);
        }
    }
}
