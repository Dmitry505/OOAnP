global using Hwdtech;

public class DiResolution
{
    public readonly Type structure;
    public DiResolution(Type type) 
    { 
        structure = type;
    }

    public object? Resolution()
    {
        var constructor = structure.GetConstructors()[0];
        var property = constructor.GetParameters();
        property.Select(m => IoC.Resolve<object>($"Addiction{m.ParameterType}"));
        var activator = Activator.CreateInstance(structure, property.ToArray());
        return activator;
    }
}
