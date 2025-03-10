using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public class AdapterWorkTests
{
    public AdapterWorkTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    private static Type CompileAdapter()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AdapterGeneration", (object[] args) => new AdapterGeneration().Run(args)).Execute();

        var type = typeof(IRotateble);
        var result = IoC.Resolve<string>("AdapterGeneration", type);
        var code = result;

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IUObject).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Moq.Mock).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("SpaceBattle.Lib").Location)
        };

        var compilation = CSharpCompilation.Create("DynamicAssembly")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);

        using (var ms = new MemoryStream())
        {
            compilation.Emit(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            return assembly.GetType("IRotatebleAdapter");
        }
    }

    [Fact]
    public void GetPropertyTest()
    {
        var adapterType = CompileAdapter();

        var mockUObject = new Mock<IUObject>();
        mockUObject.Setup(m => m.GetProperty("Angle")).Returns(45);
        mockUObject.Setup(m => m.GetProperty("AngularSpeed")).Returns(60);

        var adapterInstance = (IRotateble)Activator.CreateInstance(adapterType, mockUObject.Object);

        var raelAngel = adapterInstance.Angle;
        mockUObject.Verify(m => m.GetProperty("Angle"), Times.Once);
        Assert.Equal("GetProperty", mockUObject.Invocations[0].Method.Name);
        Assert.Equal(45, raelAngel);

        var raelAngularSpeed = adapterInstance.AngularSpeed;
        mockUObject.Verify(m => m.GetProperty("AngularSpeed"), Times.Once);
        Assert.Equal(60, raelAngularSpeed);

    }

    [Fact]
    public void SetAngleTest()
    {
        var adapterType = CompileAdapter();

        var mockUObject = new Mock<IUObject>();
        mockUObject.Setup(m => m.SetProperty("Angle", It.IsAny<object>()));

        var adapterInstance = (IRotateble)Activator.CreateInstance(adapterType, mockUObject.Object);

        adapterInstance.Angle = 90;
        mockUObject.Verify(m => m.SetProperty("Angle", 90), Times.Once);
        Assert.Equal("SetProperty", mockUObject.Invocations[0].Method.Name);
        Assert.Equal(90, mockUObject.Invocations[0].Arguments[1]);
    }

    [Fact]
    public void GetAngleWhenAngleNotSetShouldThrowException()
    {
        var adapterType = CompileAdapter();

        var mockUObject = new Mock<IUObject>();
        mockUObject.Setup(m => m.GetProperty("Angle")).Throws(new Exception("Angle not set"));

        var adapterInstance = (IRotateble)Activator.CreateInstance(adapterType, mockUObject.Object);

        var exception = Assert.Throws<Exception>(() => adapterInstance.Angle);
        Assert.Equal("Angle not set", exception.Message);
    }

    [Fact]
    public void GetAngleNullValueShouldThrowException()
    {
        var adapterType = CompileAdapter();

        var mockUObject = new Mock<IUObject>();
        mockUObject.Setup(m => m.SetProperty("Angle", null))
            .Throws(new ArgumentNullException());

        var adapterInstance = (IRotateble)Activator.CreateInstance(adapterType, mockUObject.Object);

        Assert.Throws<ArgumentNullException>(() => adapterInstance.Angle = null);
    }

    [Fact]
    public void Adapter_Should_Be_Of_Correct_Type()
    {
        var adapterType = CompileAdapter();

        var mockUObject = new Mock<IUObject>();
        var adapterInstance = Activator.CreateInstance(adapterType, mockUObject.Object);

        var actualType = adapterInstance.GetType();

        Assert.Equal(adapterType, actualType);
    }
}
