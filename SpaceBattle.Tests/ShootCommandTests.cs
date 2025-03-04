public class ShootCommandTests
{
    public ShootCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void ShotTest()
    {

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Register", (object[] args) => new PlayObject()).Execute();
        var ships1 = IoC.Resolve<PlayObject>("Game.Object.Register");
        ships1.SetProperty("Position", (10, 20));
        ships1.SetProperty("Angle", 45);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootCommand((PlayObject)args[0]); }).Execute();

        var shot = IoC.Resolve<ShootCommand>("Game.Object.Shot", ships1).Shot();
        var shotPosition = shot.GetProperty("Position");
        var shotAngel = shot.GetProperty("Angle");
        var ShotSpeed = shot.GetProperty("Speed");

        Assert.Equal(shotPosition, (10, 20));
        Assert.Equal(shotAngel, 45);
        Assert.Equal(ShotSpeed, 10);
    }
}
