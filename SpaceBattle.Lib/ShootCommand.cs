public class ShootCommand
{
    public readonly PlayObject ship;
    public ShootCommand(PlayObject Ship) => ship = Ship;
    public PlayObject Shot()
    {
        var position = ship.GetProperty("Position");
        var angle = ship.GetProperty("Angle");
        var speed = 10;

        var shot = IoC.Resolve<PlayObject>("Game.Object.Register");
        shot.SetProperty("Position", position);
        shot.SetProperty("Angle", angle);
        shot.SetProperty("Speed", speed);

        return shot;
    }
}

