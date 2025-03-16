using Moq;

public class ShootRegisterTests
{
    public ShootRegisterTests()
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

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Register", (object[] args) => testShot.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootRegister((IUObject)args[0]); }).Execute();

        IoC.Resolve<ShootRegister>("Game.Object.Shot", ships1.Object).Shot();

        testShot.Verify(m => m.SetProperty("Position", It.Is<(int, int)>(pos => pos.Item1 == 10 && pos.Item2 == 20)), Times.Once);
        testShot.Verify(m => m.SetProperty("Angle", 45), Times.Once);
        testShot.Verify(m => m.SetProperty("Speed", 10), Times.Once);
    }

    [Fact]
    public void CreatingShotMissingShipReturnNullReferenceException()
    {

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Register", (object[] args) => testShot.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootRegister((IUObject)args[0]); }).Execute();

        Assert.Throws<NullReferenceException>(() => IoC.Resolve<ShootRegister>("Game.Object.Shot", null!).Shot());
    }

    [Fact]
    public void CreatingShotShipNullAngleReturnNullReferenceException()
    {

        var ships1 = new Mock<IUObject>();
        ships1.Setup(m => m.GetProperty("Position")).Returns((10, 20));
        ships1.Setup(m => m.GetProperty("Angle")).Throws(new ArgumentNullException());

        var testShot = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Register", (object[] args) => testShot.Object).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Shot", (object[] args) =>
        { return new ShootRegister((IUObject)args[0]); }).Execute();

        Assert.Throws<ArgumentNullException>(() => IoC.Resolve<ShootRegister>("Game.Object.Shot", ships1.Object).Shot());
    }
}
