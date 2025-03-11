public class ShootRegister
{
    public readonly IUObject ship;
    public ShootRegister(IUObject Ship) => ship = Ship;
    public void Shot()
    {
        var position = ship.GetProperty("Position");
        var angle = ship.GetProperty("Angle");
        var speed = 10;

        var shot = IoC.Resolve<IUObject>("Game.Object.Register");
        shot.SetProperty("Position", position);
        shot.SetProperty("Angle", angle);
        shot.SetProperty("Speed", speed);
    }
}

