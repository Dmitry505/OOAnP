public class SetPositionCommandTests
{
    public SetPositionCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void TankFull()
    {
        var ships1 = new Mock<IUObject>();
        var ships2 = new Mock<IUObject>();
        var ships3 = new Mock<IUObject>();
        var uObjects = new List<IUObject>{
            ships1.Object,
            ships2.Object,
            ships3.Object,
        };
        var positions = new List<Vector>
        {
            new Vector(0, 0),
            new Vector(0, 10),
            new Vector(0, 20)
        };
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Ships.All", (object[] args) => uObjects).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Ships.SetPosition", (object[] args) => new SetPositionCommand(positions)).Execute();
        IoC.Resolve<ICommand>("Game.Ships.SetPosition").Execute();
        ships1.VerifyAll();
        ships2.VerifyAll();
        ships3.VerifyAll();
    }
}
