namespace Cpu;

internal static class Status
{
    public static StatusFlag Flags { get; set; }

    public static void Reset() => Flags = StatusFlag.None | StatusFlag.U;

    public static bool HasFlag(StatusFlag flag) => Flags.HasFlag(flag);

    public static byte GetFlag(StatusFlag flag) => (byte)(HasFlag(flag)? 1 : 0);

    public static void SetFlag(StatusFlag flag, bool condition)
    {
        if (condition) Flags |= flag;
        else Flags &= ~flag;
    }

    public static void SetZeroAndNegative(int value)
    {
        SetFlag(StatusFlag.Z, (byte)value == 0);
        SetFlag(StatusFlag.N, BitHelper.IsNegative(value));
    }
}
