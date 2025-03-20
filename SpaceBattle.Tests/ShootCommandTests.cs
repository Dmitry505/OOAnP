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

        var ships1 = new Mock<IUObject>();
        ships1.Setup(m => m.GetProperty("Position")).Returns((10, 20));
        ships1.Setup(m => m.GetProperty("Angle")).Returns(45);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Speed", (object[] args) => (object)10).Execute();

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Creation", (object[] args) => testShot.Object).Execute();

        var startMoveCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.StartMove", (object[] args) =>
        { return startMoveCommand.Object; }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootCommand((IUObject)args[0]); }).Execute();

        IoC.Resolve<ShootCommand>("Game.Object.Shot", ships1.Object).Execute();

        testShot.Verify(m => m.SetProperty("Position", It.Is<(int, int)>(pos => pos.Item1 == 10 && pos.Item2 == 20)), Times.Once);
        testShot.Verify(m => m.SetProperty("Angle", 45), Times.Once);
        testShot.Verify(m => m.SetProperty("Speed", 10), Times.Once);
        startMoveCommand.Verify(m => m.Execute(), Times.Once);
    }

    [Fact]
    public void CreatingShotMissingShipReturnNullReferenceException()
    {

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Creation", (object[] args) => testShot.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Speed", (object[] args) => (object)10).Execute();

        var startMoveCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.StartMove", (object[] args) =>
        { return startMoveCommand.Object; }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootCommand((IUObject)args[0]); }).Execute();

        Assert.Throws<NullReferenceException>(() => IoC.Resolve<ShootCommand>("Game.Object.Shot", null!).Execute());
    }

    [Fact]
    public void CreatingShotShipNullAngleReturnArgumentNullException()
    {

        var ships1 = new Mock<IUObject>();
        ships1.Setup(m => m.GetProperty("Position")).Returns((10, 20));
        ships1.Setup(m => m.GetProperty("Angle")).Throws(new ArgumentNullException());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Speed", (object[] args) => (object)10).Execute();

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Creation", (object[] args) => testShot.Object).Execute();

        var startMoveCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.StartMove", (object[] args) =>
        { return startMoveCommand.Object; }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootCommand((IUObject)args[0]); }).Execute();

        Assert.Throws<ArgumentNullException>(() => IoC.Resolve<ShootCommand>("Game.Object.Shot", ships1.Object).Execute());
    }

    [Fact]
    public void CreatingShotNullSpeedReturnNullReferenceException()
    {

        var ships1 = new Mock<IUObject>();
        ships1.Setup(m => m.GetProperty("Position")).Returns((10, 20));
        ships1.Setup(m => m.GetProperty("Angle")).Returns(45);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Speed", (object[] args) => (object)null!).Execute();

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Creation", (object[] args) => testShot.Object).Execute();

        var startMoveCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.StartMove", (object[] args) =>
        { return startMoveCommand.Object; }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootCommand((IUObject)args[0]); }).Execute();

        Assert.Throws<NullReferenceException>(() => IoC.Resolve<ShootCommand>("Game.Object.Shot", ships1.Object).Execute());

    }
}
