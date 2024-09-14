namespace Cpu;

internal class Stack
{
    public static byte Pointer { get; set; } = StackPointerBegin;

    private const int StackBegin = 0x100;
    private const int StackPointerBegin = 0xFD;

    public static void Reset() => Pointer = StackPointerBegin;

    public static byte Pop() => Memory.Read((ushort)(StackBegin + ++Pointer));

    public static ushort Pop16()
    {
        var aux = Memory.Read16((ushort)(StackBegin + ++Pointer));
        Pointer++;
        return aux;
    }

    public static void Push(byte value) 
        => Memory.Write((ushort)(StackBegin + Pointer--), value);

    public static void Push16(ushort value)
    {
        Push((byte)(value >> 8));
        Push((byte)value);
    }
}
