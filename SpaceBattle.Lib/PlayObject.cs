public class PlayObject : IUObject
{
    private readonly Dictionary<string, object> _properties = new();

    public void SetProperty(string key, object value) => _properties[key] = value;
    public object GetProperty(string key) => _properties[key];
}
