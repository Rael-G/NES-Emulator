namespace Cpu;

[Flags]
internal enum StatusFlag
{
    None = 0,
    
    /// <summary>
    /// Carry
    /// </summary>
    C = 1 << 0,

    Carry = C,

    /// <summary>
    /// Zero
    /// </summary>
    Z = 1 << 1,

    Zero = Z,

    /// <summary>
    /// Interrupt Disable
    /// </summary>
    I = 1 << 2,

    InterruptDisable = I,

    /// <summary>
    /// Decimal
    /// </summary>
    D = 1 << 3,

    Decimal = D,

    /// <summary>
    /// Break
    /// </summary>
    B = 1 << 4,

    Break = B,

    /// <summary>
    /// Unused
    /// </summary>
    U = 1 << 5,

    Unused = U,

    /// <summary>
    /// Overflow
    /// </summary>
    V = 1 << 6,

    Overflow = V,

    /// <summary>
    /// Negative
    /// </summary>
    N = 1 << 7,

    Negative = N,
}
