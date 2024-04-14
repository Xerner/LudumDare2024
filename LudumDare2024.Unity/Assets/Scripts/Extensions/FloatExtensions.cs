public static class FloatExtensions
{
    public static bool IsBetween(this float x, float a, float b)
    {
        return (x - a) * (x - b) < 0;
    }
}
