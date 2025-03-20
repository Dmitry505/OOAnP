public class ShootCommand : ICommand
{
    public readonly IUObject ship;
    public ShootCommand(IUObject Ship) => ship = Ship;
    public void Execute()
    {
        var position = ship.GetProperty("Position");
        var angle = ship.GetProperty("Angle");
        var speed = IoC.Resolve<int>("Game.Object.Speed");

        var shot = IoC.Resolve<IUObject>("Game.Object.Creation");
        shot.SetProperty("Position", position);
        shot.SetProperty("Angle", angle);
        shot.SetProperty("Speed", speed);

        IoC.Resolve<ICommand>("Game.Object.StartMove", shot).Execute();
    }
}
