global using Hwdtech;
global using Hwdtech.Ioc;
using Moq;
using Xunit;

public class DiStrategyTests
{
    public DiStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void ResolutionTest()
    {
        var DiClass = typeof(ClassForTests);

        var mockRotable = new Mock<IRotateble>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Addiction{typeof(IRotateble)}", (object[] args) => mockRotable.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Addiction{typeof(int)}", (object[] args) => (object)180).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Addiction{typeof(string)}", (object[] args) => (object)"ship1").Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Resolution", (object[] args) => new DiStrategy()).Execute();

        var activator = (ClassForTests)IoC.Resolve<DiStrategy>("Resolution").Run((object)DiClass)!;

        Assert.Equal("ship1", activator.Name);
        Assert.Equal(180, activator.Count);
        Assert.Equal(mockRotable.Object, activator.Rotateble);

    }

    [Fact]
    public void GetNullClassReturnNullReferenceException()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Resolution", (object[] args) => new DiStrategy()).Execute();

        Assert.Throws<NullReferenceException>(() => (ClassForTests)IoC.Resolve<DiStrategy>("Resolution").Run((object)null!)!);

    }
}
