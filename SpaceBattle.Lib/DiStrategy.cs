global using Hwdtech;

public class DiStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var structure = (Type)args[0];
        var parameters = structure.GetConstructors()[0].GetParameters();

        var property = parameters.Select(m => IoC.Resolve<object>($"Addiction{m.ParameterType}"));

        return Activator.CreateInstance(structure, property.ToArray())!;
    }
}
