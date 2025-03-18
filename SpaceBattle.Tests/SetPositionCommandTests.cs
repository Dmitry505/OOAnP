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

        var positionIterator = new PositionIterator(uObjects.Count);
        var positionList = new List<dynamic>();

        foreach (var i in positionIterator)
        {
            positionList.Add(i);
        }

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Ships.All", (object[] args) => uObjects).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Ships.SetPosition", (object[] args) => new SetPositionCommand(positionList)).Execute();
        IoC.Resolve<ICommand>("Game.Ships.SetPosition").Execute();

        ships1.Verify(m => m.SetProperty("Position", It.IsAny<object>()), Times.Once);
        ships2.Verify(m => m.SetProperty("Position", It.IsAny<object>()), Times.Once);
        ships3.Verify(m => m.SetProperty("Position", It.IsAny<object>()), Times.Once);

    }
}
