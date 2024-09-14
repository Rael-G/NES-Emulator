namespace Cpu;

public partial class Cpu
{
    /// <summary>
    /// Add with Carry In, A = A + M + C, Flags C, V, N, Z
    /// </summary>
    /// <returns></returns>
    private byte ADC()
    {
        var result = A + Fetch + (byte)(Status.Flags & StatusFlag.C);

        Status.SetFlag(StatusFlag.C, result > byte.MaxValue);
        Status.SetZeroAndNegative(result);
        Status.SetFlag(StatusFlag.V, BitHelper.HasFlag(A ^ result, ~(A ^ Fetch)));

        A = (byte)result;

        return 1;
    }

    /// <summary>
    /// Bitwise AND, A = A & M, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte AND()
    {
        A &= Fetch;
        Status.SetZeroAndNegative(A);
        
        return 1;
    }

    /// <summary>
    /// Arithmetic Shift Left, A = C <- (A << 1) <- 0, N, Z, C
    /// </summary>
    /// <returns></returns>
    private byte ASL()
    {
        var result = Fetch << 1;
        Status.SetFlag(StatusFlag.C, result > byte.MaxValue);
        Status.SetZeroAndNegative(result);

        if (CurrentInstruction.AddressingMode == IMP)
        {
            A = (byte)result;
            return 0;
        }

        Memory.Write(Address, (byte)result);
        return 0;
    }

