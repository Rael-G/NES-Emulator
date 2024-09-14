namespace Cpu;

public delegate byte Operation();
public delegate byte AddressingMode();

public struct Instruction(string name, Operation operation, AddressingMode addressingMode, int cycles)
{
    public string Name = name;
    public Operation Operation = operation;
    public AddressingMode AddressingMode = addressingMode;
    public int Cycles = cycles;
}
