namespace Cpu;

internal static class Lookup
{
    public static Instruction[] Map { private get; set; } = [];

    public static byte Opcode { get; set; }

    public static Instruction Current => Map[Opcode];
}
