namespace Cpu;

public partial class Cpu
{
    /// <summary>
    /// Implied
    /// </summary>
    /// <returns></returns>
    public byte IMP()
    {
        Fetch = A;
        return 0;
    }

    /// <summary>
    /// Immediate
    /// </summary>
    /// <returns></returns>
    public byte IMM()
    {
        Address = PC++;
        return 0;
    }

    /// <summary>
    /// Zero Page
    /// </summary>
    /// <returns></returns>
    public byte ZP0()
    {
        Address = Memory.Read(PC++);
        return 0;
    }

    /// <summary>
    /// Zero Page with X Offset
    /// </summary>
    /// <returns></returns>
    public byte ZPX()
    {
        Address = (byte)(Memory.Read(PC++) + X);
        return 0;
    }

    /// <summary>
    /// Zero Page with Y Offset
    /// </summary>
    /// <returns></returns>
    public byte ZPY()
    {
        Address = (byte)(Memory.Read(PC++) + Y);
        return 0;
    }

    /// <summary>
    /// Absolute 
    /// </summary>
    /// <returns></returns>
    public byte ABS()
    {
        Address = Memory.Read16(PC++);
        PC++;

        return 0;
    }

    /// <summary>
    /// Absolute with X Offset
    /// </summary>
    /// <returns></returns>
    public byte ABX()
    {
        var aux = Memory.Read16(PC++);
        Address = (ushort)(aux + X);
        PC++;

        if (BitHelper.ClearLo(Address) != BitHelper.ClearLo(aux))
            return 1;

        return 0;
    }

    /// <summary>
    /// Absolute with Y Offset
    /// </summary>
    /// <returns></returns>
    public byte ABY()
    {
        var aux = Memory.Read16(PC++);
        Address = (ushort)(aux + Y);
        PC++;

        if (BitHelper.ClearLo(Address) != BitHelper.ClearLo(aux))
            return 1;

        return 0;
    }

    /// <summary>
    /// Indirect
    /// </summary>
    /// <returns></returns>
    public byte IND()
    {
        var ptr = Memory.Read16(PC++);
        PC++;

        if (BitHelper.ClearLo(ptr) == 0x00FF) // Simulate page boundary hardware bug
            Address = (ushort)((Memory.Read(BitHelper.ClearLo(ptr)) << 8) | Memory.Read(ptr));
        else // Behave normally
            Address = (ushort)((Memory.Read((ushort)(ptr + 1)) << 8) | Memory.Read(ptr));

        return 0;
    }

    /// <summary>
    /// Indirect X
    /// </summary>
    /// <returns></returns>
    public byte IZX()
    {
       Address = Memory.Read16((byte)(Memory.Read(PC++) + X));

        return 0;
    }

    /// <summary>
    /// Indirect Y
    /// </summary>
    /// <returns></returns>
    public byte IZY()
    {
        var aux = Memory.Read16(Memory.Read(PC++));
        Address = (ushort)(aux + Y);

        if (BitHelper.ClearLo(Address) != BitHelper.ClearLo(aux))
            return 1;

        return 0;
    }

    /// <summary>
    /// Relative
    /// </summary>
    /// <returns></returns>
    public byte REL()
    {
        AddressRelative = Memory.Read(PC++);

        if (BitHelper.IsNegative(AddressRelative))
            AddressRelative |= 0xFF00;
        
        return 0;
    }
}
