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

    [Fact]
    public void SuccessfulAdapterWorkTest()
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
            var adapterType = assembly.GetType("IRotatebleAdapter");

            var mockUObject = new Mock<IUObject>();
            mockUObject.Setup(m => m.GetProperty("Angle")).Returns(45);

            var adapterInstance = (IRotateble)Activator.CreateInstance(adapterType, mockUObject.Object);

            var raelAngel = adapterInstance.Angle;
            mockUObject.Verify(m => m.GetProperty("Angle"), Times.Once);
            Assert.Equal("GetProperty", mockUObject.Invocations[0].Method.Name);
            Assert.Equal(45, raelAngel);

            adapterInstance.Angle = 90;
            mockUObject.Verify(m => m.SetProperty("Angle", 90), Times.Once);
            Assert.Equal("SetProperty", mockUObject.Invocations[1].Method.Name);
            Assert.Equal(90, mockUObject.Invocations[1].Arguments[1]);

        }
    }
}
