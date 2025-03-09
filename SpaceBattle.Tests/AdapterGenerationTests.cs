public class AdapterGenerationTests
{
    public AdapterGenerationTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulAdapterStringTest()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AdapterGeneration", (object[] args) => new AdapterGeneration().Run(args)).Execute();

        var type = typeof(IRotateble);

        var result = IoC.Resolve<string>("AdapterGeneration", type);

        var expected = @"using System;
public class IRotatebleAdapter : IRotateble
{
    private IUObject _obj;
    public IRotatebleAdapter(IUObject obj) => _obj = obj;
    public Object Angle
    {
    
        get
        {
            return (Object)_obj.GetProperty(""Angle"");
        }
    
        set
        {
            _obj.SetProperty(""Angle"", value);
        }
    }
    public Object AngularSpeed
    {
    
        get
        {
            return (Object)_obj.GetProperty(""AngularSpeed"");
        }
    
    }
}";
        expected = expected.Replace("\r\n", "\n").Replace("\n\n", "\n");
        result = result.Replace("\r\n", "\n").Replace("\n\n", "\n");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SetNullAdapterTest()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AdapterGeneration", (object[] args) => new AdapterGeneration().Run(args)).Execute();
        Assert.Throws<NullReferenceException>(() => IoC.Resolve<string>("AdapterGeneration", null!));
    }
}
