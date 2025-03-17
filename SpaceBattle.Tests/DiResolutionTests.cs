global using Hwdtech;
global using Hwdtech.Ioc;
using Moq;
using Xunit;

public class DiResolutionTests
{
    public DiResolutionTests()
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Addiction{typeof(IRotateble)}", (object[] args) => typeof(IRotateble)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Addiction{typeof(int)}", (object[] args) => (object)180).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Addiction{typeof(string)}", (object[] args) => (object)"ship1").Execute();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Resolution", (object[] args) => new DiResolution((Type)args[0])).Execute();

        var activator = (ClassForTests)IoC.Resolve<DiResolution>("AdapterGeneration", DiClass).Resolution()!;

        Assert.Equal(activator.GetType(), DiClass.GetType());
        Assert.Equal(activator.Position, 180);
        Assert.Equal(activator.Name, "ship1");

    }
}
