using Hwdtech;

public class Identification
{
    private Dictionary<string, IUObject> _idObj = new();

    public Dictionary<string, IUObject> SetId()
    {
        var obj = IoC.Resolve<List<IUObject>>("Game.Ships.All");

        _idObj = obj.ToDictionary(item => Guid.NewGuid().ToString(), item => item);

        return _idObj;
    }
}

