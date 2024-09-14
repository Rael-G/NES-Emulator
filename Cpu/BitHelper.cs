namespace Cpu;

public static class BitHelper
{
    public static bool IsNegative(int a) => (a & 0x80) != 0;

    /// <summary>
    /// Clear the 8 less significant bits
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static ushort ClearLo(ushort a) => a &= 0xFF00;

    public static bool HasFlag(int a, int b) => (a & b) != 0;

}
