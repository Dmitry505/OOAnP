public class PositionIterator
{
    public readonly int length;

    public PositionIterator(int length) => this.length = length;

    public IEnumerator<dynamic> GetEnumerator()
    {
        var firstLineCount = (length + 1) / 2;
        var secondLineCount = length / 2;

        for (var i = 0; i < firstLineCount; i++)
        {
            yield return new { X = 10, Y = 10 + i * 10 };

        }

        for (var i = 0; i < secondLineCount; i++)
        {
            yield return new { X = 100, Y = 10 + i * 10 };
        }
    }
}

