public class ClassForTests
{
    public readonly int Position;
    public readonly IRotateble Rotable;
    public readonly string Name;
    public ClassForTests(IRotateble Rotable, int Position, string Name)
    {
        this.Rotable = Rotable;
        this.Position = Position;
        this.Name = Name;
    }
}
