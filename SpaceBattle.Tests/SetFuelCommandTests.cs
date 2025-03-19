public class SetFuelCommandTests
{
    public SetFuelCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void TrueSetFuel()
    {
        var ships1 = new Mock<IUObject>();
        var ships2 = new Mock<IUObject>();
        var ships3 = new Mock<IUObject>();
        var uObjects = new List<IUObject>{
            ships1.Object,
            ships2.Object,
            ships3.Object,
        };
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Ships.All", (object[] args) => uObjects).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Ships.SetFuel", (object[] args) => new SetFuelCommand(10)).Execute();
        IoC.Resolve<ICommand>("Game.Ships.SetFuel").Execute();
        ships1.Verify(m => m.SetProperty("Fuel", 10), Times.Once);
        ships2.Verify(m => m.SetProperty("Fuel", 10), Times.Once);
        ships3.Verify(m => m.SetProperty("Fuel", 10), Times.Once);
    }
}
