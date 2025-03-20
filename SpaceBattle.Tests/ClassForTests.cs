public class ClassForTests
{
    public readonly string Name;
    public readonly int Count;
    public readonly IRotateble Rotateble;
    public ClassForTests(string Name, int Count, IRotateble Rotateble)
    {
        this.Name = Name;
        this.Count = Count;
        this.Rotateble = Rotateble;
    }
}
