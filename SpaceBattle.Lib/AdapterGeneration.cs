using Scriban;

public class AdapterGeneration : IStrategy
{
    private Type? structure;
    public object Run(params object[] args)
    {
        structure = (Type)args[0];
        var template = Template.Parse(@"public class {{ structure_name }}Adapter : {{ structure_name }}
{
    private IUObject _obj;
    public {{ structure_name }}Adapter(IUObject obj) => _obj = obj;{{for property in (properties)}}
    public {{property.property_type.name}} {{property.name}}
    {
    {{if property.can_read}}
    get
    {
        return ({{property.property_type.name}})_obj.GetProperty(""{{property.name}}"");
    }{{end}}
    {{if property.can_write}}
    set
    {
        _obj.SetProperty(""{{property.name}}"", _obj);
    }{{end}}
    }
{{end}}
}");
        var final = template.Render(new
        {
            structure_name = structure.Name,
            properties = structure.GetProperties().ToList()
        });
        return final;
    }
}
