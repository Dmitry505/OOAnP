using Hwdtech;

public class SetFuelCommand : ICommand
{
    public readonly int fuelCount;
    public SetFuelCommand(int fuel) => fuelCount = fuel;

    public void Execute()
    {
        var ships = IoC.Resolve<List<IUObject>>("Game.Ships.All");
        ships.ForEach(ship => ship.SetProperty("Fuel", fuelCount));
    }
}
