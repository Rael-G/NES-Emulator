namespace Cpu;

public class Memory
{
    private static readonly byte[] _memory = new byte[0x10000];

    public static byte Read(ushort address) => _memory[address];

    public static ushort Read16(ushort address) 
        => (ushort)(Read(address) | (Read((ushort)(address + 1)) << 8));

    public static void Write(ushort address, byte value) => _memory[address] = value;

    public static void Write16(ushort address, ushort value)
    {
        _memory[address] = (byte)(value >> 8);
        _memory[address + 1] = (byte)value;
    }
}
