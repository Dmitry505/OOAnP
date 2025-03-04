public class Identification
{
    private readonly Dictionary<int, IUObject> _idObj = new();

    public Dictionary<int, IUObject> SetId()
    {
        var obj = IoC.Resolve<List<IUObject>>("Game.Ships.All");

        for (var i = 0; i < obj.Count; i++)
        {
            _idObj[i] = obj[i];
        }

        return _idObj;
    }
}

