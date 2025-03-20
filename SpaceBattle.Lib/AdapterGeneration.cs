using Scriban;

public class AdapterGeneration : IStrategy
{
    private Type? structure;
    public object Run(params object[] args)
    {
        structure = (Type)args[0];
        var template = IoC.Resolve<Template>("AdapterSample");
        var final = template.Render(new
        {
            structure_name = structure.Name,
            properties = structure.GetProperties().ToList()
        });
        return final;
    }
}
