namespace Cpu;

public class Stack
{
    private const int StackBegin = 0x100;
    private const int StackPointerBegin = 0xFD;

    private static byte Pointer = StackPointerBegin;

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