    /// <summary>
    /// Branch if Carry Clear, if !C,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BCC()
    {
        if (!Status.HasFlag(StatusFlag.C))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if (Address != PC)
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Branch if Carry Set, if C,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BCS()
    {
        if (Status.HasFlag(StatusFlag.C))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if (Address != PC)
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Branch if Equal, if Z,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BEQ()
    {
        if (Status.HasFlag(StatusFlag.Z))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if (Address != PC)
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Bitwise AND
    /// </summary>
    /// <returns></returns>
    private byte BIT()
    {
        var result = A & Fetch;
        Status.SetZeroAndNegative(result);
        Status.SetFlag(StatusFlag.V, BitHelper.HasFlag(Fetch, 1 << 6));

        return 0;
    }

    /// <summary>
    /// Branch if Negative, if N,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BMI()
    {
        if (Status.HasFlag(StatusFlag.N))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if (Address != PC)
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Branch if Not Equal, if !Z,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BNE()
    {
        if (Status.HasFlag(StatusFlag.Z))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if ((Address & 0xFF00) != (PC & 0xFF00))
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Branch if Positive, if !N,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BPL()
    {
        if (!Status.HasFlag(StatusFlag.N))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if ((Address & 0xFF00) != (PC & 0xFF00))
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Break, Program Sourced Interrupt
    /// </summary>
    /// <returns></returns>
    private byte BRK()
    {
        PC++;

        Status.SetFlag(StatusFlag.I, true);
        Stack.Push16(PC);

        Status.SetFlag(StatusFlag.B, true);
        Stack.Push((byte)Status.Flags);
        Status.SetFlag(StatusFlag.B, false);

        PC = Memory.Read16(0xFFFE);

        return 0;
    }

    /// <summary>
    /// Branch if Overflow Clear, if !V,  <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BVC()
    {
        if (!Status.HasFlag(StatusFlag.V))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if ((Address & 0xFF00) != (PC & 0xFF00))
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Branch if Overflow Set, if V, <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte BVS()
    {
        if (Status.HasFlag(StatusFlag.V))
        {
            Cycles++;
            Address = (ushort)(PC + AddressRelative);

            if ((Address & 0xFF00) != (PC & 0xFF00))
                Cycles++;

            PC = Address;
        }

        return 0;
    }

    /// <summary>
    /// Clear Carry Flag, C = 0
    /// </summary>
    /// <returns></returns>
    private byte CLC()
    {
        Status.SetFlag(StatusFlag.C, false);
        return 0;
    }

    /// <summary>
    /// Clear Decimal Flag, D = 0
    /// </summary>
    /// <returns></returns>
    private byte CLD()
    {
        Status.SetFlag(StatusFlag.D, false);
        return 0;
    }

    /// <summary>
    /// Clear Interrupt Flag, I = 0
    /// </summary>
    /// <returns></returns>
    private byte CLI()
    {
        Status.SetFlag(StatusFlag.I, false);
        return 0;
    }

    /// <summary>
    /// Clear Overflow Flag, V = 0
    /// </summary>
    /// <returns></returns>
    private byte CLV()
    {
        Status.SetFlag(StatusFlag.V, false);
        return 0;
    }

    /// <summary>
    /// Compare Accumulator, C <- A >= M      Z <- (A - M) == 0, Flags  N, C, Z
    /// </summary>
    /// <returns></returns>
    private byte CMP()
    {
        var result = A - Fetch;
        Status.SetFlag(StatusFlag.C, A >= Fetch);
        Status.SetZeroAndNegative(result);

        return 1;
    }

    /// <summary>
    /// Compare X Register, C <- X >= M     Z <- (X - M) == 0, Flags N, C, Z
    /// </summary>
    /// <returns></returns>
    private byte CPX()
    {
        var result = X - Fetch;
        Status.SetFlag(StatusFlag.C, X >= Fetch);
        Status.SetZeroAndNegative(result);

        return 0;
    }

    /// <summary>
    /// Compare Y Register, C <- Y >= M     Z <- (Y - M) == 0, Flags N, C, Z
    /// </summary>
    /// <returns></returns>
    private byte CPY()
    {
        var result = Y - Fetch;
        Status.SetFlag(StatusFlag.C, Y >= Fetch);
        Status.SetZeroAndNegative(result);

        return 0;
    }

    /// <summary>
    /// Decrement Value at Memory Location, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte DEC()
    {
        var result = Fetch - 1;
        Memory.Write(Address, (byte)result);
        Status.SetZeroAndNegative(result);

        return 0;
    }

    /// <summary>
    /// Decrement X Register, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte DEX()
    {
        X--;
        Status.SetZeroAndNegative(X);

        return 0;
    }

    /// <summary>
    /// Decrement Y Register, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte DEY()
    {
        Y--;
        Status.SetZeroAndNegative(X);

        return 0;
    }

    /// <summary>
    /// Bitwise Logic XOR, A = A ^ M, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte EOR()
    {
        A = (byte)(A ^ Fetch);
        Status.SetZeroAndNegative(A);

        return 1;
    }

    /// <summary>
    /// Increment Value at Memory Location, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte INC()
    {
        var result = Fetch + 1;
        Memory.Write(Address, (byte)result);
        Status.SetZeroAndNegative(result);

        return 0;
    }

    /// <summary>
    /// Increment X Register, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte INX()
    {
        X++;
        Status.SetZeroAndNegative(X);

        return 0;
    }

    /// <summary>
    /// Increment Y Register, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte INY()
    {
        Y++;
        Status.SetZeroAndNegative(Y);

        return 0;
    }

    /// <summary>
    /// Jump To Location, <see cref="PC"/> = address
    /// </summary>
    /// <returns></returns>
    private byte JMP()
    {
        PC = Address;

        return 0;
    }

    /// <summary>
    /// Jump To Sub-Routine, Push current <see cref="PC"/> to stack, PC = address
    /// </summary>
    /// <returns></returns>
    private byte JSR()
    {
        Stack.Push16(--PC);
        PC = Address;

        return 0;
    }

    /// <summary>
    /// Load The Accumulator, A = M, Flags, N, Z
    /// </summary>
    /// <returns></returns>
    private byte LDA()
    {
        A = Fetch;
        Status.SetZeroAndNegative(A);

        return 1;
    }

    /// <summary>
    /// Load The X Register, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte LDX()
    {
        X = Fetch;
        Status.SetZeroAndNegative(X);

        return 1;
    }

    /// <summary>
    /// Load The Y Register, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte LDY()
    {
        Y = Fetch;
        Status.SetZeroAndNegative(Y);

        return 1;
    }

    /// <summary>
    /// Logical Shift Right, Flags C, N, Z
    /// </summary>
    /// <returns></returns>
    private byte LSR()
    {
        var result = Fetch >> 1;
        Status.SetFlag(StatusFlag.C, (Fetch & 1) > 0);
        Status.SetZeroAndNegative(result);

        if (CurrentInstruction.AddressingMode == IMP)
        {
            A = (byte)result;
            return 0;
        }
        
        Memory.Write(Address, (byte)result);
        return 0;
    }

    /// <summary>
    /// No Operation, illegal opcodes or undocumented opcodes, are CPU instructions 
    /// that were officially left unused by the original design, some of these 
    /// instructions are useful, some are not predictable, some do nothing but 
    /// burn cycles, some halt the CPU until reset. An accurate NES emulator 
    /// must implement all instructions, not just the official ones. A small 
    /// number of games use them.
    /// <seealso cref="https://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes"/>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private byte NOP()
    {
        // https://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes
        switch (Lookup.Opcode) 
        {
            case 0x1C:
                throw new NotImplementedException();
            case 0x3C:
                throw new NotImplementedException();
            case 0x5C:
                throw new NotImplementedException();
            case 0x7C:
                throw new NotImplementedException();
            case 0xDC:
                throw new NotImplementedException();
            case 0xFC:
                return 1;
        }

        return 0;
    }

    /// <summary>
    /// Bitwise Logic OR, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte ORA()
    {
        A |= Fetch;
        Status.SetZeroAndNegative(A);

        return 1;
    }

    /// <summary>
    /// Push Accumulator to Stack
    /// </summary>
    /// <returns></returns>
    private byte PHA()
    {
        Stack.Push(A);

        return 0;
    }

    /// <summary>
    /// Push Status Register to Stack
    /// </summary>
    /// <returns></returns>
    private byte PHP()
    {
        Stack.Push((byte)(Status.Flags | StatusFlag.B | StatusFlag.U));
        Status.SetFlag(StatusFlag.B | StatusFlag.U, false);

        return 0;
    }

    /// <summary>
    /// Pop Accumulator off Stack, Flags N, Z
    /// </summary>
    /// <returns></returns>
    private byte PLA()
    {
        A = Stack.Pop();
        Status.SetZeroAndNegative(A);

        return 0;
    }

    private byte PLP() => throw new NotImplementedException();
    private byte ROL() => throw new NotImplementedException();
    private byte ROR() => throw new NotImplementedException();
    private byte RTI() => throw new NotImplementedException();
    private byte RTS() => throw new NotImplementedException();

    /// <summary>
    /// Subtraction with Borrow In, A = A - M - (1 - C), Flags C, V, N, Z
    /// </summary>
    /// <returns></returns>
    private byte SBC()
    {
        var inverseFetch = Fetch ^ 0xFF;

        var result = A + inverseFetch + Status.GetFlag(StatusFlag.C);

        Status.SetFlag(StatusFlag.C, result > byte.MaxValue);
        Status.SetZeroAndNegative(result);
        Status.SetFlag(StatusFlag.V, BitHelper.HasFlag(result ^ A, result ^ inverseFetch));

        A = (byte)result;

        return 1;
    }

    private byte SEC() => throw new NotImplementedException();
    private byte SED() => throw new NotImplementedException();
    private byte SEI() => throw new NotImplementedException();
    private byte STA() => throw new NotImplementedException();
    private byte STX() => throw new NotImplementedException();
    private byte STY() => throw new NotImplementedException();
    private byte TAX() => throw new NotImplementedException();
    private byte TAY() => throw new NotImplementedException();
    private byte TSX() => throw new NotImplementedException();
    private byte TXA() => throw new NotImplementedException();
    private byte TXS() => throw new NotImplementedException();
    private byte TYA() => throw new NotImplementedException();

    private byte ERR() => throw new NotImplementedException();
}
