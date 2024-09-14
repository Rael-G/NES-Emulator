namespace Cpu;

public partial class Cpu
{
    private byte A;

    private byte X;

    private byte Y;

    private ushort PC;

    private ushort Address;

    private ushort AddressRelative;

    private int Cycles;

    private byte Fetch
    {
        get 
        {
            if (!(CurrentInstruction.AddressingMode == IMP))
                return Memory.Read(Address);
        
            return _fetch;
        }
        set => _fetch = value;
    }

    private bool Complete => Cycles < 1;

    private static Instruction CurrentInstruction => Lookup.Current;

    private byte _fetch;

    public Cpu()
    {
        MapInstructions();
    }

    private void Clock()
    {
        if (Complete)
        {
            Lookup.Opcode = Memory.Read(PC++);
            Process();
        }

        Cycles--;
    }

    private void Process()
    {
        Cycles = CurrentInstruction.Cycles;

        var additionalCycle = CurrentInstruction.AddressingMode();
        additionalCycle &= CurrentInstruction.Operation();

        Cycles += additionalCycle;
    }

    private void NonMaskableInterrupt()
    {
        Interrupt(0xFFFA);
    }

    private void Reset()
    {
        A = 0;
        X = 0;
        Y = 0;
        AddressRelative = 0;
        Address = 0;
        Fetch = 0;
        Stack.Reset();
        Status.Reset();
        PC = Memory.Read16(0xFFFC);

        Cycles = 8;
    }

    private void InterruptRequest()
    {
        if (!Status.HasFlag(StatusFlag.I))
            Interrupt(0xFFFE);
    }

    private void ReturnInterruption()
    {
        Status.Flags = (StatusFlag)Stack.Pop() & ~StatusFlag.B & ~StatusFlag.U;

        PC = Stack.Pop16();
    }

    private void Interrupt(ushort address)
    {
        Stack.Push16(PC);

        Status.SetFlag(StatusFlag.B, false);
        Status.SetFlag(StatusFlag.U | StatusFlag.I, true);

        Stack.Push((byte)Status.Flags);

        PC = Memory.Read16(address);

        Cycles = 7;
    }

    
}
