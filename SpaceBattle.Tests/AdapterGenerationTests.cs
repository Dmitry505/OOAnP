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
    public void AdapterTest()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AdapterGeneration", (object[] args) => new AdapterGeneration().Run(args)).Execute();

        var type = typeof(IRotateble);

        var result = IoC.Resolve<string>("AdapterGeneration", type);

        var expected = @"public class IRotatebleAdapter : IRotateble
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
            _obj.SetProperty(""Angle"", _obj);
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
}
