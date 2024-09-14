namespace Cpu;

internal delegate byte Operation();
internal delegate byte AddressingMode();

internal struct Instruction(string name, Operation operation, AddressingMode addressingMode, int cycles)
{
    public string Name = name;
    public Operation Operation = operation;
    public AddressingMode AddressingMode = addressingMode;
    public int Cycles = cycles;
}
